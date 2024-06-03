using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Threading;

namespace Data
{
    public interface IBall : IObservable<IBall>
    {
        Vector2 Position { get; }
        Vector2 Velocity { get; set; }
        void StartMoving();
        void StopMoving();
    }


    internal class Ball : IBall
    {
        private readonly object PositionLock = new object();
        private readonly object VelocityLock = new object();
        private Vector2 position;
        private Vector2 velocity;
        private static int r = 15;
        internal readonly List<IObserver<IBall>> observers;

        private readonly ILogger _logger;
        private readonly int id;

        private static readonly int MILISECONDS_PER_STEP = 1;
        private const float FIXED_STEP_SIZE = 0.6f;
        private bool isMoving = true;

        internal Ball(int id, Vector2 position, Vector2 velocity, ILogger logger)
        {
            observers = new List<IObserver<IBall>>();
            this.id = id;
            this.velocity = velocity;
            this.position = position;
            this._logger = logger;
        }

        private void Log(string message)
        {
            _logger?.Log($"Ball {id}: {message}");
        }

        public static int GetBallRadius()
        {
            return r;
        }

        public Vector2 Position
        {
            get
            {
                lock (PositionLock)
                {
                    return position;
                }
            }
        }

        public Vector2 Velocity
        {
            get
            {
                lock (VelocityLock)
                {
                    return velocity;
                }
            }

            set
            {
                lock (VelocityLock)
                {
                    velocity = value;
                }
            }
        }

        public void StartMoving()
        {
            while (isMoving)
            {
                Stopwatch watch = Stopwatch.StartNew();

                Vector2 newPosition = position + Vector2.Normalize(Velocity) * FIXED_STEP_SIZE;
                Vector2 interpolatedPosition = Vector2.Lerp(position, newPosition, FIXED_STEP_SIZE);

                lock (PositionLock)
                {
                    position = interpolatedPosition;
                }

                Log($"Ball moved to position: {interpolatedPosition.X}, {interpolatedPosition.Y}");

                foreach (IObserver<Ball> observer in observers)
                {
                    observer.OnNext(this);
                }

                watch.Stop();

                int delay = MILISECONDS_PER_STEP - (int)watch.ElapsedMilliseconds;
                if (delay > 0)
                    Thread.Sleep(delay);
            }
        }

        public void StopMoving()
        {
            isMoving = false;
            Log("Ball stopped moving");
        }

        public IDisposable Subscribe(IObserver<IBall> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<IBall>> _observers;
            private IObserver<IBall> _observer;

            public Unsubscriber(List<IObserver<IBall>> observers, IObserver<IBall> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }
}
