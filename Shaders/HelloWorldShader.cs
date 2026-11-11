namespace Shaders;

using ComputeSharp;

[ThreadGroupSize(DefaultThreadGroupSizes.XY)]
[GeneratedComputeShaderDescriptor]
public readonly partial struct HelloWorldShader(
    IReadOnlyNormalizedTexture2D<float4> source,
    float time) : IComputeShader<float4>
{
    #region Methods

    public float4 Execute()
    {


        var uv = ThreadIds.Normalized.XY;
        var sourceColor = source.Sample(uv);

        var angle = time * 0.5f;
        var cosine = Hlsl.Cos(angle);
        var sine = Hlsl.Sin(angle);

        var rgb = sourceColor.RGB;

        float3 rotated = new(
            rgb.R * (0.213f + cosine * 0.787f - sine * 0.213f) +
            rgb.G * (0.715f - cosine * 0.715f - sine * 0.715f) +
            rgb.B * (0.072f - cosine * 0.072f + sine * 0.928f),

            rgb.R * (0.213f - cosine * 0.213f + sine * 0.143f) +
            rgb.G * (0.715f + cosine * 0.285f + sine * 0.140f) +
            rgb.B * (0.072f - cosine * 0.072f - sine * 0.283f),

            rgb.R * (0.213f - cosine * 0.213f - sine * 0.787f) +
            rgb.G * (0.715f - cosine * 0.715f + sine * 0.715f) +
            rgb.B * (0.072f + cosine * 0.928f - sine * 0.072f));

        return new float4(Hlsl.Saturate(rotated), sourceColor.A);
    }

    #endregion
}