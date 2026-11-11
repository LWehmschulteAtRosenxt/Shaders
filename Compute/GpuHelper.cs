namespace Compute;

using CommunityToolkit.HighPerformance;
using ComputeSharp;

public static class GpuHelper
{
    #region Methods

    public static ReadOnlySpan<float> GetInitialData(int rows, int cols)
    {
        return GetRandomPixelArray(rows, cols).To1DSpan();
    }

    public static GraphicsDevice? GetMaxMemoryDevice()
    {
        return GraphicsDevice.EnumerateDevices().MaxBy(d => d.DedicatedMemorySize);
    }

    public static float[,] GetRandomPixelArray(int rows, int cols)
    {
        var array = new float[rows, cols];
        Random random = new Random();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                array[i, j] = (float)(random.NextDouble() * 2 - 1); // Random float between -1 and 1
            }
        }

        return array;
    }

    public static ReadOnlySpan<T> To1DSpan<T>(this T[,] array)
    {
        return array.AsSpan();
    }

    public static bool TryArrayFitIntoGpu<T>(ref T[] array)
        where T : unmanaged
    {
        try
        {
            using var device = GetMaxMemoryDevice();
            if (device is null)
            {
                return false;
            }

            using var buffer = device.AllocateReadOnlyBuffer<T>(array);
            using var w = device.AllocateReadWriteBuffer<T>(array.Length);

            return true;
        }
        catch
        {
            return false;
        }
    }

    #endregion
}