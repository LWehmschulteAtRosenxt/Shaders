namespace Shaders.Platforms.Windows;

using ComputeSharp.WinUI;
using Microsoft.Maui.Handlers;
using Shaders.Controls;

public sealed class AnimatedComputeShaderViewHandler() : ViewHandler<AnimatedComputeShaderView, AnimatedComputeShaderPanel>(Mapper)
{
    #region Fields

    private static readonly IPropertyMapper<AnimatedComputeShaderView, AnimatedComputeShaderViewHandler> Mapper =
        new PropertyMapper<AnimatedComputeShaderView, AnimatedComputeShaderViewHandler>(ViewHandler.ViewMapper)
        {
            [nameof(IView.Background)] = MapBackground, [nameof(AnimatedComputeShaderView.ShaderRunner)] = MapShaderRunner
        };

    #endregion

    #region Methods

    protected override AnimatedComputeShaderPanel CreatePlatformView() =>
        new() { IsDynamicResolutionEnabled = false, IsVerticalSyncEnabled = true, ResolutionScale = 1.0 };

    protected override void DisconnectHandler(AnimatedComputeShaderPanel platformView)
    {
        platformView.ShaderRunner = null;
        platformView.Dispose();

        base.DisconnectHandler(platformView);
    }

    private static void MapBackground(AnimatedComputeShaderViewHandler handler, AnimatedComputeShaderView view)
    {
    }

    private static void MapShaderRunner(AnimatedComputeShaderViewHandler handler, AnimatedComputeShaderView view)
    {
        handler.PlatformView.ShaderRunner = view.ShaderRunner;
    }

    #endregion
}