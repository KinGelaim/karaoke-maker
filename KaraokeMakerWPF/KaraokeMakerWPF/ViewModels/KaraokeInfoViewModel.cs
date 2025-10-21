using KaraokeMakerWPF.Environment;
using System.Collections.ObjectModel;

namespace KaraokeMakerWPF.ViewModels;

public class KaraokeInfoViewModel : NotificationObject
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

    private string _musicFilePath = string.Empty;
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
        for (var i = 0; i < lines.Length; i++)
        {
            SongLines.Add(new SongLineInfoViewModel(i, lines[i]));
        }
    }
}