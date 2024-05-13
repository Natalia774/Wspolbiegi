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
            // Tworzymy mock dla interfejsu DataAbstractAPI
            var dataAPIMock = new Mock<DataAbstractAPI>();

            // Tworzymy instancję LogicAPI
            var logicAPI = new LogicAPI();

            // Ustawiamy zachowanie mocka dla GetBallRadius
            dataAPIMock.Setup(m => m.GetBallRadius()).Returns(5);

            // Ustawiamy zachowanie mocka dla GetBoardWidth i GetBoardHeight
            dataAPIMock.Setup(m => m.GetBoardWidth()).Returns(800);
            dataAPIMock.Setup(m => m.GetBoardHeight()).Returns(600);

            // Ustawiamy zachowanie mocka dla GetBallsPositions, aby zwracał pustą listę
            dataAPIMock.Setup(m => m.GetBallsPositions()).Returns(new List<Vector2>());

            // Ustawiamy zachowanie mocka dla CreateBall, aby zwracał nowy obiekt IBall
            dataAPIMock.Setup(m => m.CreateBall(It.IsAny<Vector2>(), It.IsAny<Vector2>())).Returns(new Mock<IBall>().Object);

            // Ustawiamy dataAPI w logicAPI na mock DataAbstractAPI
            var dataAPIField = logicAPI.GetType().GetField("dataAPI", BindingFlags.NonPublic | BindingFlags.Instance);
            dataAPIField.SetValue(logicAPI, dataAPIMock.Object);

            // Wywołujemy metodę CreateBalls z 3 jako argument
            logicAPI.CreateBalls(3);

            // Sprawdzamy, czy CreateBall została wywołana 3 razy
            dataAPIMock.Verify(m => m.CreateBall(It.IsAny<Vector2>(), It.IsAny<Vector2>()), Times.Exactly(3));
        }

        [TestMethod]
        public void TestStart()
        {
            // Tworzymy mock dla interfejsu DataAbstractAPI
            var dataAPIMock = new Mock<DataAbstractAPI>();

            // Tworzymy instancję LogicAPI
            var logicAPI = new LogicAPI();

            // Ustawiamy zachowanie mocka dla GetBallsCount
            dataAPIMock.Setup(m => m.GetBallsCount()).Returns(2);

            // Tworzymy mock dla IBall
            var ballMock1 = new Mock<IBall>();
            var ballMock2 = new Mock<IBall>();

            // Ustawiamy zachowanie mocków dla metody StartMoving
            ballMock1.Setup(b => b.StartMoving());
            ballMock2.Setup(b => b.StartMoving());

            // Ustawiamy zachowanie mocka dla GetBall
            dataAPIMock.SetupSequence(m => m.GetBall(It.IsAny<int>()))
                .Returns(ballMock1.Object)
                .Returns(ballMock2.Object);

            // Ustawiamy dataAPI w logicAPI na mock DataAbstractAPI
            var dataAPIField = logicAPI.GetType().GetField("dataAPI", BindingFlags.NonPublic | BindingFlags.Instance);
            dataAPIField.SetValue(logicAPI, dataAPIMock.Object);

            // Wywołujemy metodę Start
            logicAPI.Start();

            // Sprawdzamy, czy metoda StartMoving została wywołana dla każdej kuli
            ballMock1.Verify(b => b.StartMoving(), Times.Once);
            ballMock2.Verify(b => b.StartMoving(), Times.Once);
        }
    }
}
