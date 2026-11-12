namespace Shaders;

using System.Windows.Input;

public class RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null) : ICommand
{
    #region Events

    public event EventHandler? CanExecuteChanged;

    #endregion

    #region Methods

    public bool CanExecute(object? parameter) => canExecute?.Invoke(parameter) ?? true;

    public void Execute(object? parameter)
    {
        if (this.CanExecute(parameter))
        {
            execute(parameter);
        }
    }

    #endregion
}