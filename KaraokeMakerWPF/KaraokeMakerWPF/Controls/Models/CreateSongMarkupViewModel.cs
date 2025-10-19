using KaraokeMakerWPF.Models;
using System.ComponentModel;

namespace KaraokeMakerWPF.Controls.Models;

public class CreateSongMarkupViewModel
{
    private SongLineInfo[] _allInfo = [];
    public SongLineInfo[] AllInfo
    {
        get => _allInfo;
        set
        {
            _allInfo = value;
            OnPropertyChanged(nameof(AllInfo));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
