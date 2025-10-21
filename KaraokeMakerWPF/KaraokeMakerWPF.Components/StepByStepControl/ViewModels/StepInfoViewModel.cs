using KaraokeMakerWPF.Components.StepByStepControl.Models;
using System.ComponentModel;

namespace KaraokeMakerWPF.Components.StepByStepControl.ViewModels;

public class StepInfoViewModel : INotifyPropertyChanged
{
    public string DisplayName { get; set; }
    public int DisplayNumber { get; set; }

    public StepStatus LeftPath { get; set; } = StepStatus.None;
    public StepStatus RightPath { get; set; } = StepStatus.None;
    public StepStatus Circle { get; set; } = StepStatus.Next;

    public StepInfoViewModel(string displayName, int displayNumber)
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

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion INotifyPropertyChanged
}
