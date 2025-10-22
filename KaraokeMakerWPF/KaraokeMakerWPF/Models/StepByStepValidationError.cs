namespace KaraokeMakerWPF.Models;

public sealed class StepByStepValidationError
{
    public bool IsValid { get; set; }
    public string? Message { get; set; }

    private StepByStepValidationError() { }

    public static StepByStepValidationError Success()
    {
        return new StepByStepValidationError
        {
            IsValid = true
        };
    }

    public static StepByStepValidationError Error(string message)
    {
        return new StepByStepValidationError
        {
            IsValid = false,
            Message = message
        };
    }
}