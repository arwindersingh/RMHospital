using System;
using Xunit;
using Moq;
using System.Collections.Generic;

namespace HospitalAllocation.Tests
{
    public class ExampleTests
    {
        [Fact]
        public void FactExample()
        {
            Assert.Equal(1 + 1, 2);
        }

        [Theory]
        [InlineData(1, 12)]
        [InlineData(2, 6)]
        [InlineData(3, 4)]
        public void TheoryExample(int a, int b)
        {
            Assert.Equal(a * b, 12);
        }

        [Fact]
        public void ExceptionExample()
        {
            int zero = 0;
            Assert.Throws<DivideByZeroException>(() => 5 / zero);
        }

        public interface IMockExample
        {
            int Add(int a, int b);

            int Sum(IEnumerable<int> values);
        }

        [Theory]
        [InlineData(1, 2, 3)]
        public void AddMockExample(int a, int b, int c)
        {
            var mock = new Mock<IMockExample>();
            mock.Setup(foo => foo.Add(It.IsAny<int>(), It.IsAny<int>())).Returns((int x, int y) => x + y);
            IMockExample obj = mock.Object;

            Assert.Equal(obj.Add(a, b), c);
        }

        [Fact]
        public void NullSumMockExample()
        {
            var mock = new Mock<IMockExample>();
            mock.Setup(foo => foo.Sum(null)).Throws<NullReferenceException>();
            IMockExample obj = mock.Object;

            Assert.Throws<NullReferenceException>(() => obj.Sum(null));
        }

        [Fact]
        public void SumMockExample()
        {
            var mock = new Mock<IMockExample>();
            mock.Setup(foo => foo.Sum(It.IsAny<IEnumerable<int>>())).Returns(42);
            IMockExample obj = mock.Object;

            var list = new List<int> { 1, 2, 3 };
            Assert.Equal(obj.Sum(list), 42);
        }
    }
}
