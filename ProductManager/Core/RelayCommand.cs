using System.Windows.Input;

namespace ProductManager.Core;
public class RelayCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    private Action<object> _execute { get; set; }
    private Predicate<object> _canExecute { get; set; }

    public RelayCommand(Action<object> executeMethod, Predicate<object> canExecuteMethod)
    {
        _execute = executeMethod;
        _canExecute = canExecuteMethod;
    }

    public bool CanExecute(object? parameter)
    {
        if (parameter != null)
            return _canExecute(parameter);
        return false;
    }

    public void Execute(object? parameter)
    {
        if (parameter != null)
            _execute(parameter);
    }
}