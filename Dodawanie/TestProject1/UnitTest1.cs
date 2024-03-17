using Dodaj;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;
namespace TestProject1

{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestDodaj()
        {
            int a = 5;
            int b = 3;
            int oczekiwana = 8;

            int wynik = Dodaj.Liczenie.Dodawanie(a, b);

            Assert.AreEqual(oczekiwana, wynik);
        }
    }
}
