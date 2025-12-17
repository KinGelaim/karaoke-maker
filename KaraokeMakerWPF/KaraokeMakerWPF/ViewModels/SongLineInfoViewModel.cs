using KaraokeMakerWPF.Environment;

namespace KaraokeMakerWPF.ViewModels;

public sealed class SongLineInfoViewModel : NotificationObject
{
    public int _index;
    public int Index
    {
        get => _index;
        init
        {
            _index = value;
            OnPropertyChanged(nameof(Index));
        }
    }

    private double _startTime;
    public double StartTime
    {
        get => _startTime;
        set
        {
            _startTime = value;
            OnPropertyChanged(nameof(StartTime));
        }
    }

    private double _endTime;
    public double EndTime
    {
        get => _endTime;
        set
        {
            _endTime = value;
            OnPropertyChanged(nameof(EndTime));
        }
    }

    public string _text;
    public string Text
    {
        get => _text;
        init
        {
            _text = value;
            OnPropertyChanged(nameof(Text));
        }
    }

    public SongLineInfoViewModel(int index, string text)
    {
        Index = index;
        Text = text;
    }

    public void SetTime(double startTime, double endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
    }
}