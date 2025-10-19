using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace KaraokeMakerWPF.Controls.Views;

/// <summary>
/// Логика взаимодействия для KaraokePreview.xaml
/// </summary>
public partial class KaraokePreview : UserControl
{
    private readonly MediaPlayer _mediaPlayer = new();

    public static readonly DependencyProperty ImageFilePathProperty =
        DependencyProperty.Register("ImageFilePath", typeof(string), typeof(KaraokePreview), new PropertyMetadata(null));

    public string ImageFilePath
    {
        get { return (string)GetValue(ImageFilePathProperty); }
        set { SetValue(ImageFilePathProperty, value); }
    }

    public static readonly DependencyProperty MusicFilePathProperty =
        DependencyProperty.Register("MusicFilePath", typeof(string), typeof(KaraokePreview), new PropertyMetadata(null));

    public string MusicFilePath
    {
        get { return (string)GetValue(MusicFilePathProperty); }
        set { SetValue(MusicFilePathProperty, value); }
    }

    public static readonly DependencyProperty FontFilePathProperty =
        DependencyProperty.Register("FontFilePath", typeof(string), typeof(KaraokePreview), new PropertyMetadata(null));

    public string FontFilePath
    {
        get { return (string)GetValue(FontFilePathProperty); }
        set { SetValue(FontFilePathProperty, value); }
    }

    public static readonly DependencyProperty SongLinesProperty =
        DependencyProperty.Register("SongLines", typeof(string[]), typeof(KaraokePreview), new PropertyMetadata(null));

    public string[] SongLines
    {
        get { return (string[])GetValue(SongLinesProperty); }
        set { SetValue(SongLinesProperty, value); }
    }

    public static readonly DependencyProperty AllInfoProperty =
        DependencyProperty.Register("AllInfo", typeof((int, long, long)[]), typeof(KaraokePreview), new PropertyMetadata(null));

    public (int, long, long)[] AllInfo
    {
        get { return ((int, long, long)[])GetValue(AllInfoProperty); }
        set { SetValue(AllInfoProperty, value); }
    }

    public KaraokePreview()
    {
        InitializeComponent();
    }

    private void StartBtn_Click(object sender, RoutedEventArgs e)
    {
        _mediaPlayer.Open(new Uri(MusicFilePath));

        var timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };

        timer.Tick += Timer_Tick;
        timer.Start();

        _mediaPlayer.Play();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (_mediaPlayer.Source != null)
        {
            var currentSecond = _mediaPlayer.Position.Seconds;
            var currentLineIndex = AllInfo.FirstOrDefault(x => x.Item2 <= currentSecond && currentSecond <= x.Item3).Item1;
            var currentLine = SongLines[currentLineIndex];

            SongLabel.Text = currentLine;
        }
        else
        {
            SongLabel.Text = "";
        }
    }

    private void StopBtn_Click(object sender, RoutedEventArgs e)
    {
        _mediaPlayer.Stop();
    }
}
