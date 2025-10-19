using System.ComponentModel;

namespace KaraokeMakerWPF.Components.StepByStepControl.Models;

public class StepInfo : INotifyPropertyChanged
{
    public string DisplayName { get; set; }
    public int DisplayNumber { get; set; }

    public StepStatus LeftPath { get; set; } = StepStatus.None;
    public StepStatus RightPath { get; set; } = StepStatus.None;
    public StepStatus Circle { get; set; } = StepStatus.Next;

    public StepInfo(string displayName, int displayNumber)
    {
        DisplayName = displayName;
        DisplayNumber = displayNumber;
    }

    public void Refresh()
    {
        OnPropertyChanged(nameof(Circle));
        OnPropertyChanged(nameof(LeftPath));
        OnPropertyChanged(nameof(RightPath));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
