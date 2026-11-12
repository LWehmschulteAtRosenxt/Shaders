namespace Shaders.Platforms.Windows;

using ComputeSharp.WinUI;

internal sealed class FrameRequestQueue : IFrameRequestQueue
{
    #region Fields

    private readonly SemaphoreSlim pendingFrame = new(0, 1);

    private readonly Lock requestLock = new();

    #endregion

    #region Methods

    public void Dequeue(out object? parameter, CancellationToken token)
    {
        this.pendingFrame.Wait(token);
        parameter = null;
    }

    public void RequestFrame()
    {
        lock (this.requestLock)
        {
            if (this.pendingFrame.CurrentCount == 0)
            {
                this.pendingFrame.Release();
            }
        }
    }

    #endregion
}