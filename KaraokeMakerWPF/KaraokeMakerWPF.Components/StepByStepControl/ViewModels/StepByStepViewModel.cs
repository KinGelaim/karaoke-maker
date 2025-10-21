using KaraokeMakerWPF.Components.StepByStepControl.Models;
using System.Collections.ObjectModel;

namespace KaraokeMakerWPF.Components.StepByStepControl.ViewModels;

public class StepByStepViewModel
{
    public ObservableCollection<StepInfoViewModel> Steps { get; set; } = [];

    public int _currentIndex = -1;
    public int CurrentIndex => _currentIndex;

    public void SetSteps(string[] steps)
    {
        Steps = new ObservableCollection<StepInfoViewModel>(
            steps.Select((stepName, index) => new StepInfoViewModel(stepName, index + 1)));
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