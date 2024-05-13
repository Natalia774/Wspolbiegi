using System.Collections.Generic;
using System.Numerics;

namespace Data
{
    public abstract class DataAbstractAPI
    {
        public abstract int GetBoardWidth();
        public abstract int GetBoardHeight();
        public abstract IBall CreateBall(Vector2 position, Vector2 velocity);
        public abstract IBall GetBall(int index);
        public abstract int GetBallsCount();
        public abstract int GetBallRadius();
        public abstract List<Vector2> GetBallsPositions();
        public abstract List<IBall> GetBalls();
        public abstract void ClearBalls();
        public static DataAbstractAPI CreateAPI()
        {
            return new DataAPI();
        }
    }

    internal class DataAPI : DataAbstractAPI
    {
        private readonly BallsCollection balls;
        public override int GetBoardWidth()
        {
            return Board.width;
        }

        public override List<Vector2> GetBallsPositions()
        {
            return balls.GetBallsPositions();
        }

        public override List<IBall> GetBalls()
        {
            return balls.GetBalls();
        }

        public override int GetBoardHeight()
        {
            return Board.height;
        }

        public DataAPI()
        {
            balls = new BallsCollection();
        }

        public override IBall CreateBall(Vector2 position, Vector2 velocity)
        {
            return balls.CreateBall(position, velocity);
        }

        public override int GetBallsCount()
        {
            return balls.GetBallsCount();
        }

        public override void ClearBalls()
        {
            balls.Clear();
        }

        public override IBall GetBall(int index)
        {
            return balls.GetBall(index);
        }

        public override int GetBallRadius()
        {
            return Ball.GetBallRadius();
        }
    }
}
