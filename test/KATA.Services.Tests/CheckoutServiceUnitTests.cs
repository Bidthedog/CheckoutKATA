using System;
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
        /// <param name="discounts">A complete list of discounts available for each SKU</param>
        /// <returns></returns>
        private static ICheckoutService GetService(IReadOnlyDictionary<string, decimal> prices, IReadOnlyDictionary<string, IEnumerable<Discount>> discounts) {
            return new CheckoutService(prices, discounts);
        }

        [Fact]
        public void SingleItem_ScansAsExpectedUnitPrice_WhenNoDiscountExists() {
            // Arrange
            var readOnlyPrices = GetMockedPrices();
            var service = GetService(readOnlyPrices, null);

            // Act
            service.Scan("A", 1);
            var total = service.GetTotal();

            // Assert
            Assert.Equal(50, total);
        }

        [Fact]
        public void SingleItem_ScansAsExpectedUnitPrice_WhenDiscountExists() {
            // Arrange
            var readOnlyPrices = GetMockedPrices();
            var discounts = GetMockedDiscounts();
            var service = GetService(readOnlyPrices, discounts);

            // Act
            service.Scan("A", 1);
            var total = service.GetTotal();

            // Assert
            Assert.Equal(50, total);
        }

        [Fact]
        public void MultiItem_ScansAsExpectedUnitPrice_WhenDiscountExists() {
            // Arrange
            var readOnlyPrices = GetMockedPrices();
            var discounts = GetMockedDiscounts();
            var service = GetService(readOnlyPrices, discounts);

            // Act
            service.Scan("A", 2);
            var total = service.GetTotal();

            // Assert
            Assert.Equal(100, total);
        }

        [Fact]
        public void MultiItem_ScansAsExpectedUnitPrice_WhenNoDiscountExists() {
            // Arrange
            var readOnlyPrices = GetMockedPrices();
            var service = GetService(readOnlyPrices, null);

            // Act
            service.Scan("A", 2);
            var total = service.GetTotal();

            // Assert
            Assert.Equal(100, total);
        }

        [Fact]
        public void SingleItem_ThrowsExceptionWhenSKUNotFound() {
            // Arrange
            var readOnlyPrices = GetMockedPrices();
            var discounts = GetMockedDiscounts();
            var service = GetService(readOnlyPrices, discounts);

            // Act & Assert
            Assert.Throws<SKUNotFoundException>(() => service.Scan("B", 1));
        }

        [Fact]
        public void AppliesDiscount_WhenAmountIsInDiscountList() {
            // Arrange
            var readOnlyPrices = GetMockedPrices();
            var discounts = GetMockedDiscounts();
            var service = GetService(readOnlyPrices, discounts);

            // Act
            service.Scan("A", 3);
            var total = service.GetTotal();

            // Assert
            Assert.Equal(130, total);
        }

        [Fact]
        public void MultipleScansOfTheSameSKU_ReturnExpectedTotal_WhenDiscountExists() {
            // Arrange
            var readOnlyPrices = GetMockedPrices();
            var discounts = GetMockedDiscounts();
            var service = GetService(readOnlyPrices, discounts);

            // Act
            service.Scan("A", 1);
            service.Scan("A", 1);
            var total = service.GetTotal();

            // Assert
            Assert.Equal(100, total);
        }

        [Fact]
        public void MultipleScansOfTheSameSKU_ReturnExpectedTotal_WhenNoDiscountExists() {
            // Arrange
            var readOnlyPrices = GetMockedPrices();
            var service = GetService(readOnlyPrices, null);

            // Act
            service.Scan("A", 1);
            service.Scan("A", 1);
            var total = service.GetTotal();

            // Assert
            Assert.Equal(100, total);
        }

        [Fact]
        public void MultipleScansOfTheSameSKU_ThatMatchesDiscountedAmount_ReturnExpectedTotal() {
            // Arrange
            var readOnlyPrices = GetMockedPrices();
            var discounts = GetMockedDiscounts();
            var service = GetService(readOnlyPrices, discounts);

            // Act
            service.Scan("A", 1);
            service.Scan("A", 2);
            var total = service.GetTotal();

            // Assert
            Assert.Equal(130, total);
        }

        [Fact]
        public void MultiplesOfDiscount_ApplyExpectedDiscounts() {
            // Arrange
            var readOnlyPrices = GetMockedPrices();
            var discounts = GetMockedDiscounts();
            var service = GetService(readOnlyPrices, discounts);

            // Act
            service.Scan("A", 1);
            service.Scan("A", 2);
            service.Scan("A", 3);
            var total = service.GetTotal();

            // Assert
            Assert.Equal(260, total);
        }

        [Fact]
        public void MultiplesOfDiscount_WithRemainder_AppliesExpectedDiscounts() {
            // Arrange
            var readOnlyPrices = GetMockedPrices();
            var discounts = GetMockedDiscounts();
            var service = GetService(readOnlyPrices, discounts);

            // Act
            service.Scan("A", 1);
            service.Scan("A", 2);
            service.Scan("A", 3);
            service.Scan("A", 1);
            var total = service.GetTotal();

            // Assert
            Assert.Equal(310, total);
        }

        [Fact]
        public void DoesNotApplyDiscount_WhenAmountIsInDiscountListButCurrentDateIsOutsideExpectedRange() {
            // Arrange
            SystemTime.Now = () => new DateTime(2019, 2, 23);
            var readOnlyPrices = GetMockedPrices();
            var discounts = new Dictionary<string, IEnumerable<Discount>> {
                {"A", new List<Discount> {
                    new Discount(3, 130, new DateTime(2019, 1, 1), new DateTime(2019, 2, 22))
                }}
            };
            var readOnlyDiscounts = new ReadOnlyDictionary<string, IEnumerable<Discount>>(discounts);
            var service = GetService(readOnlyPrices, readOnlyDiscounts);

            // Act
            service.Scan("A", 3);
            var total = service.GetTotal();

            // Assert
            Assert.Equal(150, total);
        }


        #region Helper methods

        /// <summary>
        ///     Mocked prices. We don't need to implement all prices as the test are abstract.
        /// </summary>
        private static ReadOnlyDictionary<string, decimal> GetMockedPrices() {
            var prices = new Dictionary<string, decimal> {
                {"A", 50}
            };
            return new ReadOnlyDictionary<string, decimal>(prices);
        }

        /// <summary>
        ///     Mocked discounts. We don't need to implement all discounts as the tests are abstract.
        /// </summary>
        /// <returns></returns>
        private static ReadOnlyDictionary<string, IEnumerable<Discount>> GetMockedDiscounts() {
            var discounts = new Dictionary<string, IEnumerable<Discount>> {
                {"A", new List<Discount> {new Discount(3, 130)}}
            };
            return new ReadOnlyDictionary<string, IEnumerable<Discount>>(discounts);
        }

        #endregion
    }
}