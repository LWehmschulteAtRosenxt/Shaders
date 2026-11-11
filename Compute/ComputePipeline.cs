namespace Compute;

using ComputeSharp;

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
public readonly partial struct ToRgb(float scaleR, float scaleB, float scaleG, ReadWriteBuffer<float> input, ReadWriteBuffer<float3> output) : IComputeShader
{
    #region Methods

    public void Execute()
    {
        int index = ThreadIds.X;

        output[index].R = scaleR * input[index];
        output[index].G = scaleG * input[index];
        output[index].B = scaleB * input[index];
    }

    #endregion
}