using KaraokeMakerWPF.Controls.Models;
using System.ComponentModel;

namespace KaraokeMakerWPF;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private SelectBackgroundViewModel _selectBackgroundVM;
    public SelectBackgroundViewModel SelectBackgroundVM
    {
        get => _selectBackgroundVM;
        set
        {
            _selectBackgroundVM = value;
            OnPropertyChanged(nameof(SelectBackgroundVM));
        }
    }

    private SelectFontViewModel _selectFontVM;
    public SelectFontViewModel SelectFontVM
    {
        get => _selectFontVM;
        set
        {
            _selectFontVM = value;
            OnPropertyChanged(nameof(SelectFontVM));
        }
    }

    private SelectMusicViewModel _selectMusicVM;
    public SelectMusicViewModel SelectMusicVM
    {
        get => _selectMusicVM;
        set
        {
            _selectMusicVM = value;
            OnPropertyChanged(nameof(SelectMusicVM));
        }
    }

    private SelectSongTextViewModel _selectSongTextVM;
    public SelectSongTextViewModel SelectSongTextVM
    {
        get => _selectSongTextVM;
        set
        {
            _selectSongTextVM = value;
            OnPropertyChanged(nameof(SelectSongTextVM));
        }
    }

    private CreateSongMarkupViewModel _createSongMarkupVM;
    public CreateSongMarkupViewModel CreateSongMarkupVM
    {
        get => _createSongMarkupVM;
        set
        {
            _createSongMarkupVM = value;
            OnPropertyChanged(nameof(CreateSongMarkupVM));
        }
    }

    public MainWindowViewModel()
    {
        SelectBackgroundVM = new();
        SelectFontVM = new();
        SelectMusicVM = new();
        SelectSongTextVM = new();
        CreateSongMarkupVM = new();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
