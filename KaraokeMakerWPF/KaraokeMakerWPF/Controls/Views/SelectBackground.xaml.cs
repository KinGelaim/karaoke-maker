using KaraokeMakerWPF.Controls.Models;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace KaraokeMakerWPF.Controls.Views;

/// <summary>
/// Логика взаимодействия для SelectBackground.xaml
/// </summary>
public partial class SelectBackground : UserControl
{
    public SelectBackground()
    {
        InitializeComponent();
    }

    private void SelectImageBtn_Click(object sender, RoutedEventArgs e)
    {
        var imageDialog = new OpenFileDialog
        {
            Filter = "Файлы рисунков (*.png, *.jpg)|*.png;*.jpg"
        };

        if (imageDialog.ShowDialog() == true)
        {
            if (DataContext is SelectBackgroundViewModel viewModel)
            {
                viewModel.ImageFilePath = imageDialog.FileName;
            }
        }
    }
}
