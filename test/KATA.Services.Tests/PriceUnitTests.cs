using System.Collections.Generic;

using KATA.Services.Model;

using Xunit;

namespace KATA.Services.Tests {
    /// <summary>
    ///     Performs unit tests on the <see cref="Price" /> object. These tests are often overkill, but demonstrated here for
    ///     completeness.
    /// </summary>
    public class PriceUnitTests {
        [Fact]
        public void SetsUnitPrice_OnInstantiation() {
            // Arrange  & Act
            var price = new Price(56, null);

            // Assert
            Assert.Equal(56, price.UnitPrice);
        }

        [Fact]
        public void SetsDiscounts_OnInstantiation() {
            // Arrange  & Act
            var discounts = new List<PriceDiscount> {
                new PriceDiscount(0, 0)
            };
            var price = new Price(0, discounts);

            // Assert
            Assert.Equal(discounts, price.Discounts);
        }
    }
}