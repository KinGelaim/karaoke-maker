using System.ComponentModel;

namespace KaraokeMakerWPF.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private KaraokeInfoViewModel _karaokeInfo;
    public KaraokeInfoViewModel KaraokeInfoVM
    {
        get => _karaokeInfo;
        set
        {
            _karaokeInfo = value;
            OnPropertyChanged(nameof(KaraokeInfoVM));
        }
    }

    public MainWindowViewModel()
    {
        _karaokeInfo = new();
    }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion INotifyPropertyChanged
}