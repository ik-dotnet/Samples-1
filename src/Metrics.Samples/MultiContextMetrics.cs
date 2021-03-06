﻿using App.Metrics;
using App.Metrics.Counter.Abstractions;
using App.Metrics.Meter.Abstractions;
using App.Metrics.Timer.Abstractions;

namespace Metrics.Samples
{
    public class MultiContextMetrics
    {
        private readonly ICounter _firstCounter;
        private readonly ICounter _secondCounter;
        private readonly IMeter _secondMeter;

        public MultiContextMetrics(IMetrics metrics)
        {
            _firstCounter = metrics.Provider.Counter.Instance(SampleMetricsRegistry.Contexts.FirstContext.Counters.Counter);
            _secondCounter = metrics.Provider.Counter.Instance(
                SampleMetricsRegistry.Contexts.SecondContext.Counters.Counter);
            _secondMeter = metrics.Provider.Meter.Instance(SampleMetricsRegistry.Contexts.SecondContext.Meters.Requests);
        }

        public void Run()
        {
            _firstCounter.Increment();
            _secondCounter.Increment();
            _secondMeter.Mark();
        }
    }

    public class MultiContextInstanceMetrics
    {
        private readonly ICounter _instanceCounter;
        private readonly ITimer _instanceTimer;
        private static IMetrics _metrics;

        public MultiContextInstanceMetrics(string instanceName, IMetrics metrics)
        {
            _metrics = metrics;
            _instanceCounter = _metrics.Provider.Counter.Instance(SampleMetricsRegistry.Counters.SampleCounter);
            _instanceTimer = _metrics.Provider.Timer.Instance(SampleMetricsRegistry.Timers.SampleTimer);
        }

        public void Run()
        {
            using (_instanceTimer.NewContext())
            {
                _instanceCounter.Increment();
            }
        }

        public void RunSample()
        {
            for (var i = 0; i < 5; i++)
            {
                new MultiContextInstanceMetrics("Sample Instance " + i, _metrics).Run();
            }
        }
    }
}