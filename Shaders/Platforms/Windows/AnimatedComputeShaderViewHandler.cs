namespace Shaders.Platforms.Windows;

using ComputeSharp;
using ComputeSharp.WinUI;
using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml.Controls;
using Shaders.Controls;

public sealed class AnimatedComputeShaderViewHandler() : ViewHandler<AnimatedComputeShaderView, Grid>(Mapper)
{
    #region Fields

    private static readonly IPropertyMapper<AnimatedComputeShaderView, AnimatedComputeShaderViewHandler> Mapper =
        new PropertyMapper<AnimatedComputeShaderView, AnimatedComputeShaderViewHandler>(ViewHandler.ViewMapper)
        {
            [nameof(AnimatedComputeShaderView.ShaderRunner)] = MapShaderRunner,
            [nameof(AnimatedComputeShaderView.GraphicsDevice)] = MapGraphicsDevice
        };

    private AnimatedComputeShaderPanel? shaderPanel;
    private GraphicsDevice? graphicsDevice;

    #endregion

    #region Methods

    protected override Grid CreatePlatformView() => new();

    protected override void DisconnectHandler(Grid platformView)
    {
        if (this.shaderPanel is not null)
        {
            this.shaderPanel.ShaderRunner = null;
            platformView.Children.Remove(this.shaderPanel);
            this.shaderPanel.Dispose();
            this.shaderPanel = null;
            this.graphicsDevice = null;
        }

        base.DisconnectHandler(platformView);
    }

    private static void MapGraphicsDevice(AnimatedComputeShaderViewHandler handler, AnimatedComputeShaderView view)
    {
        var graphicsDevice = view.GraphicsDevice ?? GraphicsDevice.GetDefault();

        if (handler.shaderPanel is not null && ReferenceEquals(handler.graphicsDevice, graphicsDevice))
        {
            return;
        }

        var newPanel = new AnimatedComputeShaderPanel(graphicsDevice)
        {
            IsDynamicResolutionEnabled = false,
            IsVerticalSyncEnabled = true,
            ResolutionScale = 1.0,
            ShaderRunner = view.ShaderRunner
        };

        if (handler.shaderPanel is not null)
        {
            handler.shaderPanel.ShaderRunner = null;
            handler.PlatformView.Children.Remove(handler.shaderPanel);
            handler.shaderPanel.Dispose();
        }

        handler.PlatformView.Children.Add(newPanel);
        handler.shaderPanel = newPanel;
        handler.graphicsDevice = graphicsDevice;
    }

    private static void MapShaderRunner(AnimatedComputeShaderViewHandler handler, AnimatedComputeShaderView view)
    {
        if (handler.shaderPanel is not null)
        {
            handler.shaderPanel.ShaderRunner = view.ShaderRunner;
        }
    }

    #endregion
}