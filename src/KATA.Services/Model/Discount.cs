namespace KATA.Services.Model {
    /// <summary>
    ///     Basic discount class - this could be abstracted out into a fully-fledged, dynamic pricing table with its own DSL,
    ///     but the KATA specifies that discounts are only based on multiples of specific SKUs. I have not implemented tests
    ///     for this class as this is demonstrated in the <see cref="Price" /> class.
    /// </summary>
    public class Discount {
        /// <summary>
        ///     Instantiates a discount with the specified <paramref name="amount" /> and <paramref name="totalPrice" />
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="totalPrice"></param>
        public Discount(int amount, decimal totalPrice) {
            Amount = amount;
            TotalPrice = totalPrice;
        }

        /// <summary>
        ///     Specifies the amount of items that a discount should be applied to
        /// </summary>
        public int Amount { get; }

        /// <summary>
        ///     The total price that should be returned when this amount is returned
        /// </summary>
        public decimal TotalPrice { get; }
    }
}