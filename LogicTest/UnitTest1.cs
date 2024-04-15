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
            var dataAPIMock = new Mock<DataAbstractAPI>();
            dataAPIMock.Setup(m => m.GetBoardWidth()).Returns(800);
            dataAPIMock.Setup(m => m.GetBoardHeight()).Returns(600);
            var logicAPI = new LogicAPI();
            FieldInfo dataAPIField = logicAPI.GetType().GetField("dataAPI", BindingFlags.NonPublic | BindingFlags.Instance);
            if (dataAPIField == null)
            {
                throw new InvalidOperationException("Could not find dataAPI field in LogicAPI class.");
            }

            dataAPIField.SetValue(logicAPI, dataAPIMock.Object);

            logicAPI.StartGame(5);
            dataAPIMock.Verify(m => m.CreateBall(It.IsAny<Vector2>(), It.IsAny<Vector2>(), It.IsAny<int>()), Times.Exactly(5));
        }
    }
}
