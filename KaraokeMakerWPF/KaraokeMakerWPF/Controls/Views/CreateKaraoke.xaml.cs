using KaraokeMakerWPF.Models;
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
        DependencyProperty.Register("KaraokeInfo", typeof(KaraokeInfo), typeof(CreateKaraoke), new PropertyMetadata(null));

    public KaraokeInfo KaraokeInfo
    {
        get { return (KaraokeInfo)GetValue(KaraokeInfoProperty); }
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
        var ffmpegPath = FfmpegLabel.Content;
        var imagePath = KaraokeInfo.ImageFilePath;
        var musicPath = KaraokeInfo.MusicFilePath;

        // Шрифт должен лежать возле либы (TODO: поправить на абсолютный путь?)
        var fontFileName = Path.GetFileName(KaraokeInfo.FontFilePath);

        var fileName = $"Karaoke_{Guid.NewGuid()}.mp4";
        var outputPath = $"{OutputFolderLabel.Content}\\{fileName}";

        var textInfo = string.Empty;
        foreach (var songLine in KaraokeInfo.SongLines)
        {
            textInfo += $"drawtext=fontfile={fontFileName}:text='{songLine.Text}':fontcolor=white:fontsize=24:box=1:boxcolor=black@0.5:boxborderw=5:x=(w-text_w)/2:y=(h-text_h)/2:enable='between(t,{songLine.StartTime},{songLine.EndTime})',";
        }
        textInfo = textInfo.TrimEnd(',');

        var command = ffmpegPath + " -loop 1 -i \"" + imagePath + "\" -i \"" + musicPath + "\" -shortest -vf \"" + textInfo + "\" -codec:a copy \"" + outputPath + "\" -y";
        var process = Process.Start("cmd.exe", @"/c " + command);
        process.WaitForExit();
    }
}
