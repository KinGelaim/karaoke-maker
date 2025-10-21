using KaraokeMakerWPF.Components.StepByStepControl.ViewModels;
using KaraokeMakerWPF.Environment;
using System.Windows.Input;

namespace KaraokeMakerWPF.ViewModels;

public class MainWindowViewModel : NotificationObject
{
    private StepByStepViewModelBase[] Steps = [];

    private StepByStepViewModelBase _stepByStepViewModel;
    public StepByStepViewModelBase StepByStepViewModel
    {
        get => _stepByStepViewModel;
        set
        {
            _stepByStepViewModel = value;
            OnPropertyChanged(nameof(StepByStepViewModel));
        }
    }

    public ICommand PreviousStepCommand { get; set; }
    public ICommand NextStepCommand { get; set; }

    private int _currentStep = 0;
    private static readonly string[] _steps = [
        "Фон",
        "Шрифт",
        "Музыка",
        "Текст песни",
        "Разметка\r\nкараоке",
        "Предпросмотр\r\nрезультата",
        "Создание\r\nкараоке"
    ];

    public StepByStepViewModel _stepByStepVM;
    public StepByStepViewModel StepByStepVM
    {
        get => _stepByStepVM;
        set
        {
            _stepByStepVM = value;
            OnPropertyChanged(nameof(StepByStepVM));
        }
    }

    public MainWindowViewModel()
    {
        var karaokeInfo = new KaraokeInfoViewModel();

        InitializeSteps(karaokeInfo);

        PreviousStepCommand = new DelegateCommand(PreviousStep);
        NextStepCommand = new DelegateCommand(NextStep);
    }

    private void InitializeSteps(KaraokeInfoViewModel karaokeInfo)
    {
        StepByStepVM = new StepByStepViewModel();
        StepByStepVM.SetSteps(_steps);

        Steps = [
            new SelectBackgroundViewModel(karaokeInfo),
            new SelectFontViewModel(karaokeInfo),
            new SelectMusicViewModel(karaokeInfo),
            new SelectSongTextViewModel(karaokeInfo),
            new CreateSongMarkupViewModel(karaokeInfo),
            new KaraokePreviewViewModel(karaokeInfo),
            new CreateKaraokeViewModel(karaokeInfo)
        ];

        _currentStep = 0;

        UpdateStepByStepViewModel();
    }

    private void PreviousStep()
    {
        // TODO: Обработать границы
        _currentStep--;
        UpdateStepByStepViewModel();
    }

    private void NextStep()
    {
        // TODO: Обработать границы
        _currentStep++;
        UpdateStepByStepViewModel();
    }

    private void UpdateStepByStepViewModel()
    {
        StepByStepVM.SetIndex(_currentStep);

        // TODO: обработать окончание
        if (_currentStep >= 0 && _currentStep < Steps.Length)
        {
            StepByStepViewModel = Steps[_currentStep];
        }
    }
}