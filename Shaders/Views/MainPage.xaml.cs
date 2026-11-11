namespace Shaders.Views;

using ComputeSharp.WinUI;

public partial class MainPage : ContentPage
{
    #region Constructors

    public MainPage()
    {
        this.InitializeComponent();

        this.ShaderView.ShaderRunner = new ShaderRunner<HelloWorldShader>(static elapsed => new HelloWorldShader((float)elapsed.TotalSeconds));
    }

    #endregion
}