using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data;
using Moq;

namespace DataTest
{
    [TestClass]
    public class DataCreationTest
    {
        private Mock<DataAbstractAPI> dataAPIMock;

        [TestInitialize]
        public void Setup()
        {
            dataAPIMock = new Mock<DataAbstractAPI>();
            dataAPIMock.Setup(m => m.GetBoardWidth()).Returns(1200);
            dataAPIMock.Setup(m => m.GetBoardHeight()).Returns(700);
        }

        [TestMethod]
        public void TestCreateApi()
        {
            DataAbstractAPI DApi = dataAPIMock.Object;

            Assert.IsNotNull(DApi);
        }

        [TestMethod]
        public void TestGetWidthHeight()
        {
            DataAbstractAPI DApi = dataAPIMock.Object;

            Assert.AreEqual(1200, DApi.GetBoardWidth());
            Assert.AreEqual(700, DApi.GetBoardHeight());
        }
    }
}
