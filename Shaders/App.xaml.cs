namespace Shaders;

public partial class App : Application
{
    #region Constructors

    public App()
    {
        this.InitializeComponent();

        this.UserAppTheme = this.PlatformAppTheme;
    }

    #endregion

    #region Methods

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new MainPage());
        window.Title = "Shaders";
        return window;
    }

    #endregion
}