using System;
using Exercise2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Exercise2Tests
{
    [TestClass]
    public class FibonacciEnumeratorCreatorTests
    {
        [TestMethod]
        public void GetEnumerator_Nothing_ReturnedValueIsNotNull()
        {
            // Arrange
            // Act
            var result = FibonacciEnumeratorCreator.GetEnumerator();

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
