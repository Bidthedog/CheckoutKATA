using KATA.Services.Model;

using Xunit;

namespace KATA.Services.Tests {
    /// <summary>
    ///     Performs unit tests on the <see cref="Price" /> object. These tests are often overkill, but demonstrated here for
    ///     completeness.
    /// </summary>
    public class PriceUnitTests {
        [Fact]
        public void SetsSKU_OnInstantiation() {
            // Arrange  & Act
            var price = new Price("A", 0);

            // Assert
            Assert.Equal("A", price.SKU);
        }
    }
}