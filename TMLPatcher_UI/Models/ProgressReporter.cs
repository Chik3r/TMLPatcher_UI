using System;
using System.Threading;

namespace TMLPatcher_UI.Models
{
    public class ProgressReporter : IDisposable, IProgress<int>
    {
        private int _currentElements;

        public virtual TimeSpan AnimationInterval => TimeSpan.FromSeconds(1.0 / 8); // Update 8 times a second 

        public int? MaxElements { get; protected set; }

        public int CurrentElements
        {
            get => _currentElements;
            protected set => _currentElements = value;
        }

        public Timer Timer { get; protected set; }

        public bool Disposed { get; protected set; }

        public Action<int> UpdateProgress;

        public ProgressReporter()
        {
            CurrentElements = 0;
            Timer = new Timer(TimerHandle);
            UpdateProgress = _ => { };
        }

        public static ProgressReporter StartNew()
        {
            ProgressReporter reporter = new ProgressReporter();
            reporter.Start();
            return reporter;
        }

        public void Report(int amount)
        {
            if (!MaxElements.HasValue)
            {
                MaxElements = amount;
                return;
            }

            Interlocked.Add(ref _currentElements, amount);
        }

        public virtual void Start() => ResetTimer();

        public virtual void Finish()
        {
            if (Disposed)
                return;
            
            Dispose();
            lock (Timer) UpdateProgress.Invoke((int) (CurrentElements / (MaxElements ?? CurrentElements) * 100f));
        }

        public void Dispose()
        {
            lock (Timer) Disposed = true;

            GC.SuppressFinalize(this);
        }

        protected virtual void TimerHandle(object state)
        {
            lock (Timer)
            {
                if (Disposed) return;

                ResetTimer();
                UpdateProgress.Invoke((int) ((float)CurrentElements / MaxElements * 100f));
            }
        }

        protected virtual void ResetTimer() => Timer.Change(AnimationInterval, TimeSpan.FromMilliseconds(-1));
    }
}