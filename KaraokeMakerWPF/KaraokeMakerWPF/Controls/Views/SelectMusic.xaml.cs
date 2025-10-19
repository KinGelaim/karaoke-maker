using KaraokeMakerWPF.Controls.Models;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace KaraokeMakerWPF.Controls.Views;

/// <summary>
/// Логика взаимодействия для SelectMusic.xaml
/// </summary>
public partial class SelectMusic : UserControl
{
    public SelectMusic()
    {
        InitializeComponent();
    }

    private void SelectMusicBtn_Click(object sender, RoutedEventArgs e)
    {
        var musicDialog = new OpenFileDialog
        {
            Filter = "Файлы музыки (*.mp3)|*.mp3"
        };

        if (musicDialog.ShowDialog() == true)
        {
            if (DataContext is SelectMusicViewModel viewModel)
            {
                viewModel.MusicFilePath = musicDialog.FileName;
            }
        }
    }
}
