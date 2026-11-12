namespace Shaders.Controls;

using ComputeSharp;
using ComputeSharp.WinUI;

public sealed class ComputeShaderView : View
{
    #region Fields

    public static readonly BindableProperty GraphicsDeviceProperty = BindableProperty.Create(
        nameof(GraphicsDevice),
        typeof(GraphicsDevice),
        typeof(ComputeShaderView));

    public static readonly BindableProperty ShaderRunnerProperty = BindableProperty.Create(
        nameof(ShaderRunner),
        typeof(IShaderRunner),
        typeof(ComputeShaderView));

    #endregion

    #region Properties

    public GraphicsDevice? GraphicsDevice
    {
        get => (GraphicsDevice?)this.GetValue(GraphicsDeviceProperty);
        set => this.SetValue(GraphicsDeviceProperty, value);
    }

    public IShaderRunner? ShaderRunner
    {
        get => (IShaderRunner?)this.GetValue(ShaderRunnerProperty);
        set => this.SetValue(ShaderRunnerProperty, value);
    }

    #endregion

    #region Events

    internal event EventHandler? FrameRequested;

    #endregion

    #region Methods

    public void RequestFrame() => this.FrameRequested?.Invoke(this, EventArgs.Empty);

    #endregion
}