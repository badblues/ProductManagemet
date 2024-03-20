using System.Windows.Input;

namespace ProductManager.Commands;

public class RelayCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    private Action<object?> ExecuteMethod { get; set; }
    private Predicate<object?> CanExecuteMethod { get; set; }

    public RelayCommand(Action<object?> executeMethod, Predicate<object?> canExecuteMethod)
    {
        ExecuteMethod = executeMethod;
        CanExecuteMethod = canExecuteMethod;
    }

    public bool CanExecute(object? parameter)
    {
        return CanExecuteMethod(parameter);
    }

    public void Execute(object? parameter)
    {
        ExecuteMethod(parameter);
    }
}