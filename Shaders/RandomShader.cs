namespace Shaders;

using ComputeSharp;

[ThreadGroupSize(DefaultThreadGroupSizes.X)]
[GeneratedComputeShaderDescriptor]
public readonly partial struct RandomShader(ReadWriteBuffer<float> output) : IComputeShader
{
    public void Execute()
    {
        var index = ThreadIds.X;
        var state = (uint)index + 0x9e3779b9u;
        state = (state ^ (state >> 16)) * 0x7feb352du;
        state = (state ^ (state >> 15)) * 0x846ca68bu;
        state ^= state >> 16;

        output[index] = (state >> 8) * (1f / 16777216f);
    }
}
