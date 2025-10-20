using KaraokeMakerWPF.Components.StepByStepControl.ViewModels;
using KaraokeMakerWPF.ViewModels;
using System.Windows;

namespace KaraokeMakerWPF;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        InitializeSteps();

        DataContext = new MainWindowViewModel();
    }

    private void InitializeSteps()
    {
        StepByStepControl.SetSteps([
            "Фон",
            "Шрифт",
            "Музыка",
            "Текст песни",
            "Разметка\r\nкараоке",
            "Предпросмотр\r\nрезультата",
            "Создание\r\nкараоке"
        ]);
        StepByStepControl.SetIndex(0);

        UpdateCurrentStepsVisibility();
    }

    private void PreviousStepBtn_Click(object sender, RoutedEventArgs e)
    {
        StepByStepControl.Previous();
        UpdateCurrentStepsVisibility();
    }

    private void NextStepBtn_Click(object sender, RoutedEventArgs e)
    {
        StepByStepControl.Next();
        UpdateCurrentStepsVisibility();
    }

    private void UpdateCurrentStepsVisibility()
    {
        SetDefaultStepsVisibility();
        switch (StepByStepControl.CurrentIndex)
        {
            case 0:
                SelectBackgroundControl.Visibility = Visibility.Visible;
                break;
            case 1:
                SelectFontControl.Visibility = Visibility.Visible;
                break;
            case 2:
                SelectMusicControl.Visibility = Visibility.Visible;
                break;
            case 3:
                SelectSongTextControl.Visibility = Visibility.Visible;
                break;
            case 4:
                CreateSongMarkupControl.Visibility = Visibility.Visible;
                break;
            case 5:
                KaraokePreviewControl.Visibility = Visibility.Visible;
                break;
            case 6:
                CreateKaraokeControl.Visibility = Visibility.Visible;
                break;
        }
    }

    private void SetDefaultStepsVisibility()
    {
        SelectBackgroundControl.Visibility = Visibility.Collapsed;
        SelectFontControl.Visibility = Visibility.Collapsed;
        SelectMusicControl.Visibility = Visibility.Collapsed;
        SelectSongTextControl.Visibility = Visibility.Collapsed;
        CreateSongMarkupControl.Visibility = Visibility.Collapsed;
        KaraokePreviewControl.Visibility = Visibility.Collapsed;
        CreateKaraokeControl.Visibility = Visibility.Collapsed;
    }
}