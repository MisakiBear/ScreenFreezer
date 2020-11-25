using System;
using System.Threading;
using System.Threading.Tasks;

namespace ScreenFreezer
{
    public class Trigger : IDisposable
    {
        public bool IsAlive { get; private set; } = false;
        public bool IsPulled { get; private set; } = false;
        public event Action<bool> Result;
        private readonly CancellationTokenSource _source = new CancellationTokenSource();
        private readonly int _interval;
        private bool _disposed = false;

        public Trigger(int interval)
            => _interval = interval;

        private async Task CountDown()
        {
            await Task.Delay(_interval, _source.Token)
                .ContinueWith(t =>
                {
                    Result.Invoke(!t.IsCanceled);
                });

            IsAlive = false;
        }

        internal void Pull()
        {
            if (IsPulled) return;

            IsAlive = true;
            Task.Run(() => CountDown());
            IsPulled = true;
        }

        ~Trigger() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
                _source.Cancel();

            _disposed = true;
        }
    }
}
