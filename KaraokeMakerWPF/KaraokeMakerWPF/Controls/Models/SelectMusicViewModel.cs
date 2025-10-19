using System.ComponentModel;

namespace KaraokeMakerWPF.Controls.Models;

public class SelectMusicViewModel : INotifyPropertyChanged
{
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

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}