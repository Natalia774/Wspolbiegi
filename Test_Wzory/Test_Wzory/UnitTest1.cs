
namespace Test_Wzory
{
    [TestClass]
    public class UnitTest1
    {
        [Test]
        public void SprawdzSume()

        {
            int a = 5;
            int b = 3;
            int oczekiwanaSuma = 8;

            int wynikSumy = ObliczSume(a, b);
            Assert.AreEqual(oczekiwanaSuma, wynikSumy);
        }
        [Test]
        public void SprawdzRoznice()
        {
               int a = 5;
            int b = 3;
            int oczekiwanaRóżnica = 2;


            int wynikRóżnicy = ObliczRóżnicę(a, b);


            Assert.AreEqual(oczekiwanaRóżnica, wynikRóżnicy);
        }
        
    }
}
