using KaraokeMakerWPF.Environment;
using KaraokeMakerWPF.Models;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace KaraokeMakerWPF.ViewModels;

public sealed class SelectBackgroundViewModel : StepByStepViewModelBase
{
    public KaraokeInfoViewModel KaraokeInfoVM { get; init; }

    public ICommand SelectImagePathCommand { get; }

    private ObservableCollection<ImageItemData> _images = [];
    public ObservableCollection<ImageItemData> Images
    {
        get => _images;
        set
        {
            _images = value;
            OnPropertyChanged(nameof(Images));
        }
    }

    public SelectBackgroundViewModel(KaraokeInfoViewModel karaokeInfoVM)
    {
        KaraokeInfoVM = karaokeInfoVM;

        SelectImagePathCommand = new DelegateCommand(SelectImagePath);

        LoadDefaultBackgrounds();
    }

    private void LoadDefaultBackgrounds()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources\\Backgrounds");

        var files = Directory.GetFiles(path);

        Images.Clear();
        foreach (var filePath in files)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            Images.Add(new ImageItemData(fileName, filePath));
        }

        if (Images.Count > 0)
        {
            KaraokeInfoVM.ImageFilePath = Images[0].ImagePath;
        }
    }

    private void SelectImagePath()
    {
        var imageDialog = new OpenFileDialog
        {
            Filter = "Файлы изображений (*.png, *.jpg)|*.png;*.jpg"
        };

        if (imageDialog.ShowDialog() == true)
        {
            KaraokeInfoVM.ImageFilePath = imageDialog.FileName;
        }
    }

    public override StepByStepValidationError ValidateBeforeNextStep()
    {
        if (string.IsNullOrWhiteSpace(KaraokeInfoVM.ImageFilePath))
        {
            return StepByStepValidationError.Error("Необходимо выбрать фон для создания Караоке!");
        }

        return StepByStepValidationError.Success();
    }
}

public class ImageItemData
{
    public string DisplayText { get; set; }
    public string ImagePath { get; set; }

    public ImageItemData(string displayText, string imagePath)
    {
        DisplayText = displayText;
        ImagePath = imagePath;
    }
}