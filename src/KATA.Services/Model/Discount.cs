using System;

namespace KATA.Services.Model {
    /// <summary>
    ///     Basic discount class - this could be abstracted out into a fully-fledged, dynamic pricing table with its own DSL,
    ///     but the KATA specifies that discounts are only based on multiples of specific SKUs.
    /// </summary>
    public class Discount {
        /// <summary>
        ///     Instantiates a discount with the specified <paramref name="amount" /> and <paramref name="totalPrice" />
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="totalPrice"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public Discount(int amount, decimal totalPrice, DateTime? startDate = null, DateTime? endDate = null) {
            Amount = amount;
            TotalPrice = totalPrice;
            StartDate = startDate;
            EndDate = endDate;
        }

        /// <summary>
        ///     Specifies the amount of items that a discount should be applied to
        /// </summary>
        public int Amount { get; }

        /// <summary>
        ///     The total price that should be returned when this amount is returned
        /// </summary>
        public decimal TotalPrice { get; }

        /// <summary>
        ///     Inclusive start date of when the discount should be applied
        /// </summary>
        public DateTime? StartDate { get; }

        /// <summary>
        ///     Inclusive end date of when the discount should be applied
        /// </summary>
        public DateTime? EndDate { get; }
    }
}