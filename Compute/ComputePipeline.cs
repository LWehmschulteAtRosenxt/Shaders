namespace Compute;

using ComputeSharp;

public static class ComputePipeline
{
    #region Methods

    extension(ref ComputeContext context)
    {
        public void Reduce(ReadWriteBuffer<float> gpuBuffer, ReadWriteBuffer<float> gpuOutputBuffer, int binSize)
        {
            context.For(gpuOutputBuffer.Length, new ReduceShader(binSize, gpuBuffer, gpuOutputBuffer));
        }

        public void ToFloat3(ReadWriteBuffer<float> gpuBuffer, ReadWriteBuffer<float3> gpuOutputBuffer)

        {
            context.For(gpuBuffer.Length, new ToRgb(gpuBuffer, gpuOutputBuffer));
        }

        public void ZToCos(ReadOnlyBuffer<float> gpuBuffer, ReadWriteBuffer<float> gpuOutputBuffer)
        {
            context.For(gpuBuffer.Length, new ZToCosShader(gpuBuffer, gpuOutputBuffer));
        }

        public void ToRgb(ReadWriteBuffer<float> gpuBuffer, ReadWriteBuffer<float3> gpuOutputBuffer)
        {
            context.For(gpuBuffer.Length, new ToRgb(gpuBuffer, gpuOutputBuffer));
        }
    }

    #endregion
}

[ThreadGroupSize(DefaultThreadGroupSizes.X)]
[GeneratedComputeShaderDescriptor]
public readonly partial struct ReduceShader(int binSize, ReadWriteBuffer<float> input, ReadWriteBuffer<float> output) : IComputeShader
{
    #region Methods

    public void Execute()
    {
        int index = ThreadIds.X;

        float sum = 0f;
        for (int i = 0; i < binSize; i++)
        {
            var inputIndex = (index * binSize) + i;
            sum += (1f / binSize) * input[inputIndex];
        }

        output[index] = sum;
    }

    #endregion
}

[ThreadGroupSize(DefaultThreadGroupSizes.X)]
[GeneratedComputeShaderDescriptor]
public readonly partial struct ZToCosShader(ReadOnlyBuffer<float> input, ReadWriteBuffer<float> output) : IComputeShader
{
    #region Methods

    public void Execute()
    {
        int index = ThreadIds.X;

        output[index] = Hlsl.Cos(input[index]);
    }

    #endregion
}

[ThreadGroupSize(DefaultThreadGroupSizes.X)]
[GeneratedComputeShaderDescriptor]
public readonly partial struct ToRgb(ReadWriteBuffer<float> input, ReadWriteBuffer<float3> output) : IComputeShader
{
    #region Methods

    public void Execute()
    {
        var index = ThreadIds.X;
        var value = input[index];

        float3 phase = new(0f, 0.33f, 0.67f);

        output[index] = 0.5f + 0.5f * Hlsl.Cos(6.2831853f * (value + phase));
    }

    #endregion
}