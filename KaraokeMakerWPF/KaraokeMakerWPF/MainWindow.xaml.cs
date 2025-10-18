using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace KaraokeMakerWPF;

public partial class MainWindow : Window
{
    private string[] lines = [];
    private int currentLineIndex = 0;
    private int maxLineIndex = 0;

    private bool isPlayVideo = false;
    private bool isStartText = false;
    private bool isSongEnd = false;

    private long startTime = 0;
    private long endTime = 0;

    private readonly Stopwatch stopwatch = new();

    private readonly List<(int, long, long)> allInfo = [];

    public MainWindow()
    {
        InitializeComponent();
    }

    private void SelectImageBtn_Click(object sender, RoutedEventArgs e)
    {
        var imageDialog = new OpenFileDialog
        {
            Filter = "Файлы рисунков (*.png, *.jpg)|*.png;*.jpg"
        };

        if (imageDialog.ShowDialog() == true)
        {
            ImageLabel.Content = imageDialog.FileName;
        }
    }

    private void SelectMusicBtn_Click(object sender, RoutedEventArgs e)
    {
        var musicDialog = new OpenFileDialog
        {
            Filter = "Файл музыки (*.mp3)|*.mp3"
        };

        if (musicDialog.ShowDialog() == true)
        {
            MusicLabel.Content = musicDialog.FileName;
        }
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

    private void StartCreatingKaraokeBtn_Click(object sender, RoutedEventArgs e)
    {
        var ffmpegPath = FfmpegLabel.Content;
        var imagePath = ImageLabel.Content;
        var musicPath = MusicLabel.Content;

        var fileName = $"{Guid.NewGuid()}.avi";
        var outputPath = $"{OutputFolderLabel.Content}\\{fileName}";

        var command = ffmpegPath + " -loop 1 -i \"" + imagePath + "\" -i \"" + musicPath + "\" -shortest -s 3840x2160 \"" + outputPath + "\" -y";
        ResultLabel.Content = command;

        var process = Process.Start("cmd.exe", @"/c " + command);
        process.WaitForExit();
    }

    private void SelectTextFileBtn_Click(object sender, RoutedEventArgs e)
    {
        var textFileDialog = new OpenFileDialog
        {
            Filter = "Файл текста (*.txt)|*.txt"
        };

        if (textFileDialog.ShowDialog() == true)
        {
            TextFileLabel.Content = textFileDialog.FileName;
        }
    }

    private void ParseTextFileBtn_Click(object sender, RoutedEventArgs e)
    {
        var textFilePath = TextFileLabel.Content.ToString();
        if (string.IsNullOrWhiteSpace(textFilePath))
        {
            MessageBox.Show("Некорректный путь к файлу песни");
            return;
        }

        lines = File.ReadAllLines(textFilePath);
        currentLineIndex = 0;
        maxLineIndex = lines.Length;

        UpdateCurrentLineLabel();
    }

    private void UpdateCurrentLineLabel()
    {
        if (currentLineIndex >= maxLineIndex)
        {
            return;
        }

        CurrentLineLabel.Content = lines[currentLineIndex];
    }

    private void OpenVideoFileBtn_Click(object sender, RoutedEventArgs e)
    {
        var videoFileDialog = new OpenFileDialog
        {
            Filter = "Видео файлы (*.mp4;*.avi;*.wmv;*.mov)|*.mp4;*.avi;*.wmv;*.mov"
        };

        if (videoFileDialog.ShowDialog() == true)
        {
            VideoMediaElement.Source = new Uri(videoFileDialog.FileName);
            VideoMediaElement.Play();

            isPlayVideo = true;
            stopwatch.Restart();
        }
    }

    private void NextLineBtn_Click(object sender, RoutedEventArgs e)
    {
        if (!isPlayVideo || isSongEnd)
        {
            return;
        }

        if (!isStartText)
        {
            startTime = stopwatch.Elapsed.Seconds;
            isStartText = true;
        }
        else
        {
            endTime = stopwatch.Elapsed.Seconds;

            allInfo.Add((currentLineIndex, startTime, endTime));

            if (currentLineIndex < maxLineIndex)
            {
                currentLineIndex++;
                startTime = endTime;
                UpdateCurrentLineLabel();
            }
            else
            {
                isSongEnd = true;

                UpdateAllInfoList();
            }
        }
    }

    private void UpdateAllInfoList()
    {
        AllInfoList.Items.Clear();

        var items = allInfo.Select(x => $"{x.Item1} - {x.Item2} - {x.Item3}");
        AllInfoList.ItemsSource = items;
    }

    private void SelectVideoFileBtn_Click(object sender, RoutedEventArgs e)
    {
        var videoFileDialog = new OpenFileDialog
        {
            Filter = "Видео файлы (*.mp4;*.avi;*.wmv;*.mov)|*.mp4;*.avi;*.wmv;*.mov"
        };

        if (videoFileDialog.ShowDialog() == true)
        {
            VideoFileLabel.Content = videoFileDialog.FileName;
        }
    }

    private void SelectFontFileBtn_Click(object sender, RoutedEventArgs e)
    {
        var fontFileDialog = new OpenFileDialog
        {
            Filter = "Файл шрифта (*.ttf)|*.ttf"
        };

        if (fontFileDialog.ShowDialog() == true)
        {
            FontFileLabel.Content = fontFileDialog.FileName;
        }
    }

    private void CreateKaraokeVideoBtn_Click(object sender, RoutedEventArgs e)
    {
        var ffmpegPath = FfmpegLabel.Content;
        var inputVideoPath = VideoFileLabel.Content;

        // Шрифт должен лежать возле либы (TODO: поправить на абсолютный путь?)
        var fontFileName = Path.GetFileName(FontFileLabel.Content.ToString());

        var fileName = $"Karaoke_{Guid.NewGuid()}.avi";
        var outputPath = $"{OutputFolderLabel.Content}\\{fileName}";

        var textInfo = string.Empty;
        for (var i = 0; i < lines.Length; i++)
        {
            var start = allInfo.First(x => x.Item1 == i).Item2;
            var end = allInfo.First(x => x.Item1 == i).Item3;
            textInfo += $"drawtext=fontfile={fontFileName}:text='{lines[i]}':fontcolor=white:fontsize=48:box=1:boxcolor=black@0.5:boxborderw=5:x=(w-text_w)/2:y=(h-text_h)/2:enable='between(t,{start},{end})',";
        }
        textInfo = textInfo.TrimEnd(',');

        var command = ffmpegPath + " -i \"" + inputVideoPath + "\" -vf \"" + textInfo + "\" -codec:a copy \"" + outputPath + "\" -y";
        var process = Process.Start("cmd.exe", @"/c " + command);
        process.WaitForExit();
    }
}