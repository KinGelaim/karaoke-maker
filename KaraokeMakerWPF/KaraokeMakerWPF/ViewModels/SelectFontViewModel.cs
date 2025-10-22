using KaraokeMakerWPF.Environment;
using KaraokeMakerWPF.Models;
using Microsoft.Win32;
using System.Windows.Input;

namespace KaraokeMakerWPF.ViewModels;

public class SelectFontViewModel : StepByStepViewModelBase
{
    public KaraokeInfoViewModel KaraokeInfoVM { get; init; }

    public ICommand SelectFontPathCommand { get; }

    public SelectFontViewModel(KaraokeInfoViewModel karaokeInfoVM)
    {
        KaraokeInfoVM = karaokeInfoVM;

        SelectFontPathCommand = new DelegateCommand(SelectFontPath);
    }

    private void SelectFontPath()
    {
        var fontFileDialog = new OpenFileDialog
        {
            Filter = "Файлы шрифтов (*.ttf)|*.ttf"
        };

        if (fontFileDialog.ShowDialog() == true)
        {
            KaraokeInfoVM.FontFilePath = fontFileDialog.FileName;
        }
    }

    public override StepByStepValidationError ValidateBeforeNextStep()
    {
        if (string.IsNullOrWhiteSpace(KaraokeInfoVM.FontFilePath))
        {
            return StepByStepValidationError.Error("Необходимо выбрать шрифт для создания Караоке!");
        }

        return StepByStepValidationError.Success();
    }
}