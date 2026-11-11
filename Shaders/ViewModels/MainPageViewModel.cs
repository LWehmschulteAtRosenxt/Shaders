namespace Shaders.ViewModels;

using System.ComponentModel;
using System.Windows.Input;
using ComputeSharp;
using ComputeSharp.WinUI;

public sealed class MainPageViewModel : INotifyPropertyChanged
{
    #region Properties

    public List<GraphicsDevice> GraphicsDevices { get; } = new(GraphicsDevice.EnumerateDevices());

    public string? ImageSource { get; set => this.NotifyPropertyChanged(this.PropertyChanged, ref field, value); }

    public GraphicsDevice? SelectedGraphicsDevice
    {
        get;
        set
        {
            this.LoadTextureIntoGPU(value);
            this.NotifyPropertyChanged(this.PropertyChanged, ref field, value);
        }
    } = GraphicsDevice.GetDefault();

    public IShaderRunner? ShaderRunner { get; private set => this.NotifyPropertyChanged(this.PropertyChanged, ref field, value); }

    public ICommand PickImageCommand { get; }

    #endregion

    private async Task LoadTextureIntoGPU(GraphicsDevice? device = null)
    {
        if (this.ImageSource is null)
        {
            return;
        }

        device ??= this.SelectedGraphicsDevice;

        await using var stream = File.OpenRead(this.ImageSource);
        if (device == null)
        {
            return;
        }

        var sourceTexture = device.LoadReadOnlyTexture2D<Bgra32, float4>(stream);
        this.ShaderRunner = new ShaderRunner<HelloWorldShader>(elapsed => new HelloWorldShader(sourceTexture, (float)elapsed.TotalSeconds));
    }

    public MainPageViewModel()
    {
        this.PickImageCommand = new RelayCommand(async _ =>
        {
            var file = await FilePicker.PickAsync(
                new PickOptions()
                {
                    PickerTitle = "Choose an image",
                    FileTypes = FilePickerFileType.Images
                });

            if (file is null)
            {
                return;
            }

            this.ImageSource = file.FullPath;

            this.LoadTextureIntoGPU();
        });
    }

    #region Events

    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion
}