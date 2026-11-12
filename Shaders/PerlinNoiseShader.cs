namespace Shaders;

using ComputeSharp;

[ThreadGroupSize(DefaultThreadGroupSizes.XY)]
[GeneratedComputeShaderDescriptor]
public readonly partial struct PerlinNoiseShader(int distance, ReadWriteBuffer<float> input, ReadWriteBuffer<float> output) : IComputeShader
{
    #region Methods

    public void Execute()
    {
        var neighborTotal = 0f;
        var neighborCount = 0;

        for (var i = -distance; i <= distance; i++)
        {
            for (var j = -distance; j <= distance; j++)
            {
                var neighborX = ThreadIds.X + i;
                var neighborY = ThreadIds.Y + j;
                var neighborIndex = neighborX + (neighborY * DispatchSize.X);
                if (neighborX < 0 || neighborX >= DispatchSize.X || neighborY < 0 || neighborY >= DispatchSize.Y)
                {
                    continue;
                }

                neighborTotal += input[neighborIndex];
                neighborCount++;
            }
        }

        var average = neighborTotal / neighborCount;
        var cellIndex = ThreadIds.X + (ThreadIds.Y * DispatchSize.X);
        output[cellIndex] = average;
    }

    #endregion
}