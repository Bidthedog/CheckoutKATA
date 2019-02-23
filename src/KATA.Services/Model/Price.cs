namespace KATA.Services.Model {
    /// <summary>
    ///     Represents a price for an SKU.
    /// </summary>
    public class Price {
        /// <summary>
        ///     Creates a new <see cref="Price" />
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="price"></param>
        public Price(string sku, decimal price) {
            SKU = sku;
        }

        /// <summary>
        ///     The product's SKU
        /// </summary>
        public string SKU { get; }

        /// <summary>
        ///     The product's price
        /// </summary>
        public decimal UnitPrice { get; }
    }
}