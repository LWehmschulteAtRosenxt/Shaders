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
        var random = new Random();
        var array = new float[length];

        for (var i = 0; i < length; i++)
        {
            array[i] = (float)(random.NextDouble() * (maxValue - minValue) + minValue);
        }

        return array;
    }

    [Test]
    public void ReduceShader_Is_Correct()
    {
        // Arrange
        var length = 1024;
        var binSize = 4;
        var input = GenerateRandomFloatArray(length);
        var expected = new float[length / binSize];

        for (var i = 0; i < expected.Length; i++)
        {
            var sum = 0f;
            for (var j = 0; j < binSize; j++)
            {
                sum += (1f / binSize) * input[i * binSize + j];
            }

            expected[i] = sum;
        }

        using var gpuInput = device.AllocateReadWriteBuffer(input);
        using var gpuOutput = device.AllocateReadWriteBuffer<float>(expected.Length);

        // Act
        this.device.For(gpuOutput.Length, new ReduceShader(binSize, gpuInput, gpuOutput));

        // Assert
        var actual = gpuOutput.ToArray();
        Assert.That(actual.Length, Is.EqualTo(expected.Length));
        for (var i = 0; i < expected.Length; i++)
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
        var length = 1024;
        var input = GenerateRandomFloatArray(length);
        var expected = new float[length];
        for (var i = 0; i < length; i++)
        {
            expected[i] = MathF.Cos(input[i]);
        }

        using var gpuInput = device.AllocateReadOnlyBuffer(input);
        using var gpuOutput = device.AllocateReadWriteBuffer<float>(length);

        var context = this.device.CreateComputeContext();

        // Act
        context.ZToCos(gpuInput, gpuOutput);

        // Assert
        var actual = gpuOutput.ToArray();
        for (var i = 0; i < length; i++)
        {
            Assert.That(actual[i], Is.EqualTo(expected[i]).Within(1e-5f));
        }

        context.Dispose();
    }

    #endregion
}