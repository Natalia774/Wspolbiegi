
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dodawanie;

namespace TestDodawanie
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestDodaj()
        {
            int a = 2;
            int b = 3;

            int oczekiwanaSuma = 8;

            int wynik = Dodaj(a, b);

            // Assert (asercja)
            Assert.AreEqual(oczekiwanaSuma, wynikSumy);


        }
    }
}
