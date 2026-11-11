namespace Shaders;

using ComputeSharp;

public class DisplayShader(ReadWriteBuffer<float3> buffer) : IComputeShader<float4>
{
    public float4 Execute()
    {
        return new float4();
    }
}