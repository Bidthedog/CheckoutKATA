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
            var price = new Price(56);

            // Assert
            Assert.Equal(56, price.UnitPrice);
        }
    }
}