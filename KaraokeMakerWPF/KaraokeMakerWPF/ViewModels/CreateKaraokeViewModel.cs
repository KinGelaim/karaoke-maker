using KaraokeMakerWPF.Environment;
using KaraokeMakerWPF.Models;
using Microsoft.Win32;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Input;

namespace KaraokeMakerWPF.ViewModels;

public sealed class CreateKaraokeViewModel : StepByStepViewModelBase
{
    public KaraokeInfoViewModel KaraokeInfoVM { get; init; }

    public ICommand SelectOutputFolderCommand { get; }
    public ICommand SelectFfmpegCommand { get; }
    public ICommand CreateKaraokeVideoCommand { get; }

    private string _outputFolderLabelText = string.Empty;
    public string OutputFolderLabelText
    {
        get => _outputFolderLabelText;
        set
        {
            _outputFolderLabelText = value;
            OnPropertyChanged(nameof(OutputFolderLabelText));
        }
    }

    private string _ffmpegLabelText = string.Empty;
    public string FfmpegLabelText
    {
        get => _ffmpegLabelText;
        set
        {
            _ffmpegLabelText = value;
            OnPropertyChanged(nameof(FfmpegLabelText));
        }
    }

    public CreateKaraokeViewModel(KaraokeInfoViewModel karaokeInfoVM)
    {
        KaraokeInfoVM = karaokeInfoVM;

        SelectOutputFolderCommand = new DelegateCommand(SelectOutputFolder);
        SelectFfmpegCommand = new DelegateCommand(SelectFfmpeg);
        CreateKaraokeVideoCommand = new DelegateCommand(CreateKaraokeVideo);
    }

    private void SelectOutputFolder()
    {
        var outputFolderDialog = new OpenFolderDialog();

        if (outputFolderDialog.ShowDialog() == true)
        {
            OutputFolderLabelText = outputFolderDialog.FolderName;
        }
    }

    private void SelectFfmpeg()
    {
        var ffmpegDialog = new OpenFileDialog
        {
            Filter = "Исполняемый файл (*.exe)|*.exe"
        };

        if (ffmpegDialog.ShowDialog() == true)
        {
            FfmpegLabelText = ffmpegDialog.FileName;
        }
    }

    private void CreateKaraokeVideo()
    {
        var ffmpegPath = FfmpegLabelText;
        var imagePath = KaraokeInfoVM.ImageFilePath;
        var musicPath = KaraokeInfoVM.MusicFilePath;

        // Шрифт должен лежать возле либы (TODO: поправить на абсолютный путь?)
        var fontFileName = Path.GetFileName(KaraokeInfoVM.FontFilePath);

        var fileName = $"Karaoke_{Guid.NewGuid()}";
        var outputFileName = $"{fileName}.mp4";
        var outputPath = $"{OutputFolderLabelText}\\{outputFileName}";

        var textInfo = string.Empty;
        for (int i = 0; i < KaraokeInfoVM.SongLines.Count; i++)
        {
            var currentSongLine = KaraokeInfoVM.SongLines[i];
            var nextSongLine = i < KaraokeInfoVM.SongLines.Count - 1
                ? KaraokeInfoVM.SongLines[i + 1]
                : null;

            textInfo += CreateSongLineCode(
                fontFileName,
                currentSongLine.Text,
                currentSongLine.StartTime,
                currentSongLine.EndTime,
                "red",
                i % 2 == 1);

            if (nextSongLine != null)
            {
                textInfo += CreateSongLineCode(
                    fontFileName,
                    nextSongLine.Text,
                    currentSongLine.StartTime,
                    currentSongLine.EndTime,
                    "white",
                    i % 2 == 0);
            }
        }
        textInfo = textInfo.TrimEnd(',');

        var command = $"@chcp 65001\n\r\"" + ffmpegPath + "\" -loop 1 -i \"" + imagePath + "\" -i \"" + musicPath + "\" -shortest -vf \"" + textInfo + "\" -codec:a copy \"" + outputPath + "\" -y";

        var commandFilePath = $"{OutputFolderLabelText}\\{fileName}.bat";
        File.WriteAllText(commandFilePath, command);

        var startInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/k \"{commandFilePath}\""
        };

        using var process = Process.Start(startInfo);
        process.WaitForExit();
    }

    private string CreateSongLineCode(
        string fontFileName,
        string text,
        double startTime,
        double endTime,
        string color,
        bool needOffset)
    {
        var offset = needOffset
            ? "text_h + 20"
            : "0";
        return $"drawtext=fontfile={fontFileName}:text='{text}':fontcolor={color}:fontsize=24:box=1:boxcolor=black@0.5:boxborderw=5:x=(w-text_w)/2:y=(h-text_h)/2 + {offset}:enable='between(t,{startTime.ToString(CultureInfo.InvariantCulture)},{endTime.ToString(CultureInfo.InvariantCulture)})',";
    }

    public override StepByStepValidationError ValidateBeforeNextStep()
    {
        if (string.IsNullOrWhiteSpace(OutputFolderLabelText))
        {
            return StepByStepValidationError.Error("Необходимо выбрать директорию для создания Караоке!");
        }

        if (string.IsNullOrWhiteSpace(FfmpegLabelText))
        {
            return StepByStepValidationError.Error("Необходимо выбрать файл Ffmpeg для создания Караоке!");
        }

        return StepByStepValidationError.Success();
    }
}