// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Concurrent;

namespace Spectre.Console.Rx.Internal;

internal sealed class SpectreConsoleMessageQueue<T> : IDisposable
{
    private readonly ConcurrentQueue<T> _queue = new();
    private readonly SemaphoreSlim _sync = new(0);
    private bool _disposedValue;

    /// <summary>
    /// Gets a value indicating whether indicates whether or not the queue is empty.
    /// </summary>
    public bool IsEmpty => _queue.IsEmpty;

    /// <summary>
    /// Gets indicates the count of messages in the queue.
    /// </summary>
    public int Count => _queue.Count;

    /// <summary>
    /// Sends a message.
    /// </summary>
    /// <param name="message">The message to send.</param>
    public void Post(T message)
    {
        _queue.Enqueue(message);
        _sync.Release(1);
    }

    /// <summary>
    /// Receives the next message from the queue.
    /// </summary>
    /// <returns>The received message.</returns>
    public T Receive()
    {
        _sync.Wait();
        if (!_queue.TryDequeue(out var result))
        {
            throw new InvalidOperationException("The queue is empty");
        }

        return result;
    }

    /// <summary>
    /// Receives the next message from the queue.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The received message.</returns>
    /// <exception cref="OperationCanceledException">The operation was cancelled.</exception>
    public T Receive(CancellationToken cancellationToken)
    {
        _sync.Wait(cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        if (!_queue.TryDequeue(out var result))
        {
            throw new InvalidOperationException("The queue is empty");
        }

        cancellationToken.ThrowIfCancellationRequested();
        return result;
    }

    /// <summary>
    /// Asynchronously receives a message.
    /// </summary>
    /// <returns>A <see cref="Task{T}"/> containing the message.</returns>
    public async Task<T> ReceiveAsync()
    {
        await _sync.WaitAsync();
        if (!_queue.TryDequeue(out var result))
        {
            throw new InvalidOperationException("The queue is empty");
        }

        return result;
    }

    /// <summary>
    /// Asynchronously receives a message.
    /// </summary>
    /// <param name="cancellationToken">A cancelation token.</param>
    /// <returns>A <see cref="Task{T}"/> containing the message.</returns>
    /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
    public async Task<T> ReceiveAsync(CancellationToken cancellationToken)
    {
        await _sync.WaitAsync(cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        if (!_queue.TryDequeue(out var result))
        {
            throw new InvalidOperationException("The queue is empty");
        }

        cancellationToken.ThrowIfCancellationRequested();
        return result;
    }

    /// <summary>
    /// Polls for a message.
    /// </summary>
    /// <param name="result">The message to receive.</param>
    /// <returns>True if a message is available, otherwise false. If the result is false <paramref name="result"/> is undefined.</returns>
    /// <remarks>This function never waits.</remarks>
    public bool Poll(out T result)
        => _queue.TryDequeue(out result!);

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _sync.Dispose();
            }

            _disposedValue = true;
        }
    }
}
