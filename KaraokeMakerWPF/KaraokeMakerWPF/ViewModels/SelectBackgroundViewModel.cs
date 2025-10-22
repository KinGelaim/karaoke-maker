using KaraokeMakerWPF.Environment;
using KaraokeMakerWPF.Models;
using Microsoft.Win32;
using System.Windows.Input;

namespace KaraokeMakerWPF.ViewModels;

public class SelectBackgroundViewModel : StepByStepViewModelBase
{
    public KaraokeInfoViewModel KaraokeInfoVM { get; init; }

    public ICommand SelectImagePathCommand { get; }

    public SelectBackgroundViewModel(KaraokeInfoViewModel karaokeInfoVM)
    {
        KaraokeInfoVM = karaokeInfoVM;

        SelectImagePathCommand = new DelegateCommand(SelectImagePath);
    }

    private void SelectImagePath()
    {
        var imageDialog = new OpenFileDialog
        {
            Filter = "Файлы рисунков (*.png, *.jpg)|*.png;*.jpg"
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