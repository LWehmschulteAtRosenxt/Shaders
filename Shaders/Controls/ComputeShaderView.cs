namespace Shaders.Controls;

using ComputeSharp;
using ComputeSharp.WinUI;

public sealed class ComputeShaderView : View
{
    #region Fields

    public static readonly BindableProperty ShaderRunnerProperty = BindableProperty.Create(
        nameof(ShaderRunner),
        typeof(IShaderRunner),
        typeof(ComputeShaderView));

    public static readonly BindableProperty GraphicsDeviceProperty = BindableProperty.Create(
        nameof(GraphicsDevice),
        typeof(GraphicsDevice),
        typeof(ComputeShaderView));

    #endregion

    #region Events

    internal event EventHandler? FrameRequested;

    #endregion

    #region Properties

    public IShaderRunner? ShaderRunner
    {
        get => (IShaderRunner?)this.GetValue(ShaderRunnerProperty);
        set => this.SetValue(ShaderRunnerProperty, value);
    }

    public GraphicsDevice? GraphicsDevice
    {
        get => (GraphicsDevice?)this.GetValue(GraphicsDeviceProperty);
        set => this.SetValue(GraphicsDeviceProperty, value);
    }

    #endregion

    #region Methods

    public void RequestFrame() => this.FrameRequested?.Invoke(this, EventArgs.Empty);

    #endregion
}