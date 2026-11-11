namespace Shaders.Platforms.Windows;

using ComputeSharp.WinUI;

internal sealed class FrameRequestQueue : IFrameRequestQueue
{
    private readonly SemaphoreSlim pendingFrame = new(0, 1);

    public void RequestFrame()
    {
        try
        {
            this.pendingFrame.Release();
        }
        catch (SemaphoreFullException)
        {
            // A pending request already covers the latest state.
        }
    }

    public void Dequeue(out object? parameter, CancellationToken token)
    {
        this.pendingFrame.Wait(token);
        parameter = null;
    }
}