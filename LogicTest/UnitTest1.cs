using Data;
using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Numerics;
using System.Reflection;

namespace LogicTest
{
    [TestClass]
    public class LogicAPITest
    {
        [TestMethod]
        public void TestStartMethod()
        {
            // Tworzymy mock dla interfejsu DataAbstractAPI
            var dataAPIMock = new Mock<DataAbstractAPI>();

            // Ustawiamy zachowanie mocka dla metody GetBoardWidth
            dataAPIMock.Setup(m => m.GetBoardWidth()).Returns(800);

            // Ustawiamy zachowanie mocka dla metody GetBoardHeight
            dataAPIMock.Setup(m => m.GetBoardHeight()).Returns(600);

            // Tworzymy instancję LogicAPI
            var logicAPI = new LogicAPI();

            // Używamy refleksji do uzyskania dostępu do pola dataAPI w klasie LogicAPI
            FieldInfo dataAPIField = logicAPI.GetType().GetField("dataAPI", BindingFlags.NonPublic | BindingFlags.Instance);
            if (dataAPIField == null)
            {
                throw new InvalidOperationException("Could not find dataAPI field in LogicAPI class.");
            }

            // Ustawiamy wartość pola dataAPI na mock DataAbstractAPI
            dataAPIField.SetValue(logicAPI, dataAPIMock.Object);

            // Wywołujemy metodę Start
            logicAPI.Start(5);

            // Sprawdzamy, czy metoda CreateBall została wywołana poprawnie
            dataAPIMock.Verify(m => m.CreateBall(It.IsAny<Vector2>(), It.IsAny<Vector2>(), It.IsAny<int>()), Times.Exactly(5));
        }
    }
}
