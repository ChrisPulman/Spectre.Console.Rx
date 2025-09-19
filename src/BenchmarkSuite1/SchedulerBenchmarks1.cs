// Copyright (c) Benchmarks for Spectre.Console.Rx
// Licensed under the MIT license.
using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using BenchmarkDotNet.Attributes;
using Microsoft.VSDiagnostics;
using Spectre.Console.Rx;

namespace Spectre.Console.Rx.Benchmarks
{
    [CPUUsageDiagnoser]
    public class SpectreConsoleSchedulerBenchmarks
    {
        private readonly IScheduler _scheduler = SpectreConsoleScheduler.Instance;
        [GlobalSetup]
        public void GlobalSetup()
        {
            if (_scheduler is ISpectreConsoleScheduler s && !s.IsRunning)
            {
                s.Start();
            }
        }

        [Benchmark]
        public IDisposable Schedule_NoOp()
        {
            return _scheduler.Schedule(0, static (sch, state) => Disposable.Empty);
        }

        [Benchmark]
        public void Schedule_1000_NoOps()
        {
            using var d = new CompositeDisposable();
            for (var i = 0; i < 1000; i++)
            {
                d.Add(_scheduler.Schedule(i, static (sch, state) => Disposable.Empty));
            }
        }

        [Benchmark]
        public void Schedule_With_DueTime_1ms_100_NoOps()
        {
            using var d = new CompositeDisposable();
            var due = TimeSpan.FromMilliseconds(1);
            for (var i = 0; i < 100; i++)
            {
                d.Add(_scheduler.Schedule(i, due, static (sch, state) => Disposable.Empty));
            }
        }
    }
}