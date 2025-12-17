using KaraokeMakerWPF.Environment;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace KaraokeMakerWPF.ViewModels;

public sealed class CreateSongMarkupViewModel : StepByStepViewModelBase
{
    private readonly MediaPlayer _mediaPlayer = new();

    private int _currentLineIndex = 0;
    private int _maxLineIndex = 0;

    private bool _isPlayMusic = false;
    private bool _isStartText = false;

    private double _startTime = 0;
    private double _endTime = 0;

    public KaraokeInfoViewModel KaraokeInfoVM { get; init; }

    public ICommand PlayCommand { get; }
    public ICommand StopCommand { get; }
    public ICommand NextLineCommand { get; }
    public ICommand PlaySongLineCommand { get; }

    private string _statusLabel = "Не проигрывается...";
    public string StatusLabelText
    {
        get => _statusLabel;
        set
        {
            _statusLabel = value;
            OnPropertyChanged(nameof(StatusLabelText));
        }
    }

    private string _currentLineLabel = string.Empty;
    public string CurrentLineLabel
    {
        get => _currentLineLabel;
        set
        {
            _currentLineLabel = value;
            OnPropertyChanged(nameof(CurrentLineLabel));
        }
    }

    public CreateSongMarkupViewModel(KaraokeInfoViewModel karaokeInfoVM)
    {
        KaraokeInfoVM = karaokeInfoVM;

        PlayCommand = new DelegateCommand(Play);
        StopCommand = new DelegateCommand(Stop);
        NextLineCommand = new DelegateCommand(NextLine);
        PlaySongLineCommand = new DelegateCommand<int>(PlaySongLine);
    }

    private void Play()
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
        _isStartText = false;
    }

    private void Stop()
    {
        _mediaPlayer.Stop();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (_mediaPlayer.Source != null)
        {
            StatusLabelText = string.Format(
                "{0} / {1}",
                _mediaPlayer.Position.ToString(@"mm\:ss"),
                _mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
        }
        else
        {
            StatusLabelText = "Файл не выбран...";
        }
    }

    private void NextLine()
    {
        if (!_isPlayMusic)
        {
            return;
        }

        if (!_isStartText)
        {
            _startTime = _mediaPlayer.Position.TotalSeconds;
            _isStartText = true;
        }
        else
        {
            _endTime = _mediaPlayer.Position.TotalSeconds;

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
                _isPlayMusic = false;
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

        CurrentLineLabel = KaraokeInfoVM.SongLines[_currentLineIndex].Text;
    }

    private void PlaySongLine(int index)
    {
        var songLine = KaraokeInfoVM.SongLines.First(x => x.Index == index);

        _mediaPlayer.Stop();

        _mediaPlayer.Position = TimeSpan.FromSeconds(songLine.StartTime);

        var timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(songLine.EndTime - songLine.StartTime)
        };

        timer.Tick += (s, ev) =>
        {
            _mediaPlayer.Stop();
            timer.Stop();
        };

        timer.Start();
        _mediaPlayer.Play();
    }
}