namespace KATA.Services.Model {
    /// <summary>
    ///     Represents a price for an SKU.
    /// </summary>
    public class Price {
        /// <summary>
        ///     Creates a new <see cref="Price" />
        /// </summary>
        /// <param name="price">Read-only. The product's Unit Price</param>
        public Price(decimal price) {
            UnitPrice = price;
        }
        
        /// <summary>
        ///     The product's price
        /// </summary>
        public decimal UnitPrice { get; }
    }
}