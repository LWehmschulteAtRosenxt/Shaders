namespace Compute;

using ComputeSharp;

public static partial class TestShaderHelper
{
    #region Methods

    public static void FullyForwardTestShader(
        GraphicsDevice device,
        int c,
        int n,
        int m,
        int b,
        ReadOnlyBuffer<float> xb,
        ReadOnlyBuffer<float> yb,
        ReadOnlyBuffer<float> zb,
        ReadWriteBuffer<float> outb)
    {
        device.For(c, n, b, new TestShader(n, m, b, xb, yb, zb, outb));
    }

    #endregion

    [ThreadGroupSize(DefaultThreadGroupSizes.XYZ)]
    [GeneratedComputeShaderDescriptor]
    public readonly partial struct TestShader(
        int x,
        int y,
        int z,
        ReadOnlyBuffer<float> xb,
        ReadOnlyBuffer<float> yb,
        ReadOnlyBuffer<float> zb,
        ReadWriteBuffer<float> outb) : IComputeShader
    {
        #region Methods

        /// <inheritdoc/>
        public void Execute()
        {
            int x_offset = (ThreadIds.X * x * y) + (ThreadIds.Y * y);
            float result = 0f;

            for (int k = 0; k < y; k++)
            {
                result += xb[x_offset + k] * zb[(k * z) + ThreadIds.Z];
            }

            var index = (ThreadIds.X * x * z) + (ThreadIds.Y * z) + ThreadIds.Z;
            outb[index] = result + zb[ThreadIds.Z];
        }

        #endregion
    }
}