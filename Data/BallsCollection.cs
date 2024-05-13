using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Data
{
    internal class BallsCollection
    {
        private List<IBall> balls;

        public BallsCollection()
        {
            balls = new List<IBall>();
        }

        public List<Vector2> GetBallsPositions()
        {
            List<Vector2> positions = new List<Vector2>();
            for (int i = 0; i < balls.Count; i++)
            {
                positions.Add(balls[i].Position);
            }
            return positions;
        }

        public IBall CreateBall(Vector2 position, Vector2 velocity)
        {
            Ball ball = new Ball(position, velocity);
            balls.Add(ball);
            return ball;
        }

        public void Clear()
        {
            balls.Clear();
        }

        public IBall GetBall(int ballId)
        {
            return balls[ballId];
        }

        public List<IBall> GetBalls()
        {
            return balls;
        }

        public int GetBallsCount()
        {
            return balls.Count;
        }
    }
}