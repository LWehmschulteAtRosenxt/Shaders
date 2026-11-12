namespace ShadersBenchmark;

using BenchmarkDotNet.Attributes;
using Compute;
using ComputeSharp;

public class ShaderBenchmark
{
    #region Fields

    private float[] data = Array.Empty<float>();

    private const int height = 100;

    private float[] output = Array.Empty<float>();

    private const int scale = 4;

    private const int width = 1920;

    #endregion

    #region Methods

    [Benchmark]
    public void Cpu()
    {
        for (int i = 0; i < this.data.Length; i++)
        {
            var cos = (float)Math.Cos(this.data[i]);
            int outIndex = i / scale;
            if (outIndex < this.output.Length)
            {
                this.output[outIndex] += cos;
            }
        }
    }

    [Benchmark]
    public void Gpu()
    {
        var device = GpuHelper.GetMaxMemoryDevice();
        if (device is null)
        {
            throw new InvalidOperationException("No suitable GPU device found.");
        }

        var gpuInput = device.AllocateReadOnlyBuffer(this.data);
        var gpuIntermediate = device.AllocateReadWriteBuffer<float>(this.data.Length);
        var gpuOutput = device.AllocateReadWriteBuffer<float>(this.output.Length);
        var context = device.CreateComputeContext();
        context.ZToCos(gpuInput, gpuIntermediate);
        context.Reduce(gpuIntermediate, gpuOutput, scale);
        context.Dispose();
        _ = gpuOutput.ToArray();
    }

    [GlobalSetup]
    public void Setup()
    {
        this.data = GpuHelper.GetInitialData(width * scale, height * scale).ToArray();
        this.output = new float[width * height];
        Array.Fill(this.output, 0f);
    }

    #endregion
}