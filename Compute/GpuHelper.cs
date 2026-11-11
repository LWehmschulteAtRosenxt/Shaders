namespace Compute;

using CommunityToolkit.HighPerformance;
using ComputeSharp;

public static class GpuHelper
{
    #region Methods

    public static GraphicsDevice? GetMaxMemoryDevice()
    {
        return GraphicsDevice.EnumerateDevices().MaxBy(d => d.DedicatedMemorySize);
    }

    public static void ThrowIfArrayToBug<T>(ref T[] array)
    {
    }
    
    public static ReadOnlySpan<T> To1DSpan<T>(this T[,] array)
    {
        return array.AsSpan();
    }

    #endregion
}