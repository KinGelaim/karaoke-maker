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
    public static readonly DependencyProperty ImageFilePathProperty =
        DependencyProperty.Register("ImageFilePath", typeof(string), typeof(CreateKaraoke), new PropertyMetadata(null));

    public string ImageFilePath
    {
        get { return (string)GetValue(ImageFilePathProperty); }
        set { SetValue(ImageFilePathProperty, value); }
    }

    public static readonly DependencyProperty MusicFilePathProperty =
        DependencyProperty.Register("MusicFilePath", typeof(string), typeof(CreateKaraoke), new PropertyMetadata(null));

    public string MusicFilePath
    {
        get { return (string)GetValue(MusicFilePathProperty); }
        set { SetValue(MusicFilePathProperty, value); }
    }

    public static readonly DependencyProperty FontFilePathProperty =
        DependencyProperty.Register("FontFilePath", typeof(string), typeof(CreateKaraoke), new PropertyMetadata(null));

    public string FontFilePath
    {
        get { return (string)GetValue(FontFilePathProperty); }
        set { SetValue(FontFilePathProperty, value); }
    }

    public static readonly DependencyProperty SongLinesProperty =
        DependencyProperty.Register("SongLines", typeof(string[]), typeof(CreateKaraoke), new PropertyMetadata(null));

    public string[] SongLines
    {
        get { return (string[])GetValue(SongLinesProperty); }
        set { SetValue(SongLinesProperty, value); }
    }

    public static readonly DependencyProperty AllInfoProperty =
        DependencyProperty.Register("AllInfo", typeof((int, long, long)[]), typeof(CreateKaraoke), new PropertyMetadata(null));

    public (int, long, long)[] AllInfo
    {
        get { return ((int, long, long)[])GetValue(AllInfoProperty); }
        set { SetValue(AllInfoProperty, value); }
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
        var imagePath = ImageFilePath;
        var musicPath = MusicFilePath;

        // Шрифт должен лежать возле либы (TODO: поправить на абсолютный путь?)
        var fontFileName = Path.GetFileName(FontFilePath);

        var fileName = $"Karaoke_{Guid.NewGuid()}.mp4";
        var outputPath = $"{OutputFolderLabel.Content}\\{fileName}";

        var textInfo = string.Empty;
        for (var i = 0; i < SongLines.Length; i++)
        {
            var start = AllInfo.First(x => x.Item1 == i).Item2;
            var end = AllInfo.First(x => x.Item1 == i).Item3;
            textInfo += $"drawtext=fontfile={fontFileName}:text='{SongLines[i]}':fontcolor=white:fontsize=24:box=1:boxcolor=black@0.5:boxborderw=5:x=(w-text_w)/2:y=(h-text_h)/2:enable='between(t,{start},{end})',";
        }
        textInfo = textInfo.TrimEnd(',');

        var command = ffmpegPath + " -loop 1 -i \"" + imagePath + "\" -i \"" + musicPath + "\" -shortest -vf \"" + textInfo + "\" -codec:a copy \"" + outputPath + "\" -y";
        var process = Process.Start("cmd.exe", @"/c " + command);
        process.WaitForExit();
    }
}
