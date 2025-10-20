using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace KaraokeMakerWPF.Controls.Views;

/// <summary>
/// Логика взаимодействия для SelectMusic.xaml
/// </summary>
public partial class SelectMusic : UserControl
{
    public static readonly DependencyProperty MusicFilePathProperty =
        DependencyProperty.Register(
            nameof(MusicFilePath),
            typeof(string),
            typeof(SelectMusic),
            new PropertyMetadata(string.Empty));

    public string MusicFilePath
    {
        get { return (string)GetValue(MusicFilePathProperty); }
        set { SetValue(MusicFilePathProperty, value); }
    }

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
            MusicFilePath = musicDialog.FileName;
        }
    }
}
