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
        var device = texture.GraphicsDevice;
        var width = texture.Width;
        var height = texture.Height;

        var data = GpuHelper.GetInitialData(width * scale, height);

        using var initialDataBuffer = device.AllocateReadOnlyBuffer(data);
        using var intermediat = device.AllocateReadWriteBuffer<float>(checked(width * height * scale));
        using var output = device.AllocateReadWriteBuffer<float>(checked(width * height));
        using var perlinNoise = device.AllocateReadWriteBuffer<float>(checked(width * height));
        using var rgb = device.AllocateReadWriteBuffer<float3>(checked(width * height));

        var context = device.CreateComputeContext();
        context.ZToCos(initialDataBuffer, intermediat);
        context.Barrier(intermediat);
        context.Reduce(intermediat, output, scale);
        context.Barrier(output);
        context.For(width, height, new PerlinNoiseShader(this.NeighborDistance, output, perlinNoise));
        context.Barrier(perlinNoise);
        context.ToRgb(perlinNoise, rgb);
        context.Barrier(rgb);
        context.ForEach(texture, new DisplayShader(rgb));
        context.Dispose();

        return true;
    }

    #endregion
}