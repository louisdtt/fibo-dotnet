using Xunit;

namespace Fibonacci.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async void Test1()
        {
            var result = await Compute.ExecuteAsync(new[] {"6"});
            Assert.Equal(8, result[0]);
        }
    }
}