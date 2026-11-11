namespace Shaders.Controls;

using ComputeSharp;
using ComputeSharp.WinUI;

public sealed class AnimatedComputeShaderView : View
{
    #region Fields

    public static readonly BindableProperty ShaderRunnerProperty = BindableProperty.Create(
        nameof(ShaderRunner),
        typeof(IShaderRunner),
        typeof(AnimatedComputeShaderView));

    public static readonly BindableProperty GraphicsDeviceProperty = BindableProperty.Create(
        nameof(GraphicsDevice),
        typeof(GraphicsDevice),
        typeof(AnimatedComputeShaderView));

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
}