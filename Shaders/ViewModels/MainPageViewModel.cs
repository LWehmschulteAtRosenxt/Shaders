namespace Shaders.ViewModels;

using System.ComponentModel;
using Compute;
using ComputeSharp;

public sealed class MainPageViewModel : INotifyPropertyChanged
{
    #region Properties

    public List<GraphicsDevice> GraphicsDevices { get; } = new(GraphicsDevice.EnumerateDevices());

    public string? ImageSource
    {
        get;
        set => this.NotifyPropertyChanged(this.PropertyChanged, ref field, value);
    }

    public int NeighborDistance
    {
        get;
        set
        {
            if (this.ShaderRunner?.NeighborDistance == value)
            {
                return;
            }

            this.ShaderRunner?.NeighborDistance = value;
            this.NotifyPropertyChanged(this.PropertyChanged, ref field, value);
        }
    }

    public GraphicsDevice? SelectedGraphicsDevice
    {
        get;
        set => this.NotifyPropertyChanged(this.PropertyChanged, ref field, value);
    } = GraphicsDevice.GetDefault();

    public CustomShaderRunner? ShaderRunner { get; } = new() { NeighborDistance = 0 };

    #endregion

    #region Events

    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion
}