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
            //mock initialization for the DataAbstractAPI interface
            dataAPIMock = new Mock<DataAbstractAPI>();

            //setting the mock behavior for the GetBoardWidth method
            dataAPIMock.Setup(m => m.GetBoardWidth()).Returns(1200);

            //setting the mock behavior for the GetBoardHeight method 
            dataAPIMock.Setup(m => m.GetBoardHeight()).Returns(700);
        }

        [TestMethod]
        public void TestCreateApi()
        {
            //create an instance of the tested class and inject the mock
            DataAbstractAPI DApi = dataAPIMock.Object;

            Assert.IsNotNull(DApi);
        }

        [TestMethod]
        public void TestGetWidthHeight()
        {
            //create an instance of the tested class and inject the mock
            DataAbstractAPI DApi = dataAPIMock.Object;

            Assert.AreEqual(1200, DApi.GetBoardWidth());
            Assert.AreEqual(700, DApi.GetBoardHeight());
        }
    }
}
