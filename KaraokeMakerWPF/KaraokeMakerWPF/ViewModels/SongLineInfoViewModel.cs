using System.ComponentModel;

namespace KaraokeMakerWPF.ViewModels;

public class SongLineInfoViewModel : INotifyPropertyChanged
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

    private long _startTime;
    public long StartTime
    {
        get => _startTime;
        private set
        {
            _startTime = value;
            OnPropertyChanged(nameof(StartTime));
        }
    }

    private long _endTime;
    public long EndTime
    {
        get => _endTime;
        private set
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

    public void SetTime(long startTime, long endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
    }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion INotifyPropertyChanged
}