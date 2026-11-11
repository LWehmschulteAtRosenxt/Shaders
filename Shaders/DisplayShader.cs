namespace Shaders;

using ComputeSharp;


[ThreadGroupSize(DefaultThreadGroupSizes.XY)]
[GeneratedComputeShaderDescriptor]
public readonly partial struct DisplayShader(ReadWriteBuffer<float3> buffer, int width) : IComputeShader<float4>
{
    public float4 Execute()
    {
        var index = ThreadIds.X + (ThreadIds.Y * width);
        var sourceColor = buffer[index];
        return new float4(sourceColor, 1f);
    }
}