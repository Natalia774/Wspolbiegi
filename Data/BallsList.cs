using System.Collections.Generic;
using System.Numerics;

namespace Data
{
    internal class BallsList
    {
        private List<IBall> balls;

        public BallsList()
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
        

        public IBall GetBall(int ID)
        {
            return balls[ID];
        }

        public int GetBallsCount()
        {
            return balls.Count;
        }
    }
}