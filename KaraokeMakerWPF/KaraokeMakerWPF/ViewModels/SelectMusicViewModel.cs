using KaraokeMakerWPF.Environment;
using Microsoft.Win32;
using System.Windows.Input;

namespace KaraokeMakerWPF.ViewModels;

public class SelectMusicViewModel : StepByStepViewModelBase
{
    public KaraokeInfoViewModel KaraokeInfoVM { get; init; }

    public ICommand SelectMusicPathCommand { get; }

    public SelectMusicViewModel(KaraokeInfoViewModel karaokeInfoVM)
    {
        KaraokeInfoVM = karaokeInfoVM;

        SelectMusicPathCommand = new DelegateCommand(SelectMusicPath);
    }

    private void SelectMusicPath()
    {
        var musicDialog = new OpenFileDialog
        {
            Filter = "Файлы музыки (*.mp3)|*.mp3"
        };

        if (musicDialog.ShowDialog() == true)
        {
            KaraokeInfoVM.MusicFilePath = musicDialog.FileName;
        }
    }
}
