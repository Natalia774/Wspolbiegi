using Data;
using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;

namespace LogicTest
{
    [TestClass]
    public class LogicAPITest
    {
        [TestMethod]
        public void TestCreateBalls()
        {
            var dataAPIMock = new Mock<DataAbstractAPI>();

            var logicAPI = new LogicAPI();

            dataAPIMock.Setup(m => m.GetBallRadius()).Returns(5);
            dataAPIMock.Setup(m => m.GetBoardWidth()).Returns(800);
            dataAPIMock.Setup(m => m.GetBoardHeight()).Returns(600);

            //set the mock behavior for GetBallsPositions to return an empty list
            dataAPIMock.Setup(m => m.GetBallsPositions()).Returns(new List<Vector2>());

            //set the mock behavior for CreateBall to return a new IBall object
            dataAPIMock.Setup(m => m.CreateBall(It.IsAny<Vector2>(), It.IsAny<Vector2>())).Returns(new Mock<IBall>().Object);

            //set the dataAPI in logicAPI to mock DataAbstractAPI
            var dataAPIField = logicAPI.GetType().GetField("dataAPI", BindingFlags.NonPublic | BindingFlags.Instance);
            dataAPIField.SetValue(logicAPI, dataAPIMock.Object);

            logicAPI.CreateBalls(3);

            dataAPIMock.Verify(m => m.CreateBall(It.IsAny<Vector2>(), It.IsAny<Vector2>()), Times.Exactly(3));
        }

        [TestMethod]
        public void TestStart()
        {
            var dataAPIMock = new Mock<DataAbstractAPI>();

            var logicAPI = new LogicAPI();

            dataAPIMock.Setup(m => m.GetBallsCount()).Returns(2);

            var ballMock1 = new Mock<IBall>();
            var ballMock2 = new Mock<IBall>();

            ballMock1.Setup(b => b.StartMoving());
            ballMock2.Setup(b => b.StartMoving());

            dataAPIMock.SetupSequence(m => m.GetBall(It.IsAny<int>()))
                .Returns(ballMock1.Object)
                .Returns(ballMock2.Object);

            var dataAPIField = logicAPI.GetType().GetField("dataAPI", BindingFlags.NonPublic | BindingFlags.Instance);
            dataAPIField.SetValue(logicAPI, dataAPIMock.Object);

            logicAPI.Start();

            //check if the StartMoving method has been called for each ball
            ballMock1.Verify(b => b.StartMoving(), Times.Once);
            ballMock2.Verify(b => b.StartMoving(), Times.Once);
        }
    }
}
