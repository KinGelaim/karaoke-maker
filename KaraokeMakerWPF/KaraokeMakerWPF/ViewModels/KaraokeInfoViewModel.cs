using System.Collections.ObjectModel;
using System.ComponentModel;

namespace KaraokeMakerWPF.ViewModels;

public class KaraokeInfoViewModel : INotifyPropertyChanged
{
    private string _imageFilePath = string.Empty;
    public string ImageFilePath
    {
        get => _imageFilePath;
        set
        {
            _imageFilePath = value;
            OnPropertyChanged(nameof(ImageFilePath));
        }
    }

    private string _fontFilePath = string.Empty;
    public string FontFilePath
    {
        get => _fontFilePath;
        set
        {
            _fontFilePath = value;
            OnPropertyChanged(nameof(FontFilePath));
        }
    }

    private string _musicFilePath { get; set; } = string.Empty;
    public string MusicFilePath
    {
        get => _musicFilePath;
        set
        {
            _musicFilePath = value;
            OnPropertyChanged(nameof(MusicFilePath));
        }
    }

    private ObservableCollection<SongLineInfoViewModel> _songLines = [];
    public ObservableCollection<SongLineInfoViewModel> SongLines
    {
        get => _songLines;
        set
        {
            _songLines = value;
            OnPropertyChanged(nameof(SongLines));
        }
    }

    public void SetSongLines(string[] lines)
    {
        SongLines = new ObservableCollection<SongLineInfoViewModel>(
            lines.Select((line, index) => new SongLineInfoViewModel(index, line)));
    }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion INotifyPropertyChanged
}