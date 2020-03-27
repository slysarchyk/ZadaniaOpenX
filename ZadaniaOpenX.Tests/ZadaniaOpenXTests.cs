using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZadaniaOpenX.Tests
{
    [TestClass]
    public class ZadaniaOpenXTests
    {
        [TestMethod]
        public void TestCheckRepeatTitlePost()
        {
            int expected = 0;

            int actual = Program.CheckRepeatTitlePost();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestCountPost()
        {
            int expected = 100;

            int actual = Program.CountPost();

            Assert.AreEqual(expected, actual);
        }
    }
}
