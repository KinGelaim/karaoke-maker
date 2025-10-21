using System.Diagnostics;
using System.Windows.Input;

namespace KaraokeMakerWPF.Environment;

public class DelegateCommand : ICommand
{
    private readonly Action _executeMethod;

    private readonly Func<bool>? _canExecuteMethod;

    public DelegateCommand(Action execute) : this(execute, null) { }

    public DelegateCommand(Action execute, Func<bool>? canExecute)
    {
        ArgumentNullException.ThrowIfNull(execute);

        _executeMethod = execute;
        _canExecuteMethod = canExecute;
    }

    public bool CanExecute() => _canExecuteMethod == null || _canExecuteMethod();

    public void Execute()
    {
        if (_executeMethod == null)
        {
            return;
        }

        _executeMethod();
    }

    #region ICommand

    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    [DebuggerStepThrough]
    bool ICommand.CanExecute(object? parameter) => CanExecute();

    void ICommand.Execute(object? parameter) => this.Execute();

    #endregion ICommand
}

public class DelegateCommand<T> : ICommand
{
    private readonly Action<T> _executeMethod;

    private readonly Func<T, bool>? _canExecuteMethod;

    public DelegateCommand(Action<T> execute) : this(execute, null) { }

    public DelegateCommand(Action<T> execute, Func<T, bool>? canExecute)
    {
        ArgumentNullException.ThrowIfNull(execute);

        _executeMethod = execute;
        _canExecuteMethod = canExecute;
    }

    public bool CanExecute(T parameter) => _canExecuteMethod == null || _canExecuteMethod(parameter);

    public void Execute(T parameter)
    {
        if (_executeMethod == null)
        {
            return;
        }

        _executeMethod(parameter);
    }

    #region ICommand

    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    [DebuggerStepThrough]
    bool ICommand.CanExecute(object parameter)
    {
        return parameter != null || !typeof(T).IsValueType
            ? CanExecute((T)parameter)
            : _canExecuteMethod == null;
    }

    void ICommand.Execute(object parameter) => Execute((T)parameter);

    #endregion ICommand
}