namespace Shaders.ViewModels;

using System.ComponentModel;
using System.Windows.Input;
using Compute;
using ComputeSharp;
using ComputeSharp.WinUI;

public sealed class MainPageViewModel : INotifyPropertyChanged
{
    #region Properties

    public List<GraphicsDevice> GraphicsDevices { get; } = new(GraphicsDevice.EnumerateDevices());

    public string? ImageSource { get; set => this.NotifyPropertyChanged(this.PropertyChanged, ref field, value); }

    public GraphicsDevice? SelectedGraphicsDevice
    {
        get;
        set => this.NotifyPropertyChanged(this.PropertyChanged, ref field, value);
    } = GraphicsDevice.GetDefault();

    public IShaderRunner? ShaderRunner { get; } = new CustomShaderRunner();

    #endregion

    #region Events

    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion
}