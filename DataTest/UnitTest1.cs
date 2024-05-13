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
            // Inicjalizujemy mock dla interfejsu DataAbstractAPI
            dataAPIMock = new Mock<DataAbstractAPI>();

            // Ustawiamy zachowanie mocka dla metody GetBoardWidth
            dataAPIMock.Setup(m => m.GetBoardWidth()).Returns(1200);

            // Ustawiamy zachowanie mocka dla metody GetBoardHeight
            dataAPIMock.Setup(m => m.GetBoardHeight()).Returns(700);
        }

        [TestMethod]
        public void TestCreateApi()
        {
            // Tworzymy instancję testowanej klasy i wstrzykujemy mocka
            DataAbstractAPI DApi = dataAPIMock.Object;

            // Aserty
            Assert.IsNotNull(DApi);
        }

        [TestMethod]
        public void TestGetWidthHeight()
        {
            // Tworzymy instancję testowanej klasy i wstrzykujemy mocka
            DataAbstractAPI DApi = dataAPIMock.Object;

            // Aserty
            Assert.AreEqual(1200, DApi.GetBoardWidth());
            Assert.AreEqual(700, DApi.GetBoardHeight());
        }
    }
}
