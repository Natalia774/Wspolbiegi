using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading;

namespace Data
{
    public interface IBall
    {
        Vector2 Position { get; }
        Vector2 Velocity { get; set; }
        float Mass { get; }
        void StartMoving();
        void StopMoving();
    }


    internal class Ball : IBall
    {
        private Vector2 position;
        private Vector2 velocity;
        private static int r = 15;
        Random random = new Random();
        private float mass { get; set; }
        private static readonly int MILISECONDS_PER_STEP = 2;
        private const float FIXED_STEP_SIZE = 0.6f;
        private bool isMoving = true;

        internal Ball(Vector2 position, Vector2 velocity)
        {
            this.velocity = velocity;
            this.position = position;
            this.mass = random.Next(500);
        }

        public static int GetBallRadius()
        {
            return r;
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        public float Mass
        {
            get
            {
                return mass;
            }
        }

        public Vector2 Velocity
        {
            get
            {
                return velocity;

            }

            set
            {

                velocity = value;

            }
        }

        public void UpdatePosition(Vector2 newPosition)
        {
            position = newPosition;
        }

        // Metoda do synchronizowanego aktualizowania prędkości kulki
        public void UpdateVelocity(Vector2 newVelocity)
        {
            velocity = newVelocity;
        }

        public void StartMoving()
        {
            while (isMoving)
            {
                Stopwatch watch = Stopwatch.StartNew();

                Vector2 newPosition = position + Vector2.Normalize(Velocity) * FIXED_STEP_SIZE;
                Vector2 interpolatedPosition = Vector2.Lerp(position, newPosition, FIXED_STEP_SIZE);

                UpdatePosition(interpolatedPosition);

                watch.Stop();

                int delay = MILISECONDS_PER_STEP - (int)watch.ElapsedMilliseconds;
                if (delay > 0)
                    Thread.Sleep(delay);
            }
        }

        public void StopMoving()
        {
            isMoving = false;
        }
    }
}
