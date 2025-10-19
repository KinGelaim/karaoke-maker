using System.ComponentModel;

namespace KaraokeMakerWPF.Controls.Models;

public class SelectSongTextViewModel : INotifyPropertyChanged
{
    private string _songTextFilePath = string.Empty;
    public string SongTextFilePath
    {
        get => _songTextFilePath;
        set
        {
            _songTextFilePath = value;
            OnPropertyChanged(nameof(SongTextFilePath));
        }
    }

    private string[] _lines = [];
    public string[] Lines
    {
        get => _lines;
        set
        {
            _lines = value;
            OnPropertyChanged(nameof(Lines));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}