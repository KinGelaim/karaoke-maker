using KaraokeMakerWPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace KaraokeMakerWPF.Controls.Views;

/// <summary>
/// Логика взаимодействия для CreateSongMarkup.xaml
/// </summary>
public partial class CreateSongMarkup : UserControl
{
    private readonly MediaPlayer _mediaPlayer = new();

    private int _currentLineIndex = 0;
    private int _maxLineIndex = 0;

    private bool _isPlayMusic = false;
    private bool _isStartText = false;
    private bool _isSongEnd = false;

    private long _startTime = 0;
    private long _endTime = 0;

    public static readonly DependencyProperty KaraokeInfoProperty =
        DependencyProperty.Register(
            nameof(KaraokeInfoVM),
            typeof(KaraokeInfoViewModel),
            typeof(CreateSongMarkup),
            new PropertyMetadata(null));

    public KaraokeInfoViewModel KaraokeInfoVM
    {
        get { return (KaraokeInfoViewModel)GetValue(KaraokeInfoProperty); }
        set { SetValue(KaraokeInfoProperty, value); }
    }

    public CreateSongMarkup()
    {
        InitializeComponent();
    }

    private void PlayBtn_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(KaraokeInfoVM.MusicFilePath))
        {
            return;
        }

        _currentLineIndex = 0;
        _maxLineIndex = KaraokeInfoVM.SongLines.Count;
        UpdateCurrentLineLabel();


        _mediaPlayer.Open(new Uri(KaraokeInfoVM.MusicFilePath));

        var timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };

        timer.Tick += Timer_Tick;
        timer.Start();

        _mediaPlayer.Play();
        _isPlayMusic = true;
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (_mediaPlayer.Source != null)
        {
            StatusLabel.Content = string.Format(
                "{0} / {1}",
                _mediaPlayer.Position.ToString(@"mm\:ss"),
                _mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
        }
        else
        {
            StatusLabel.Content = "Файл не выбран...";
        }
    }

    private void NextLineBtn_Click(object sender, RoutedEventArgs e)
    {
        if (!_isPlayMusic || _isSongEnd)
        {
            return;
        }

        if (!_isStartText)
        {
            _startTime = _mediaPlayer.Position.Seconds;
            _isStartText = true;
        }
        else
        {
            _endTime = _mediaPlayer.Position.Seconds;

            if (_currentLineIndex < _maxLineIndex)
            {
                var currentSongLine = KaraokeInfoVM.SongLines.First(x => x.Index == _currentLineIndex);
                currentSongLine.SetTime(_startTime, _endTime);

                _currentLineIndex++;
                _startTime = _endTime;
                UpdateCurrentLineLabel();
            }

            if (_currentLineIndex >= _maxLineIndex)
            {
                _isSongEnd = true;
                _mediaPlayer.Stop();
            }
        }
    }

    private void UpdateCurrentLineLabel()
    {
        if (_currentLineIndex >= _maxLineIndex)
        {
            return;
        }

        CurrentLineLabel.Content = KaraokeInfoVM.SongLines[_currentLineIndex].Text;
    }
}
