using System.Collections.Generic;
using System.Collections.ObjectModel;

using KATA.Services.Contracts;
using KATA.Services.Exceptions;
using KATA.Services.Model;

using Xunit;

namespace KATA.Services.Tests {
    /// <summary>
    ///     Performs unit tests on the <see cref="CheckoutService" />
    /// </summary>
    public class CheckoutServiceUnitTests {
        /// <summary>
        ///     Returns a <see cref="CheckoutService" /> (as an <see cref="ICheckoutService" /> in order to verify that the
        ///     contract has also been updated) with default dependencies. Abstracted into its own method to simplify updating
        ///     dependencies during TDD.
        /// </summary>
        /// <param name="prices">A complete list of prices available to the service</param>
        /// <returns></returns>
        private static ICheckoutService GetService(IReadOnlyDictionary<string, Price> prices) {
            return new CheckoutService(prices);
        }

        [Fact]
        public void SingleItem_ScansAsExpectedUnitPrice() {
            // Arrange
            var readOnlyPrices = GetMockedPrices();
            var service = GetService(readOnlyPrices);

            // Act
            service.Scan("A", 1);
            var total = service.GetTotal();

            // Assert
            Assert.Equal(50, total);
        }

        [Fact]
        public void SingleItem_ThrowsExceptionWhenSKUNotFound() {
            // Arrange
            var readOnlyPrices = GetMockedPrices();
            var service = GetService(readOnlyPrices);

            // Act & Assert
            Assert.Throws<SKUNotFoundException>(() => service.Scan("B", 1));
        }

        [Fact]
        public void AppliesDiscount_WhenAmountIsInDiscountList() {
            // Arrange
            var readOnlyPrices = GetMockedPrices();
            var service = GetService(readOnlyPrices);

            // Act
            service.Scan("A", 3);
            var total = service.GetTotal();

            // Assert
            Assert.Equal(130, total);
        }

        #region Helper methods

        /// <summary>
        ///     Mocked prices. We don't need to implement all prices as the test are abstract.
        /// </summary>
        private static ReadOnlyDictionary<string, Price> GetMockedPrices() {
            var prices = new Dictionary<string, Price> {
                {
                    "A", new Price(50, new List<PriceDiscount> {
                        new PriceDiscount(3, 130)
                    })
                }
            };
            return new ReadOnlyDictionary<string, Price>(prices);
        }

        #endregion
    }
}