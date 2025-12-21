using KaraokeMakerWPF.Environment;
using KaraokeMakerWPF.Models;
using Microsoft.Win32;
using System;
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

        var fontFileName = KaraokeInfoVM.FontFilePath;

        var fileName = $"Karaoke_{Guid.NewGuid()}";
        var outputExtension = "mp4";

        // 1. Создаём видео
        var tempVideoOutputPath = $"{OutputFolderLabelText}\\{fileName}_0.{outputExtension}";
        var createVideoCommand = "@chcp 65001\n\r\"" + ffmpegPath + "\" -loop 1 -i \"" + imagePath + "\" -i \"" + musicPath + "\" -shortest -vf \"scale=1920:1080\" -codec:a copy \"" + tempVideoOutputPath + "\" -y";

        var createVideoCommandFilePath = $"{OutputFolderLabelText}\\{fileName}_0.bat";
        File.WriteAllText(createVideoCommandFilePath, createVideoCommand);

        var startInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/c \"{createVideoCommandFilePath}\""
        };

        using var createVideoProcess = Process.Start(startInfo);
        createVideoProcess?.WaitForExit();

        // 2. Разбиваем всю песню кусками на N строк
        var chunkSize = 30;
        var chunks = KaraokeInfoVM.SongLines.Chunk(chunkSize);

        var currentChunk = 0;
        foreach (var chunk in chunks)
        {
            // 3. Добавляем в видео по N строк песни
            var textInfo = string.Empty;
            for (int i = 0; i < chunk.Length; i++)
            {
                var currentSongLine = chunk[i];
                var nextSongLine = i < chunk.Length - 1
                    ? chunk[i + 1]
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

            var inputPath = $"{OutputFolderLabelText}\\{fileName}_{currentChunk}.{outputExtension}";
            var outputPath = $"{OutputFolderLabelText}\\{fileName}_{++currentChunk}.{outputExtension}";

            var command = "@chcp 65001\n\r\"" + ffmpegPath + $"\" -i \"{inputPath}\"" + " -vf \"" + textInfo + "\" -codec:a copy \"" + outputPath + "\" -y";

            var commandFilePath = $"{OutputFolderLabelText}\\{fileName}_{currentChunk}.bat";
            File.WriteAllText(commandFilePath, command);

            startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c \"{commandFilePath}\""
            };

            using var process = Process.Start(startInfo);
            process?.WaitForExit();
        }
    }

    private string CreateSongLineCode(
        string fontFileName,
        string text,
        double startTime,
        double endTime,
        string color,
        bool needOffset)
    {
        var fontFilePath = fontFileName.Replace("\\", "/").Replace(":", "\\:");

        var normalizedText = TextNormalization(text);

        var startTimeStr = Math.Round(startTime, 3).ToString(CultureInfo.InvariantCulture);
        var endTimeStr = Math.Round(endTime, 3).ToString(CultureInfo.InvariantCulture);

        var offset = needOffset
            ? "text_h+20"
            : "0";
        return $"drawtext=fontfile='{fontFilePath}':text='{normalizedText}':fontcolor={color}:fontsize=(h/21):box=1:boxcolor=black@0.5:boxborderw=5:x=(w-text_w)/2:y=(h-text_h)/2+{offset}:enable='between(t,{startTimeStr},{endTimeStr})',";
    }

    public string TextNormalization(string text)
    {
        return text
            .Replace("\"", "\\\"")
            .Replace("'", "\'")
            .Replace("\\", "\\\\");
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