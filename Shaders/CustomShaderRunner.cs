namespace Compute;

using ComputeSharp;
using ComputeSharp.WinUI;
using Shaders;

public class CustomShaderRunner : IShaderRunner
{
    public bool TryExecute(IReadWriteNormalizedTexture2D<float4> texture, TimeSpan timespan, object? parameter)
    {
        var device = texture.GraphicsDevice;
        var width = texture.Width;
        var height = texture.Height;

        using var random = device.AllocateReadWriteBuffer<float>(checked(width * height));
        using var rgb = device.AllocateReadWriteBuffer<float3>(checked(width * height));
        using var context = device.CreateComputeContext();

        context.For(checked(width * height), new RandomShader(random));
        context.For(checked(width * height), new ToRgb(random, rgb));
        context.ForEach(texture, new DisplayShader(rgb, width));

        return true;
    }
}