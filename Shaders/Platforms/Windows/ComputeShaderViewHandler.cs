namespace Shaders.Platforms.Windows;

using ComputeSharp;
using ComputeSharp.WinUI;
using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Shaders.Controls;

public sealed class ComputeShaderViewHandler() : ViewHandler<ComputeShaderView, Grid>(Mapper)
{
    #region Fields

    private FrameRequestQueue? frameRequests;

    private GraphicsDevice? graphicsDevice;

    private static readonly IPropertyMapper<ComputeShaderView, ComputeShaderViewHandler> Mapper =
        new PropertyMapper<ComputeShaderView, ComputeShaderViewHandler>(ViewHandler.ViewMapper)
        {
            [nameof(ComputeShaderView.ShaderRunner)] = MapShaderRunner, [nameof(ComputeShaderView.GraphicsDevice)] = MapGraphicsDevice
        };

    private ComputeShaderPanel? shaderPanel;

    #endregion

    #region Methods

    protected override void ConnectHandler(Grid platformView)
    {
        base.ConnectHandler(platformView);
        this.VirtualView.FrameRequested += this.OnFrameRequested;
    }

    protected override Grid CreatePlatformView() => new();

    protected override void DisconnectHandler(Grid platformView)
    {
        this.VirtualView.FrameRequested -= this.OnFrameRequested;

        if (this.shaderPanel is not null)
        {
            this.shaderPanel.Loaded -= this.OnPanelLoaded;
            this.shaderPanel.SizeChanged -= this.OnPanelSizeChanged;
            this.shaderPanel.ShaderRunner = null;
            platformView.Children.Remove(this.shaderPanel);
            this.shaderPanel.Dispose();
            this.shaderPanel = null;
            this.graphicsDevice = null;
            this.frameRequests = null;
        }

        base.DisconnectHandler(platformView);
    }

    private static void MapGraphicsDevice(ComputeShaderViewHandler handler, ComputeShaderView view)
    {
        var graphicsDevice = view.GraphicsDevice ?? GraphicsDevice.GetDefault();

        if (handler.shaderPanel is not null && ReferenceEquals(handler.graphicsDevice, graphicsDevice))
        {
            return;
        }

        var frameRequests = new FrameRequestQueue();
        var newPanel = new ComputeShaderPanel(graphicsDevice)
                       {
                           IsDynamicResolutionEnabled = false,
                           IsVerticalSyncEnabled = true,
                           ResolutionScale = 1.0,
                           FrameRequestQueue = frameRequests,
                           ShaderRunner = view.ShaderRunner
                       };

        newPanel.Loaded += handler.OnPanelLoaded;
        newPanel.SizeChanged += handler.OnPanelSizeChanged;

        if (handler.shaderPanel is not null)
        {
            handler.shaderPanel.Loaded -= handler.OnPanelLoaded;
            handler.shaderPanel.SizeChanged -= handler.OnPanelSizeChanged;
            handler.shaderPanel.ShaderRunner = null;
            handler.PlatformView.Children.Remove(handler.shaderPanel);
            handler.shaderPanel.Dispose();
        }

        handler.PlatformView.Children.Add(newPanel);
        handler.shaderPanel = newPanel;
        handler.graphicsDevice = graphicsDevice;
        handler.frameRequests = frameRequests;
        frameRequests.RequestFrame();
    }

    private static void MapShaderRunner(ComputeShaderViewHandler handler, ComputeShaderView view)
    {
        if (handler.shaderPanel is not null)
        {
            handler.shaderPanel.ShaderRunner = view.ShaderRunner;
            handler.frameRequests?.RequestFrame();
        }
    }

    private void OnFrameRequested(object? sender, EventArgs args) => this.frameRequests?.RequestFrame();

    private void OnPanelLoaded(object sender, RoutedEventArgs args) => this.frameRequests?.RequestFrame();

    private void OnPanelSizeChanged(object sender, SizeChangedEventArgs args)
    {
        if (args.NewSize.Width > 0 && args.NewSize.Height > 0)
        {
            this.frameRequests?.RequestFrame();
        }
    }

    #endregion
}