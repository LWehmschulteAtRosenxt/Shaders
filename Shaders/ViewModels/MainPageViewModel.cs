namespace Shaders.ViewModels;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using ComputeSharp;
using ComputeSharp.WinUI;

public sealed class MainPageViewModel : INotifyPropertyChanged
{
    #region Properties

    public List<GraphicsDevice> GraphicsDevices { get; } = new(GraphicsDevice.EnumerateDevices());

    public GraphicsDevice? SelectedGraphicsDevice
    {
        get;
        set => this.NotifyPropertyChanged(this.PropertyChanged, ref field, value);
    } = GraphicsDevice.GetDefault();

    public IShaderRunner ShaderRunner { get; } = new ShaderRunner<HelloWorldShader>(static elapsed => new HelloWorldShader((float)elapsed.TotalSeconds));

    #endregion

    #region Events

    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion
}