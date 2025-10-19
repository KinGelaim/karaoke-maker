using KaraokeMakerWPF.Controls.Models;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace KaraokeMakerWPF.Controls.Views;

/// <summary>
/// Логика взаимодействия для SelectFont.xaml
/// </summary>
public partial class SelectFont : UserControl
{
    public SelectFont()
    {
        InitializeComponent();
    }

    private void SelectFontFileBtn_Click(object sender, RoutedEventArgs e)
    {
        var fontFileDialog = new OpenFileDialog
        {
            Filter = "Файлы шрифтов (*.ttf)|*.ttf"
        };

        if (fontFileDialog.ShowDialog() == true)
        {
            if (DataContext is SelectFontViewModel viewModel)
            {
                viewModel.FontFilePath = fontFileDialog.FileName;
            }
        }
    }
}
