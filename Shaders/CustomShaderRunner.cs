namespace Compute;

using ComputeSharp;
using ComputeSharp.WinUI;

public class CustomShaderRunner() : IShaderRunner
{
    #region Properties

    public int NeighborDistance { get; set; }

    #endregion

    #region Methods

    public bool TryExecute(IReadWriteNormalizedTexture2D<float4> texture, TimeSpan timespan, object? parameter)
    {
        var scale = 2;
        var textureDevice = texture.GraphicsDevice;
        var width = texture.Width;
        var height = texture.Height;

        var device = GpuHelper.GetMaxMemoryDevice();
        if (device is null)
        {
            device = textureDevice;
        }

        var data = GpuHelper.GetInitialData(width * scale, height * scale);

        using var initialDataBuffer = device.AllocateReadOnlyBuffer(data);
        using var intermediat = device.AllocateReadWriteBuffer<float>(checked(width * height));
        using var output = device.AllocateReadWriteBuffer<float>(checked(width * height));
        ;
        using var rgb = device.AllocateReadWriteBuffer<float3>(checked(width * height));
        using var context = device.CreateComputeContext();

        device.ZToCos(initialDataBuffer, intermediat);
        context.Barrier(intermediat);
        device.Reduce(intermediat, output, scale);
        context.Barrier(output);
        device.ToRgb(output, rgb);
        context.Barrier(rgb);
        context.ForEach(texture, new DisplayShader(rgb));

        return true;
    }

    #endregion
}