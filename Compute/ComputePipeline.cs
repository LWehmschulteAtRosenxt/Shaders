namespace Compute;

using ComputeSharp;

public static class ComputePipeline
{
    #region Methods

    public static void ToFloat3(
        this GraphicsDevice device,
        ReadOnlyBuffer<float> gpuBuffer,
        ReadWriteBuffer<float3> gpuOutputBuffer,
        float scaleR = 1.0f,
        float scaleB = 1.0f,
        float scaleG = 1.0f)

    {
        device.For(gpuBuffer.Length, new ToRgb(scaleR, scaleB, scaleG, gpuBuffer.Length, gpuBuffer, gpuOutputBuffer));
    }

    public static void ZToCos(this GraphicsDevice device, ReadOnlyBuffer<float> gpuBuffer, ReadWriteBuffer<float> gpuOutputBuffer)
    {
        device.For(gpuBuffer.Length, new ZToCosShader(gpuBuffer.Length, gpuBuffer, gpuOutputBuffer));
    }

    #endregion
}

[ThreadGroupSize(DefaultThreadGroupSizes.XY)]
[GeneratedComputeShaderDescriptor]
public readonly partial struct ZToCosShader(int bufferSize, ReadOnlyBuffer<float> input, ReadWriteBuffer<float> output) : IComputeShader
{
    #region Methods

    public void Execute()
    {
        int index = ThreadIds.X + (ThreadIds.Y * ThreadIds.X);

        if (index < bufferSize)
        {
            output[index] = Hlsl.Cos(input[index]);
        }
    }

    #endregion
}

[ThreadGroupSize(DefaultThreadGroupSizes.XY)]
[GeneratedComputeShaderDescriptor]
public readonly partial struct ToRgb(float scaleR, float scaleB, float scaleG, int bufferSize, ReadOnlyBuffer<float> input, ReadWriteBuffer<float3> output)
    : IComputeShader
{
    #region Methods

    public void Execute()
    {
        int index = ThreadIds.X + (ThreadIds.Y * ThreadIds.X);

        if (index < bufferSize)
        {
            output[index].R = scaleR * input[index];
            output[index].G = scaleG * input[index];
            output[index].B = scaleB * input[index];
        }
    }

    #endregion
}