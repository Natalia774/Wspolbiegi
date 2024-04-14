using Data;
using System;
using System.Numerics;

namespace Logic
{
    public abstract class LogicAbstractAPI
    {
        public abstract void Start(int nrOfBalls);
        public abstract DataAbstractAPI getDataAPI();
        public static LogicAbstractAPI CreateLogicAPI()
        {
            return new LogicAPI();
        }
    }

    public class LogicAPI : LogicAbstractAPI
    {
        private DataAbstractAPI dataAPI;

        public LogicAPI()
        {
            this.dataAPI = DataAbstractAPI.CreateAPI();
        }

        public override DataAbstractAPI getDataAPI()
        {
            return this.dataAPI;
        }

        public override void Start(int nrOfBalls)
        {
            Vector2 maxPosition = new Vector2(dataAPI.GetBoardWidth(), dataAPI.GetBoardHeight());
            Vector2 maxVelocity = new Vector2(2.0f, 2.0f);
            int radius = 2;

            Random Rand = new Random();

            for (int i = 0; i < nrOfBalls; i++)
            {
                IBall ball = dataAPI.CreateBall(
                    new Vector2(
                        ((float)Rand.NextDouble() * (maxPosition.X - 8.0F - float.Epsilon)) + (4.0F + float.Epsilon),
                        ((float)Rand.NextDouble() * (maxPosition.Y - 8.0F - float.Epsilon)) + (4.0F + float.Epsilon)
                    ),
                    maxVelocity, radius);
            }
        }


    }
}