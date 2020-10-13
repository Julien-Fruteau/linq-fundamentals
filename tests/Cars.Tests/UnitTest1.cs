using System;
using Xunit;

namespace Cars.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var d = double.Parse("1.8", System.Globalization.CultureInfo.InvariantCulture);
            Assert.Equal(1.8, d);
        }
    }
}
