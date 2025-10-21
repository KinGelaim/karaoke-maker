using KaraokeMakerWPF.Environment;
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
}