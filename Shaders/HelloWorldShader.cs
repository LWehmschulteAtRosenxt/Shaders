namespace Shaders;

using ComputeSharp;

[ThreadGroupSize(DefaultThreadGroupSizes.XY)]
[GeneratedComputeShaderDescriptor]
public readonly partial struct HelloWorldShader(float time) : IComputeShader<float4>
{
    #region Methods

    public float4 Execute()
    {
        var uv = ThreadIds.Normalized.XY;
        var color = .5f + .5f * Hlsl.Cos(time + new float3(uv, uv.X) + new float3(0, 2, 4));
        return new float4(color, 1f);
    }

    #endregion
}