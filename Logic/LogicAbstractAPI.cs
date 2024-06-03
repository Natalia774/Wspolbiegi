using Data;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace Logic
{
    public abstract class LogicAbstractAPI
    {
        public abstract void Start();
        public abstract void Stop();
        public abstract int GetBoardWidth();
        public abstract int GetBoardHeight();
        public abstract Vector2 GetBallPosition(int index);
        public abstract int GetBallRadius();
        public abstract void CreateBalls(int nrOfBalls);
        public abstract IBall GetBall(int index);
        public abstract void DetectAndHandleCollisions();
        public abstract void ClearBalls();
        public static LogicAbstractAPI CreateLogicAPI()
        {
            return new LogicAPI();
        }
    }

    public class LogicAPI : LogicAbstractAPI
    {
        private DataAbstractAPI dataAPI;
        private object lockObject = new object();

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
                Vector2 position;

                // Repeat until a non-overlapping position is found
                while (overlap)
                {
                    overlap = false;
                    position = new Vector2(
                        rand.Next(GetBallRadius(), dataAPI.GetBoardWidth() - GetBallRadius()),
                        rand.Next(GetBallRadius(), dataAPI.GetBoardWidth() - GetBallRadius()));

                    // Check if the new position overlaps with any existing ball
                    foreach (var existingPosition in ballPositions)
                    {
                        if (Vector2.Distance(existingPosition, position) < ballRadius)
                        {
                            overlap = true;
                            break;
                        }
                    }

                    // If no overlap, add the position to the list
                    if (!overlap)
                    {
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
                        }
                    }
                }
            }
        }

        public override void Start()
        {
            int numberOfBalls = dataAPI.GetBallsCount();

            for (int i = 0; i < numberOfBalls; i++)
            {
                IBall ball = dataAPI.GetBall(i);
                Thread thread = new Thread(new ThreadStart(ball.StartMoving));
                thread.Name = "ball no" + i;
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

        public override void DetectAndHandleCollisions()
        {
            List<Vector2> allBallPositions = dataAPI.GetBallsPositions();

            for (int i = 0; i < allBallPositions.Count; i++)
            {
                for (int j = i + 1; j < allBallPositions.Count; j++)
                {
                    Vector2 position1 = allBallPositions[i];
                    Vector2 position2 = allBallPositions[j];

                    double distance = Vector2.Distance(position1, position2);
                    double radiusSum = GetBallRadius() * 2;

                    if (distance < radiusSum)
                    {
                        HandleBallCollision(dataAPI.GetBall(i), dataAPI.GetBall(j));
                    }
                }
            }

            for (int i = 0; i < allBallPositions.Count; i++)
            {
                Vector2 position = allBallPositions[i];

                if (position.X < 0 || position.Y < 0 ||
                    position.X + GetBallRadius() * 2 >= GetBoardWidth() ||
                    position.Y + GetBallRadius() * 2 >= GetBoardHeight())
                {
                    HandleWallCollision(dataAPI.GetBall(i));
                    Console.WriteLine("Collision detected");
                }
            }
        }

        private void HandleBallCollision(IBall ball1, IBall ball2)
        {
            Vector2 normal = Vector2.Normalize(ball2.Position - ball1.Position);
            Vector2 relativeVelocity = ball2.Velocity - ball1.Velocity;

            // Calculate velocities after collision
            float m1 = ball1.Mass;
            float m2 = ball2.Mass;
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
            ball.Velocity *= -1;
        }
    }
}
