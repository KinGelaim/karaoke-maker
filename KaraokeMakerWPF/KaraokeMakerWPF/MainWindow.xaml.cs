using KaraokeMakerWPF.Components.StepByStepControl.Models;
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
        // TODO: завернуть во внутреннию часть компонента, чтобы не руками проставлять порядковые номера
        StepByStepControl.Steps.Add(new StepInfo("Фон", 1));
        StepByStepControl.Steps.Add(new StepInfo("Шрифт", 2));
        StepByStepControl.Steps.Add(new StepInfo("Музыка", 3));
        StepByStepControl.Steps.Add(new StepInfo("Текст песни", 4));
        StepByStepControl.Steps.Add(new StepInfo("Разметка\r\nкараоке", 5));
        StepByStepControl.Steps.Add(new StepInfo("Предпросмотр\r\nрезультата", 6));
        StepByStepControl.Steps.Add(new StepInfo("Создание\r\nкараоке", 7));
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