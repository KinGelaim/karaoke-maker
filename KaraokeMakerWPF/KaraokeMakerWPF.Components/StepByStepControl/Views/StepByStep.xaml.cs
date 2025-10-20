using KaraokeMakerWPF.Components.StepByStepControl.Models;
using KaraokeMakerWPF.Components.StepByStepControl.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace KaraokeMakerWPF.Components.StepByStepControl.Views;

/// <summary>
/// Логика взаимодействия для StepByStep.xaml
/// </summary>
public partial class StepByStep : UserControl
{
    public ObservableCollection<StepInfo> Steps { get; set; } = [];

    public int _currentIndex = -1;
    public int CurrentIndex => _currentIndex;

    // TODO: вынести в отдельный компонент? StepByStepControlWPF?
    public StepByStep()
    {
        InitializeComponent();
        Items.ItemsSource = Steps;
    }

    public void SetSteps(string[] steps)
    {
        for (var i = 0; i < steps.Length; i++)
        {
            Steps.Add(new StepInfo(steps[i], i + 1));
        }
    }

    public void SetIndex(int idx)
    {
        _currentIndex = idx;
        Refresh();
    }

    public void Previous()
    {
        _currentIndex--;
        if (_currentIndex < 0)
        {
            _currentIndex = 0;
        }
        Refresh();
    }

    public void Next()
    {
        _currentIndex++;
        if (_currentIndex > Steps.Count)
        {
            _currentIndex = Steps.Count;
        }
        Refresh();
    }

    private void Refresh()
    {
        for (int i = 0; i < Steps.Count; i++)
        {
            var step = Steps[i];
            var isLast = i == Steps.Count - 1;
            if (i < _currentIndex)
            {
                step.LeftPath = StepStatus.Previous;
                step.RightPath = StepStatus.Previous;
                step.Circle = StepStatus.Previous;
            }
            else if (i == _currentIndex)
            {
                step.LeftPath = StepStatus.Previous;
                step.RightPath = StepStatus.Next;
                step.Circle = StepStatus.Active;
            }
            else if (i > _currentIndex)
            {
                step.LeftPath = StepStatus.Next;
                step.RightPath = StepStatus.Next;
                step.Circle = StepStatus.Next;
            }

            if (i == 0)
            {
                step.LeftPath = StepStatus.None;
            }
            if (isLast)
            {
                step.RightPath = StepStatus.None;
            }

            step.Refresh();
        }
    }
}
