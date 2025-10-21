using KaraokeMakerWPF.Environment;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace KaraokeMakerWPF.ViewModels;

public class KaraokePreviewViewModel : StepByStepViewModelBase
{
    private readonly MediaPlayer _mediaPlayer = new();

    public KaraokeInfoViewModel KaraokeInfoVM { get; init; }

    private string? _songLabelText = null;
    public string? SongLabelText
    {
        get => _songLabelText;
        set
        {
            _songLabelText = value;
            OnPropertyChanged(nameof(SongLabelText));
        }
    }

    public ICommand StartCommand { get; }
    public ICommand StopCommand { get; }

    public KaraokePreviewViewModel(KaraokeInfoViewModel karaokeInfoVM)
    {
        KaraokeInfoVM = karaokeInfoVM;

        StartCommand = new DelegateCommand(Start);
        StopCommand = new DelegateCommand(Stop);
    }

    private void Start()
    {
        _mediaPlayer.Open(new Uri(KaraokeInfoVM.MusicFilePath));

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
            var currentLine = KaraokeInfoVM.SongLines
                .FirstOrDefault(x => x.StartTime <= currentSecond && currentSecond <= x.EndTime)?
                .Text;

            SongLabelText = currentLine;
        }
        else
        {
            SongLabelText = "";
        }
    }

    private void Stop()
    {
        _mediaPlayer.Stop();
    }
}