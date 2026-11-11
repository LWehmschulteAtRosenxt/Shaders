namespace Shaders.Controls;

using ComputeSharp.WinUI;

public sealed class AnimatedComputeShaderView : View
{
    #region Fields

    public static readonly BindableProperty ShaderRunnerProperty = BindableProperty.Create(
        nameof(ShaderRunner),
        typeof(IShaderRunner),
        typeof(AnimatedComputeShaderView));

    #endregion

    #region Properties

    public IShaderRunner? ShaderRunner
    {
        get => (IShaderRunner?)GetValue(ShaderRunnerProperty);
        set => SetValue(ShaderRunnerProperty, value);
    }

    #endregion
}