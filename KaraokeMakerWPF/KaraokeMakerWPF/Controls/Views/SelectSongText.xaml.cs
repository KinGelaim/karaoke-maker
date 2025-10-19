using KaraokeMakerWPF.Controls.Models;
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
            if (DataContext is SelectSongTextViewModel viewModel)
            {
                viewModel.SongTextFilePath = textFileDialog.FileName;
            }
        }
    }

    private void ParseTextFileBtn_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is SelectSongTextViewModel viewModel)
        {
            var textFilePath = viewModel.SongTextFilePath;
            if (string.IsNullOrWhiteSpace(textFilePath))
            {
                MessageBox.Show("Некорректный путь к файлу песни");
                return;
            }

            viewModel.Lines = File.ReadAllLines(textFilePath);
        }
    }
}
