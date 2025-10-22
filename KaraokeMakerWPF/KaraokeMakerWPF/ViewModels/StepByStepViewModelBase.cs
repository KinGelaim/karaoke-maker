using KaraokeMakerWPF.Environment;
using KaraokeMakerWPF.Models;

namespace KaraokeMakerWPF.ViewModels;

public abstract class StepByStepViewModelBase : NotificationObject
{
    public virtual StepByStepValidationError ValidateBeforeNextStep()
    {
        return StepByStepValidationError.Success();
    }
}