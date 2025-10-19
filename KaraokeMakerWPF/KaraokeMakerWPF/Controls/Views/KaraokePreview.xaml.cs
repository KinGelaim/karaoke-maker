using KaraokeMakerWPF.Models;
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

    public static readonly DependencyProperty KaraokeInfoProperty =
        DependencyProperty.Register("KaraokeInfo", typeof(KaraokeInfo), typeof(KaraokePreview), new PropertyMetadata(null));

    public KaraokeInfo KaraokeInfo
    {
        get { return (KaraokeInfo)GetValue(KaraokeInfoProperty); }
        set { SetValue(KaraokeInfoProperty, value); }
    }

    public KaraokePreview()
    {
        InitializeComponent();
    }

    private void StartBtn_Click(object sender, RoutedEventArgs e)
    {
        _mediaPlayer.Open(new Uri(KaraokeInfo.MusicFilePath));

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
            var currentLine = KaraokeInfo.SongLines
                .FirstOrDefault(x => x.StartTime <= currentSecond && currentSecond <= x.EndTime)?
                .Text;

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
