namespace Compute;

using ComputeSharp;

public static partial class GpuShaderChain
{
    public static void AddThenMultiply(
        GraphicsDevice device,
        ReadOnlyBuffer<float> input,
        ReadWriteBuffer<float> output,
        float addend,
        float multiplier)
    {
        if (input.Length != output.Length)
        {
            throw new ArgumentException("Input and output buffers must have the same length.");
        }

        if (input.Length == 0)
        {
            return;
        }

        using ReadWriteBuffer<float> intermediate = device.AllocateReadWriteBuffer<float>(input.Length);

        device.For(input.Length, new AddStageShader(input.Length, addend, input, intermediate));
        device.For(input.Length, new MultiplyStageShader(input.Length, multiplier, intermediate, output));
    }

    [ThreadGroupSize(DefaultThreadGroupSizes.X)]
    [GeneratedComputeShaderDescriptor]
    public readonly partial struct AddStageShader(
        int length,
        float addend,
        ReadOnlyBuffer<float> input,
        ReadWriteBuffer<float> intermediate) : IComputeShader
    {
        public void Execute()
        {
            int index = ThreadIds.X;

            if (index < length)
            {
                intermediate[index] = input[index] + addend;
            }
        }
    }

    [ThreadGroupSize(DefaultThreadGroupSizes.X)]
    [GeneratedComputeShaderDescriptor]
    public readonly partial struct MultiplyStageShader(
        int length,
        float multiplier,
        ReadWriteBuffer<float> intermediate,
        ReadWriteBuffer<float> output) : IComputeShader
    {
        public void Execute()
        {
            int index = ThreadIds.X;

            if (index < length)
            {
                output[index] = intermediate[index] * multiplier;
            }
        }
    }
}
