using Exercise2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Exercise2Tests
{
    [TestClass]
    public class FibonacciEnumeratorTests
    {
        [TestMethod]
        public void Current_BeforeFirstCallMoveNext_ReturnsZero()
        {
            // Arrange
            var enumerator = FibonacciEnumeratorCreator.GetEnumerator();

            // Act
            int current = enumerator.Current;

            // Assert
            Assert.AreEqual(0, current);
        }

        [TestMethod]
        public void Current_AfterOneCallMoveNext_ReturnsOne()
        {
            // Arrange
            var enumerator = FibonacciEnumeratorCreator.GetEnumerator();
            enumerator.MoveNext();

            // Act
            int current = enumerator.Current;

            // Assert
            Assert.AreEqual(1, current);
        }

        [TestMethod]
        public void Current_Nothing_ReturnsFibonacciSequenceForFirstTwentyCalls()
        {
            // Arrange
            var enumerator = FibonacciEnumeratorCreator.GetEnumerator();
            enumerator.MoveNext();

            // Act
            int last = 0;
            int current = 0;
            current = enumerator.Current;
            for (int i = 0; i < 20; i++)
            {
                int sum = last + current;
                last = current;
                enumerator.MoveNext();
                current = enumerator.Current;

                // Assert
                Assert.AreEqual(sum, current);
            }                    
        }
    }
}
