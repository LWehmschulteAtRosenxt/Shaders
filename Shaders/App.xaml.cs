namespace Shaders
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            UserAppTheme = PlatformAppTheme;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new MainPage());
            window.Title = "Shaders";
            return window;
        }
    }
}
