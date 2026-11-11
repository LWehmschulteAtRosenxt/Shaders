namespace Shaders;

using Shaders.Controls;
using Shaders.Platforms.Windows;

public static partial class MauiProgram
{
    #region Methods

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-SemiBold.ttf", "OpenSansSemiBold");
            });

        builder.ConfigureMauiHandlers(handlers => { handlers.AddHandler<AnimatedComputeShaderView, AnimatedComputeShaderViewHandler>(); });

        return builder.Build();
    }

    #endregion
}