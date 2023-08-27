// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Internal;

/// <summary>
/// Provides a <see cref="SynchronizationContext"/> using <see cref="SpectreConsoleMessageQueue{T}"/>.
/// </summary>
internal class SpectreConsoleSynchronizationContext : SynchronizationContext, IDisposable
{
    private readonly SpectreConsoleMessageQueue<Message> _messageQueue = new();
    private bool _disposedValue;

    /// <summary>
    /// Sends a message and does not wait.
    /// </summary>
    /// <param name="d">The delegate to execute.</param>
    /// <param name="state">The state associated with the message.</param>
    public override void Post(SendOrPostCallback d, object? state) => _messageQueue.Post(new Message(d, state));

    /// <summary>
    /// Sends a message and waits for completion.
    /// </summary>
    /// <param name="d">The delegate to execute.</param>
    /// <param name="state">The state associated with the message.</param>
    public override void Send(SendOrPostCallback d, object? state)
    {
        var ev = new ManualResetEventSlim(false);
        try
        {
            _messageQueue.Post(new Message(d, state, ev));
            ev.Wait();
        }
        finally
        {
            ev.Dispose();
        }
    }

    /// <summary>
    /// Starts the message loop.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
    [SuppressMessage("Roslynator", "RCS1231:Make parameter ref read-only.", Justification = "Netstandard does not support")]
    public void Start(CancellationToken cancellationToken)
    {
        Message msg;
        do
        {
            // blocks until a message comes in:
            msg = _messageQueue.Receive(cancellationToken);

            // execute the code on this thread
            msg.Callback?.Invoke(msg.State);

            // let Send() know we're done:
            msg.FinishedEvent?.Set();

            // exit on the quit message
        }
        while (msg.Callback != null && !cancellationToken.IsCancellationRequested);
        cancellationToken.ThrowIfCancellationRequested();
    }

    /// <summary>
    /// Starts the message loop.
    /// </summary>
    public void Start()
    {
        Message msg;
        do
        {
            // blocks until a message comes in:
            msg = _messageQueue.Receive();

            // execute the code on this thread
            msg.Callback?.Invoke(msg.State);

            // let Send() know we're done:
            msg.FinishedEvent?.Set();

            // exit on the quit message
        }
        while (msg.Callback != null);
    }

    /// <summary>
    /// Stops the message loop.
    /// </summary>
    public void Stop()
    {
        var ev = new ManualResetEventSlim(false);
        try
        {
            // post the quit message
            _messageQueue.Post(new Message(null, null, ev));
            ev.Wait();
        }
        finally
        {
            ev.Dispose();
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                Stop();
                _messageQueue.Dispose();
            }

            _disposedValue = true;
        }
    }

    /// <summary>
    /// Message.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Message"/> struct.
    /// </remarks>
    private readonly struct Message(SendOrPostCallback? callback, object? state, ManualResetEventSlim? finishedEvent)
    {
        public readonly SendOrPostCallback? Callback = callback;
        public readonly object? State = state;
        public readonly ManualResetEventSlim? FinishedEvent = finishedEvent;

        public Message(SendOrPostCallback callback, object? state)
            : this(callback, state, null)
        {
        }
    }
}
