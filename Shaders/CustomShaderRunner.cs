namespace Compute;

using ComputeSharp;
using ComputeSharp.WinUI;
using Shaders;

public class CustomShaderRunner() : IShaderRunner
{
    public int NeighborDistance { get; set; }

    public bool TryExecute(IReadWriteNormalizedTexture2D<float4> texture, TimeSpan timespan, object? parameter)
    {
        var device = texture.GraphicsDevice;
        var width = texture.Width;
        var height = texture.Height;

        using var random = device.AllocateReadWriteBuffer<float>(checked(width * height));
        using var noise = device.AllocateReadWriteBuffer<float>(checked(width * height));
        using var rgb = device.AllocateReadWriteBuffer<float3>(checked(width * height));
        using var context = device.CreateComputeContext();

        context.For(checked(width * height), new RandomShader(random));
        context.Barrier(random);
        context.For(width, height, new PerlinNoiseShader(this.NeighborDistance, random, noise));
        context.Barrier(noise);
        context.For(checked(width * height), new ToRgb(noise, rgb));
        context.Barrier(rgb);
        context.ForEach(texture, new DisplayShader(rgb, width));

        return true;
    }
}