using KaraokeMakerWPF.ViewModels;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace KaraokeMakerWPF.Controls.Views;

/// <summary>
/// Логика взаимодействия для SelectSongText.xaml
/// </summary>
public partial class SelectSongText : UserControl
{
    public static readonly DependencyProperty SongTextFilePathProperty =
        DependencyProperty.Register(
            nameof(SongTextFilePath),
            typeof(string),
            typeof(SelectSongText),
            new PropertyMetadata(string.Empty));

    public string SongTextFilePath
    {
        get { return (string)GetValue(SongTextFilePathProperty); }
        set { SetValue(SongTextFilePathProperty, value); }
    }

    public static readonly DependencyProperty KaraokeInfoProperty =
        DependencyProperty.Register(
            nameof(KaraokeInfoVM),
            typeof(KaraokeInfoViewModel),
            typeof(SelectSongText),
            new PropertyMetadata(null));

    public KaraokeInfoViewModel KaraokeInfoVM
    {
        get { return (KaraokeInfoViewModel)GetValue(KaraokeInfoProperty); }
        set { SetValue(KaraokeInfoProperty, value); }
    }

    public SelectSongText()
    {
        InitializeComponent();
    }

    private void SelectTextFileBtn_Click(object sender, RoutedEventArgs e)
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

    private void ParseTextFileBtn_Click(object sender, RoutedEventArgs e)
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
