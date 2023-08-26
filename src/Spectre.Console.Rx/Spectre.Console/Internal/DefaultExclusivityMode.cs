// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Internal;

internal sealed class DefaultExclusivityMode : IExclusivityMode
{
    private SemaphoreSlim _semaphore;

    public DefaultExclusivityMode() => _semaphore = new(1, 1);

    public T Run<T>(Func<T> func)
    {
        // Try acquiring the exclusivity semaphore
        if (!_semaphore.Wait(0))
        {
            throw CreateExclusivityException();
        }

        try
        {
            return func();
        }
        finally
        {
            _semaphore.Release(1);
        }
    }

    public async Task<T> RunAsync<T>(Func<Task<T>> func)
    {
        // Try acquiring the exclusivity semaphore
        if (!await _semaphore.WaitAsync(0).ConfigureAwait(false))
        {
            throw CreateExclusivityException();
        }

        try
        {
            return await func().ConfigureAwait(false);
        }
        finally
        {
            _semaphore.Release(1);
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private static Exception CreateExclusivityException() => new InvalidOperationException(
        "Trying to run one or more interactive functions concurrently. " +
        "Operations with dynamic displays (e.g. a prompt and a progress display) " +
        "cannot be running at the same time.");

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            _semaphore.Dispose();
            _semaphore = new(1, 1);
        }
    }
}
