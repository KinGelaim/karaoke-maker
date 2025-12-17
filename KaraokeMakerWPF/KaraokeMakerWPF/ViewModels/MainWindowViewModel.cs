using KaraokeMakerWPF.Components.StepByStepControl.ViewModels;
using KaraokeMakerWPF.Environment;
using System.Windows;
using System.Windows.Input;

namespace KaraokeMakerWPF.ViewModels;

public sealed class MainWindowViewModel : NotificationObject
{
    private bool _isStart = false;
    public bool IsStart
    {
        get => _isStart;
        set
        {
            _isStart = value;
            OnPropertyChanged(nameof(IsStart));
            OnPropertyChanged(nameof(IsNotStart));
        }
    }

    public bool IsNotStart => !IsStart;

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

    public ICommand StartCommand { get; set; }
    public ICommand PreviousStepCommand { get; set; }
    public ICommand NextStepCommand { get; set; }

    private int _currentStep = -1;
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

    private bool _canGoPrevious;
    public bool CanGoPrevious
    {
        get => _canGoPrevious;
        set
        {
            _canGoPrevious = value;
            OnPropertyChanged(nameof(CanGoPrevious));
        }
    }

    private bool _canGoNext;
    public bool CanGoNext
    {
        get => _canGoNext;
        set
        {
            _canGoNext = value;
            OnPropertyChanged(nameof(CanGoNext));
        }
    }

    public MainWindowViewModel()
    {
        var karaokeInfo = new KaraokeInfoViewModel();

        InitializeSteps(karaokeInfo);

        StartCommand = new DelegateCommand(Start);
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

        UpdateStepByStepViewModel();
    }

    private void Start()
    {
        IsStart = true;
        _currentStep = 0;
        UpdateStepByStepViewModel();
    }

    private void PreviousStep()
    {
        _currentStep--;
        UpdateStepByStepViewModel();
    }

    private void NextStep()
    {
        var validationResult = StepByStepViewModel.ValidateBeforeNextStep();
        if (!validationResult.IsValid)
        {
            MessageBox.Show(validationResult.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        _currentStep++;
        UpdateStepByStepViewModel();
    }

    private void UpdateStepByStepViewModel()
    {
        CanGoPrevious = _currentStep > 0;
        CanGoNext = _currentStep < Steps.Length - 1;

        StepByStepVM.SetIndex(_currentStep);

        // TODO: обработать окончание
        if (_currentStep >= 0 && _currentStep < Steps.Length)
        {
            StepByStepViewModel = Steps[_currentStep];
        }
    }
}