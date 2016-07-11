using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Exercise2;

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
