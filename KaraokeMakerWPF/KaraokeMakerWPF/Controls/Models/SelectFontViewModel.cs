using System.ComponentModel;

namespace KaraokeMakerWPF.Controls.Models;

public class SelectFontViewModel : INotifyPropertyChanged
{
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

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
