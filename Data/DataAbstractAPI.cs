using System.Numerics;

namespace Data
{
    public abstract class DataAbstractAPI
    {
        public abstract int GetBoardWidth();
        public abstract int GetBoardHeight();
        public abstract IBall CreateBall(Vector2 position, Vector2 velocity, int radius);
        public abstract IBall GetBall(int index);
        public abstract int GetBallsCount();
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

        public override int GetBoardHeight()
        {
            return Board.height;
        }

        public DataAPI()
        {
            balls = new BallsCollection();
        }

        public override IBall CreateBall(Vector2 position, Vector2 velocity, int radius)
        {
            return balls.CreateBall(position, velocity, radius);
        }

        public override int GetBallsCount()
        {
            return balls.GetBallsCount();
        }

        public override IBall GetBall(int index)
        {
            return balls.GetBall(index);
        }
    }
}
