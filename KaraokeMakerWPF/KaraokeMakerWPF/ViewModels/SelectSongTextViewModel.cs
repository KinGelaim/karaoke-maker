using KaraokeMakerWPF.Environment;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace KaraokeMakerWPF.ViewModels;

public class SelectSongTextViewModel : StepByStepViewModelBase
{
    public KaraokeInfoViewModel KaraokeInfoVM { get; init; }

    private string _songTextFilePath = string.Empty;
    public string SongTextFilePath
    {
        get => _songTextFilePath;
        set
        {
            _songTextFilePath = value;
            OnPropertyChanged(nameof(SongTextFilePath));
        }
    }

    public ICommand SelectTextFilePathCommand { get; }
    public ICommand ParseCommand { get; }

    public SelectSongTextViewModel(KaraokeInfoViewModel karaokeInfoVM)
    {
        KaraokeInfoVM = karaokeInfoVM;

        SelectTextFilePathCommand = new DelegateCommand(SelectTextPath);
        ParseCommand = new DelegateCommand(ParseText);
    }

    private void SelectTextPath()
    {
        var textFileDialog = new OpenFileDialog
        {
            Filter = "Файл текста (*.txt)|*.txt"
        };

        if (textFileDialog.ShowDialog() == true)
        {
            SongTextFilePath = textFileDialog.FileName;
        }
    }

    private void ParseText()
    {
        if (string.IsNullOrWhiteSpace(SongTextFilePath))
        {
            MessageBox.Show("Некорректный путь к файлу с содержанием песни");
            return;
        }

        var lines = File.ReadAllLines(SongTextFilePath);
        KaraokeInfoVM.SetSongLines(lines);
    }
}