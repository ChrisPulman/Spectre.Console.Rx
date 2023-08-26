// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal sealed class AnsiConsoleFacade : IAnsiConsole
{
    private readonly object _renderLock;
    private readonly AnsiConsoleBackend _ansiBackend;
    private readonly LegacyConsoleBackend _legacyBackend;
    private bool _disposedValue;

    public AnsiConsoleFacade(Profile profile, IExclusivityMode exclusivityMode)
    {
        _renderLock = new object();

        Profile = profile ?? throw new ArgumentNullException(nameof(profile));
        Input = new DefaultInput(Profile);
        ExclusivityMode = exclusivityMode ?? throw new ArgumentNullException(nameof(exclusivityMode));
        Pipeline = new RenderPipeline();

        _ansiBackend = new AnsiConsoleBackend(this);
        _legacyBackend = new LegacyConsoleBackend(this);
    }

    public Profile Profile { get; }

    public IAnsiConsoleCursor Cursor => GetBackend().Cursor;

    public IAnsiConsoleInput Input { get; }

    public IExclusivityMode ExclusivityMode { get; }

    public RenderPipeline Pipeline { get; }

    public void Clear(bool home)
    {
        lock (_renderLock)
        {
            GetBackend().Clear(home);
        }
    }

    public void Write(IRenderable renderable)
    {
        lock (_renderLock)
        {
            GetBackend().Write(renderable);
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private IAnsiConsoleBackend GetBackend()
    {
        if (Profile.Capabilities.Ansi)
        {
            return _ansiBackend;
        }

        return _legacyBackend;
    }

    private void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                ExclusivityMode.Dispose();
            }

            _disposedValue = true;
        }
    }
}
