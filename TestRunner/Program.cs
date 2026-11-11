using System.Buffers;
using ComputeSharp;

Console.WriteLine("Hello, World!");

// float[,] m = new float[1_000_000, 99_999];

var n = 20_000;
var m = 1;  //9_999;
var rng = new Random();

var arrx = CreateRandom();
var arry = CreateRandom();
var arrz = CreateRandom();

var device = GraphicsDevice.EnumerateDevices().MaxBy(d => d.DedicatedMemorySize);
using ReadOnlyBuffer<float> xb = device.AllocateReadOnlyBuffer<float>(arrx);
using ReadOnlyBuffer<float> yb = device.AllocateReadOnlyBuffer<float>(arry);
using ReadOnlyBuffer<float> zb = device.AllocateReadOnlyBuffer<float>(arrz);
using ReadWriteBuffer<float> ob = device.AllocateReadWriteBuffer<float>(arrx.Length);


Compute.TestShaderHelper.FullyForwardTestShader(device, 512, n, m, 1024, xb, yb, zb, ob);
// Compute.GpuShaderChain.AddThenMultiply(device, xb, ob, 0.4f, 12.7f);


float[] CreateRandom()
{
    float[] arr = ArrayPool<float>.Shared.Rent(n * m);
    foreach (ref float x in arr.AsSpan())
    {
        x = (float)rng.NextDouble();
    }
    return arr;
}
