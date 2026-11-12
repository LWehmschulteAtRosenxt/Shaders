namespace Shaders;

using System.ComponentModel;
using System.Runtime.CompilerServices;

public static class INotifyPropertyChangedExtensions
{
    extension(INotifyPropertyChanged parent)
    {
        #region Methods

        public bool NotifyPropertyChanged<T>(PropertyChangedEventHandler? handler, ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (ReferenceEquals(field, value))
            {
                return false;
            }

            field = value;
            handler?.Invoke(parent, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        #endregion
    }
}