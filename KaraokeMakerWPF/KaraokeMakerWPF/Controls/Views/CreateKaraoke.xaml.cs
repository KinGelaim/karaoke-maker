using KaraokeMakerWPF.ViewModels;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace KaraokeMakerWPF.Controls.Views;

/// <summary>
/// Логика взаимодействия для CreateKaraoke.xaml
/// </summary>
public partial class CreateKaraoke : UserControl
{
    public static readonly DependencyProperty KaraokeInfoProperty =
        DependencyProperty.Register(
            nameof(KaraokeInfoVM),
            typeof(KaraokeInfoViewModel),
            typeof(CreateKaraoke),
            new PropertyMetadata(null));

    public KaraokeInfoViewModel KaraokeInfoVM
    {
        get { return (KaraokeInfoViewModel)GetValue(KaraokeInfoProperty); }
        set { SetValue(KaraokeInfoProperty, value); }
    }

    public CreateKaraoke()
    {
        InitializeComponent();
    }

    private void SelectOutputFolderBtn_Click(object sender, RoutedEventArgs e)
    {
        var outputFolderDialog = new OpenFolderDialog();

        if (outputFolderDialog.ShowDialog() == true)
        {
            OutputFolderLabel.Content = outputFolderDialog.FolderName;
        }
    }

    private void SelectFfmpegBtn_Click(object sender, RoutedEventArgs e)
    {
        var ffmpegDialog = new OpenFileDialog
        {
            Filter = "Исполняемый файл (*.exe)|*.exe"
        };

        if (ffmpegDialog.ShowDialog() == true)
        {
            FfmpegLabel.Content = ffmpegDialog.FileName;
        }
    }

    private void CreateKaraokeVideoBtn_Click(object sender, RoutedEventArgs e)
    {
        var ffmpegPath = FfmpegLabel.Content.ToString();
        var imagePath = KaraokeInfoVM.ImageFilePath;
        var musicPath = KaraokeInfoVM.MusicFilePath;

        // Шрифт должен лежать возле либы (TODO: поправить на абсолютный путь?)
        var fontFileName = Path.GetFileName(KaraokeInfoVM.FontFilePath);

        var fileName = $"Karaoke_{Guid.NewGuid()}.mp4";
        var outputPath = $"{OutputFolderLabel.Content}\\{fileName}";

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

        var command = ffmpegPath + " -loop 1 -i \"" + imagePath + "\" -i \"" + musicPath + "\" -shortest -vf \"" + textInfo + "\" -codec:a copy \"" + outputPath + "\" -y";
        var process = Process.Start("cmd.exe", @"/c " + command);
        process.WaitForExit();
    }

    private string CreateSongLineCode(
        string fontFileName,
        string text,
        long startTime,
        long endTime,
        string color,
        bool needOffset)
    {
        var offset = needOffset
            ? "text_h + 20"
            : "0";
        return $"drawtext=fontfile={fontFileName}:text='{text}':fontcolor={color}:fontsize=24:box=1:boxcolor=black@0.5:boxborderw=5:x=(w-text_w)/2:y=(h-text_h)/2 + {offset}:enable='between(t,{startTime},{endTime})',";
    }
}
