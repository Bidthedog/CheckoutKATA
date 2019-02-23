using System.Collections.Generic;

namespace KATA.Services.Model {
    /// <summary>
    ///     Represents a price for an SKU.
    /// </summary>
    public class Price {
        /// <summary>
        ///     Creates a new <see cref="Price" />
        /// </summary>
        /// <param name="price">Read-only. The product's Unit Price</param>
        /// <param name="discounts"></param>
        public Price(decimal price, IEnumerable<PriceDiscount> discounts) {
            UnitPrice = price;
            Discounts = discounts;
        }

        /// <summary>
        ///     The product's price
        /// </summary>
        public decimal UnitPrice { get; }

        /// <summary>
        ///     A list of discounts that should be scanned before a total price is returned.
        /// </summary>
        public IEnumerable<PriceDiscount> Discounts { get; }
    }
}