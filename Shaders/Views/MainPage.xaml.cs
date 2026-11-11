namespace Shaders.Views;

using Shaders.ViewModels;

public partial class MainPage : ContentPage
{
    #region Constructors

    public MainPage(MainPageViewModel viewModel)
    {
        this.InitializeComponent();
        this.BindingContext = viewModel;

        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.NeighborDistance))
            {
                this.shaderView.RequestFrame();
            }
        };
    }

    #endregion
}