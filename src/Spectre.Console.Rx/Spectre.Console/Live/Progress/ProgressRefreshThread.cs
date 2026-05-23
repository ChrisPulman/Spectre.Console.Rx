namespace Spectre.Console.Rx;

internal sealed class ProgressRefreshThread : IDisposable
{
    private readonly CancellationTokenSource _stop = new();
    private readonly ProgressContext _context;
    private readonly TimeSpan _refreshRate;
    private readonly Task _refreshTask;

    public ProgressRefreshThread(ProgressContext context, TimeSpan refreshRate)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _refreshRate = refreshRate;
        _refreshTask = Task.Run(RefreshLoopAsync);
    }

    public void Dispose()
    {
        _stop.Cancel();

        try
        {
            _refreshTask.GetAwaiter().GetResult();
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            _stop.Dispose();
        }
    }

    private async Task RefreshLoopAsync()
    {
        while (!_stop.IsCancellationRequested)
        {
            await Task.Delay(_refreshRate, _stop.Token).ConfigureAwait(false);

            if (!_stop.IsCancellationRequested)
            {
                _context.Refresh();
            }
        }
    }
}
