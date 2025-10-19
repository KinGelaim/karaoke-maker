using System.ComponentModel;

namespace KaraokeMakerWPF.Controls.Models;

public class SelectBackgroundViewModel : INotifyPropertyChanged
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

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
