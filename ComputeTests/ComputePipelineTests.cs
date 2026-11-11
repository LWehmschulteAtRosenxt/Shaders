namespace ComputeTests;

using Compute;
using ComputeSharp;

public class ComputePipelineTests : IDisposable
{
    #region Fields

    private GraphicsDevice device = GraphicsDevice.GetDefault();

    #endregion

    #region Methods

    [Test]
    public void CanAllocate()
    {
        // Arrange
        var initialX = 1_000_000;
        var initialY = 99_000;

        // Act
        var size = initialX * initialY;
        var actioalX = new float[size];
        var fits = GpuHelper.TryArrayFitIntoGpu(ref actioalX);

        // Assert
        Assert.That(fits, Is.True, $"Failed to allocate array of size {size}");
    }

    public void Dispose()
    {
        this.device.Dispose();
    }

    private float[] GenerateRandomFloatArray(int length, float minValue = -1f, float maxValue = 1f)
    {
        Random random = new Random();
        float[] array = new float[length];

        for (int i = 0; i < length; i++)
        {
            array[i] = (float)(random.NextDouble() * (maxValue - minValue) + minValue);
        }

        return array;
    }

    [Test]
    public void ReduceShader_Is_Correct()
    {
        // Arrange
        int length = 1024;
        int binSize = 4;
        float[] input = GenerateRandomFloatArray(length);
        float[] expected = new float[length / binSize];

        for (int i = 0; i < expected.Length; i++)
        {
            float sum = 0f;
            for (int j = 0; j < binSize; j++)
            {
                sum += (1f / binSize) * input[i * binSize + j];
            }

            expected[i] = sum;
        }

        using ReadOnlyBuffer<float> gpuInput = device.AllocateReadOnlyBuffer(input);
        using ReadWriteBuffer<float> gpuOutput = device.AllocateReadWriteBuffer<float>(expected.Length);

        // Act
        this.device.For(gpuOutput.Length, new ReduceShader(binSize, gpuInput, gpuOutput));

        // Assert
        float[] actual = gpuOutput.ToArray();
        Assert.That(actual.Length, Is.EqualTo(expected.Length));
        for (int i = 0; i < expected.Length; i++)
        {
            Assert.That(actual[i], Is.EqualTo(expected[i]).Within(1e-3), $"Mismatch at index {i}: expected {expected[i]}, got {actual[i]}");
            ;
        }
    }

    [SetUp]
    public void Setup()
    {
        this.device = GraphicsDevice.EnumerateDevices().MaxBy(d => d.DedicatedMemorySize)
                      ?? throw new InvalidOperationException("No suitable graphics device found.");
    }

    [Test]
    public void ZToCosShader_Is_Correct()
    {
        // Arrange
        int length = 1024;
        float[] input = GenerateRandomFloatArray(length);
        float[] expected = new float[length];
        for (int i = 0; i < length; i++)
        {
            expected[i] = MathF.Cos(input[i]);
        }

        using ReadOnlyBuffer<float> gpuInput = device.AllocateReadOnlyBuffer(input);
        using ReadWriteBuffer<float> gpuOutput = device.AllocateReadWriteBuffer<float>(length);

        // Act
        ComputePipeline.ZToCos(this.device, gpuInput, gpuOutput);

        // Assert
        float[] actual = gpuOutput.ToArray();
        for (int i = 0; i < length; i++)
        {
            Assert.That(actual[i], Is.EqualTo(expected[i]).Within(1e-5f));
        }
    }

    #endregion
}