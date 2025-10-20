using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace KaraokeMakerWPF.Controls.Views;

/// <summary>
/// Логика взаимодействия для SelectBackground.xaml
/// </summary>
public partial class SelectBackground : UserControl
{
    public static readonly DependencyProperty ImageFilePathProperty =
        DependencyProperty.Register(
            nameof(ImageFilePath),
            typeof(string),
            typeof(SelectBackground),
            new PropertyMetadata(string.Empty));

    public string ImageFilePath
    {
        get { return (string)GetValue(ImageFilePathProperty); }
        set { SetValue(ImageFilePathProperty, value); }
    }

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
            ImageFilePath = imageDialog.FileName;
        }
    }
}
