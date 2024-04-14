using System.Collections.Generic;
using System.Numerics;

namespace Data
{
    internal class BallsCollection
    {
        private List<IBall> balls;

        public BallsCollection()
        {
            balls = new List<IBall>();
        }

        public IBall CreateBall(Vector2 position, Vector2 velocity, int radius)
        {
            int ballID = balls.Count;
            Ball ball = new Ball(position, velocity, radius, ballID);
            balls.Add(ball);
            return ball;
        }

        public List<IBall> GetAllBalls()
        {
            return balls;
        }

        public void Clear()
        {
            balls.Clear();
        }

        public IBall GetBall(int ballId)
        {
            return balls[ballId];
        }

        public int GetBallsCount()
        {
            return balls.Count;
        }
    }
}