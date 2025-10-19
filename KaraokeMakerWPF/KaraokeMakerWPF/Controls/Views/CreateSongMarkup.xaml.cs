using KaraokeMakerWPF.Controls.Models;
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

    private readonly List<(int, long, long)> _allInfo = [];

    public static readonly DependencyProperty AllInfoProperty =
        DependencyProperty.Register("AllInfo", typeof((int, long, long)[]), typeof(CreateSongMarkup), new PropertyMetadata(null));

    public (int, long, long)[] AllInfo
    {
        get { return ((int, long, long)[])GetValue(AllInfoProperty); }
        set { SetValue(AllInfoProperty, value); }
    }

    public static readonly DependencyProperty MusicFilePathProperty =
        DependencyProperty.Register("MusicFilePath", typeof(string), typeof(CreateSongMarkup), new PropertyMetadata(null));

    public string MusicFilePath
    {
        get { return (string)GetValue(MusicFilePathProperty); }
        set { SetValue(MusicFilePathProperty, value); }
    }

    public static readonly DependencyProperty SongLinesProperty =
        DependencyProperty.Register("SongLines", typeof(string[]), typeof(CreateSongMarkup), new PropertyMetadata(null));

    public string[] SongLines
    {
        get { return (string[])GetValue(SongLinesProperty); }
        set { SetValue(SongLinesProperty, value); }
    }

    public CreateSongMarkup()
    {
        InitializeComponent();
    }

    private void PlayBtn_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(MusicFilePath))
        {
            return;
        }

        _currentLineIndex = 0;
        _maxLineIndex = SongLines.Length;
        UpdateCurrentLineLabel();


        _mediaPlayer.Open(new Uri(MusicFilePath));

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

            _allInfo.Add((_currentLineIndex, _startTime, _endTime));

            if (_currentLineIndex < _maxLineIndex)
            {
                _currentLineIndex++;
                _startTime = _endTime;
                UpdateCurrentLineLabel();
            }
            else
            {
                _isSongEnd = true;
                _mediaPlayer.Stop();

                UpdateAllInfoList();
            }
        }
    }

    private void UpdateCurrentLineLabel()
    {
        if (_currentLineIndex >= _maxLineIndex)
        {
            return;
        }

        CurrentLineLabel.Content = SongLines[_currentLineIndex];
    }

    private void UpdateAllInfoList()
    {
        AllInfoList.Items.Clear();

        var items = _allInfo.Select(x => $"{x.Item1} - {x.Item2} - {x.Item3}");
        AllInfoList.ItemsSource = items;

        AllInfo = [.. _allInfo];
    }
}
