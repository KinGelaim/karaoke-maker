using System.ComponentModel;

namespace KaraokeMakerWPF.Models;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private KaraokeInfo? _karaokeInfo;
    public KaraokeInfo KaraokeInfo
    {
        get => _karaokeInfo ??= new KaraokeInfo();
        set
        {
            _karaokeInfo = value;
            OnPropertyChanged(nameof(KaraokeInfo));
        }
    }

    public MainWindowViewModel()
    {
        KaraokeInfo = new();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}