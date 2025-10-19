using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace KaraokeMakerWPF.Controls.Views;

/// <summary>
/// Логика взаимодействия для SelectFont.xaml
/// </summary>
public partial class SelectFont : UserControl
{
    public static readonly DependencyProperty FontFilePathProperty =
        DependencyProperty.Register("FontFilePath", typeof(string), typeof(SelectFont), new PropertyMetadata(null));

    public string FontFilePath
    {
        get { return (string)GetValue(FontFilePathProperty); }
        set { SetValue(FontFilePathProperty, value); }
    }

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
            FontFilePath = fontFileDialog.FileName;
        }
    }
}
