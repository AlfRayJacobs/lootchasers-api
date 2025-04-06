using LootchasersAPI.Services;

namespace LootchasersAPI.Test
{
    [TestClass]
    public sealed class ParserTests
    {
        [TestMethod]
        public void ValidateLootValueParser_Billions()
        {
            var inputString = "```ldif\n1.58B gp\n```";
            var expectedValue = 1_580_000_000;
            var actual = ValueConverter.ParseStackValue(inputString);


            Assert.AreEqual(expectedValue, actual);
        }

        [TestMethod]
        public void ValidateLootValueParser_Millions()
        {
            var inputString = "```ldif\n6.96M gp\n```";
            var expectedValue = 6_960_000;
            var actual = ValueConverter.ParseStackValue(inputString);
            

            Assert.AreEqual(expectedValue, actual);
        }

        [TestMethod]
        public void ValidateLootValueParser_Thousands()
        {
            var inputString = "```ldif\n180K gp\n```";
            var expectedValue = 180_000;
            var actual = ValueConverter.ParseStackValue(inputString);


            Assert.AreEqual(expectedValue, actual);
        }

        [TestMethod]
        public void ValidateLootValueParser_LowValue()
        {
            var inputString = "```ldif\n820 gp\n```";
            var expectedValue = 820;
            var actual = ValueConverter.ParseStackValue(inputString);


            Assert.AreEqual(expectedValue, actual);
        }
    }
}
