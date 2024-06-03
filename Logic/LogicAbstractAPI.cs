using Data;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Logic
{
    public interface ICollisionHandler
    {
        event EventHandler<CollisionEventArgs> CollisionDetected;

        void HandleCollision(IBall ball1, IBall ball2);
    }

    public class CollisionEventArgs : EventArgs
    {
        public IBall Ball1 { get; }
        public IBall Ball2 { get; }

        public CollisionEventArgs(IBall ball1, IBall ball2)
        {
            Ball1 = ball1;
            Ball2 = ball2;
        }
    }
    public abstract class LogicAbstractAPI : IObserver<IBall>, IObservable<int>
    {
        public abstract void Start();
        public abstract void Stop();
        public abstract int GetBoardWidth();
        public abstract int GetBoardHeight();
        public abstract Vector2 GetBallPosition(int index);
        public abstract int GetBallRadius();
        public abstract void CreateBalls(int nrOfBalls);
        public abstract IBall GetBall(int index);
        public abstract Task DetectAndHandleCollisions();
        public abstract void ClearBalls();
        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(IBall ball);
        public abstract IDisposable Subscribe(IObserver<int> observer);
        public static LogicAbstractAPI CreateLogicAPI()
        {
            return new LogicAPI();
        }
    }

    public class LogicAPI : LogicAbstractAPI, ICollisionHandler
    {
        private DataAbstractAPI dataAPI;
        public event EventHandler<CollisionEventArgs> CollisionDetected;
        private object lockObject = new object();
        private IObserver<int> observer = null;

        private class Unsubscriber : IDisposable
        {
            private IObserver<int> _observer;

            public Unsubscriber(IObserver<int> observer)
            {
                this._observer = observer;
            }

            public void Dispose()
            {
                _observer = null;
            }
        }

        public LogicAPI()
        {
            this.dataAPI = DataAbstractAPI.CreateAPI();
        }

        public override Vector2 GetBallPosition(int index)
        {
            return dataAPI.GetBall(index).Position;
        }

        public override IBall GetBall(int index)
        {
            return dataAPI.GetBall(index);
        }

        public override void ClearBalls()
        {
            lock (lockObject)
            {
                dataAPI.ClearBalls();
            }
        }

        public override int GetBallRadius()
        {
            return dataAPI.GetBallRadius();
        }

        public override int GetBoardWidth()
        {
            return dataAPI.GetBoardWidth();
        }

        public override int GetBoardHeight()
        {
            return dataAPI.GetBoardHeight();
        }

        public override void CreateBalls(int nrOfBalls)
        {
            Random rand = new Random();
            List<Vector2> ballPositions = dataAPI.GetBallsPositions();
            float ballRadius = dataAPI.GetBallRadius();

            for (int i = 0; i < nrOfBalls; i++)
            {
                bool overlap = true;
                Vector2 position = new Vector2(0, 0);

                //repeat until a non-overlapping position is found
                while (overlap)
                {
                    overlap = false;
                    position = new Vector2(
                        rand.Next(1 + GetBallRadius(), dataAPI.GetBoardWidth() - GetBallRadius() - 1),
                        rand.Next(1 + GetBallRadius(), dataAPI.GetBoardWidth() - GetBallRadius()) - 1);

                    //check if the new position overlaps with any existing ball
                    foreach (var existingPosition in ballPositions)
                    {
                        if (Vector2.Distance(existingPosition, position) <= ballRadius)
                        {
                            overlap = true;
                            break;
                        }
                    }

                }
                //if no overlap, add the position to the list
                lock (lockObject)
                {
                    ballPositions.Add(position);
                    IBall newBall = dataAPI.CreateBall(position, Vector2.Zero);

                    Vector2 maxVelocity = new Vector2(6.0f, 6.0f);
                    Vector2 velocity = new Vector2(
                        (float)(rand.NextDouble() * maxVelocity.X - (maxVelocity.X / 2)),
                        (float)(rand.NextDouble() * maxVelocity.Y - (maxVelocity.Y / 2))
                    );
                    newBall.Velocity = velocity;
                    _ = newBall.Subscribe(this); // subskrybujemy na powiadomienia o zmianach stanu dla newBall
                }
            }
        }

        public override IDisposable Subscribe(IObserver<int> observer)
        {
            this.observer = observer;
            return new Unsubscriber(this.observer);
        }

        public override void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public override void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public override void Start()
        {
            int numberOfBalls = dataAPI.GetBallsCount();

            for (int i = 0; i < numberOfBalls; i++)
            {
                IBall ball = dataAPI.GetBall(i);
                Thread thread = new Thread(new ThreadStart(ball.StartMoving));
                thread.Start();
            }
        }

        public override void Stop()
        {
            int numberOfBalls = dataAPI.GetBallsCount();

            for (int i = 0; i < numberOfBalls; i++)
            {
                IBall ball = dataAPI.GetBall(i);
                ball.StopMoving();
            }
        }


        public override async Task DetectAndHandleCollisions()
        {
            await Task.Run(() =>
            {
                List<Vector2> allBallPositions = dataAPI.GetBallsPositions();

                for (int i = 0; i < dataAPI.GetBallsPositions().Count; i++)
                {
                    for (int j = i + 1; j < dataAPI.GetBallsPositions().Count; j++)
                    {
                        Vector2 position1 = dataAPI.GetBallsPositions()[i];
                        Vector2 position2 = dataAPI.GetBallsPositions()[j];

                        double distance = Vector2.Distance(position1, position2);
                        double radiusSum = GetBallRadius() * 2;

                        if (distance <= radiusSum)
                        {
                            HandleCollision(dataAPI.GetBall(i), dataAPI.GetBall(j));
                        }
                    }
                }

                for (int i = 0; i < dataAPI.GetBallsPositions().Count; i++)
                {
                    Vector2 position = dataAPI.GetBallsPositions()[i];

                    if (position.X <= 0 || position.Y <= 0 ||
                        position.X + GetBallRadius() >= GetBoardWidth() ||
                        position.Y + GetBallRadius() >= GetBoardHeight())
                    {
                        HandleWallCollision(dataAPI.GetBall(i));
                    }
                }
            });
        }

        public override void OnNext(IBall ball)
        {
            lock (lockObject)
            {
                int index = dataAPI.GetBallIndex(ball);
                observer.OnNext(index);

                _ = DetectAndHandleCollisions();
            }
        }

        public void HandleCollision(IBall ball1, IBall ball2)
        {
            Vector2 normal = Vector2.Normalize(ball2.Position - ball1.Position);
            Vector2 relativeVelocity = ball2.Velocity - ball1.Velocity;

            // Calculate velocities after collision
            float m1 = dataAPI.GetBallMass();
            float m2 = dataAPI.GetBallMass();
            float v1n = Vector2.Dot(normal, ball1.Velocity);
            float v2n = Vector2.Dot(normal, ball2.Velocity);
            float v1nAfter = ((m1 - m2) * v1n + 2 * m2 * v2n) / (m1 + m2);
            float v2nAfter = ((m2 - m1) * v2n + 2 * m1 * v1n) / (m1 + m2);

            // Set new velocities
            ball1.Velocity += (v1nAfter - v1n) * normal;
            ball2.Velocity += (v2nAfter - v2n) * normal;
        }

        private void HandleWallCollision(IBall ball)
        {
            Vector2 velocity = ball.Velocity;

            int boardWidth = GetBoardWidth();
            int boardHeight = GetBoardHeight();

            Vector2 position = ball.Position;

            if (position.X <= 0 || position.X >= boardWidth)
            {
                velocity.X *= -1;
            }

            if (position.Y <= 0 || position.Y >= boardHeight)
            {
                velocity.Y *= -1;
            }

            ball.Velocity = velocity;
        }
    }
}
