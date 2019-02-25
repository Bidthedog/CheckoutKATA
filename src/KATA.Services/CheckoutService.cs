using System;
using System.Collections.Generic;
using System.Linq;

using KATA.Services.Contracts;
using KATA.Services.Exceptions;
using KATA.Services.Model;

namespace KATA.Services {
    /// <summary>
    ///     Implements the <see cref="ICheckoutService" />
    /// </summary>
    public class CheckoutService : ICheckoutService {
        /// <summary>
        ///     A dictionary of SKUs and their unit prices
        /// </summary>
        private readonly IReadOnlyDictionary<string, decimal> _prices;

        /// <summary>
        ///     A dictionary of SKUs and their discount properties
        /// </summary>
        private readonly IReadOnlyDictionary<string, IEnumerable<Discount>> _discounts;

        /// <summary>
        ///     Shopping cart tracking for deferred discount calculation
        /// </summary>
        private readonly IDictionary<string, int> _cart = new Dictionary<string, int>();

        /// <summary>
        ///     Instantiates the <see cref="CheckoutService" /> that depends on the specified <paramref name="prices" /> and
        ///     <paramref name="discounts" />.
        /// </summary>
        /// <param name="prices"></param>
        /// <param name="discounts"></param>
        public CheckoutService(IReadOnlyDictionary<string, decimal> prices, IReadOnlyDictionary<string, IEnumerable<Discount>> discounts) {
            _prices = prices;
            _discounts = discounts;
        }

        /// <summary>
        ///     Finds the specified <paramref name="sku" /> in the checkout price list, then scans the specified
        ///     <see cref="amount" />
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="amount"></param>
        public void Scan(string sku, int amount) {
            if(amount <= 0) {
                throw new ArgumentOutOfRangeException(nameof(amount), "amount must be greater than zero");
            }

            if(_prices.ContainsKey(sku)) {
                if(!_cart.ContainsKey(sku)) {
                    _cart.Add(sku, amount);
                } else {
                    _cart[sku] += amount;
                }
            } else {
                throw new SKUNotFoundException();
            }
        }

        /// <summary>
        ///     Returns the total price of all items added to the cart
        /// </summary>
        /// <returns></returns>
        public decimal GetTotal() {
            var totalPrice = 0m;

            // This implementation does not cater for discounts that may need to be calculated
            // at Scan() time (which will depend on the policy of the seller). All calculations
            // are currently deferred until this method is called. The KATA spec does not
            // highlight which is preferred.
            foreach(var item in _cart) {
                var sku = item.Key;
                var amount = item.Value;
                if(!_prices.TryGetValue(sku, out var unitPrice)) {
                    // Unlikely to hit this unless price list is mutated before GetTotal() is
                    // called, as the Scan() method checks that SKUs exist.
                    continue;
                }

                // Extract all discounts for this SKU.
                // All the below discount calculations should be extracted to a separate
                // service dependency and mocked to make the CheckoutServiceUnitTests simpler
                var discounts = _discounts?.FirstOrDefault(d => d.Key == sku).Value;

                // Discover if there is at least one discount for this SKU that should be applied
                // at the moment (date based discounts).
                // The below logic can be made more complex, depending on the rules surrounding
                // how StartDate and EndDate nulls are handled. At the moment, both dates must be
                // populated for them to be applied, or both must be null. It currently ignores
                // discounts that are configured with only one date populated.

                // This lambda does not cater for multiple discounts that may exist for the same date
                // range.

                // Redundant parentheses are included for clarity / sanity. BODMAS yadda yadda.
                var discount = discounts?
                    .FirstOrDefault(d =>
                        (!d.StartDate.HasValue && !d.EndDate.HasValue)
                        || (d.StartDate.HasValue && d.EndDate.HasValue
                                                 && SystemTime.Now() > d.StartDate
                                                 && SystemTime.Now() < d.EndDate)
                    );

                if(discount != null && amount >= discount.Amount) {
                    var noDiscounts = amount / discount.Amount;
                    var noRemainingUnits = amount % discount.Amount;

                    totalPrice += ((discount.TotalPrice) * noDiscounts) + (unitPrice * noRemainingUnits);
                } else {
                    totalPrice += unitPrice * amount;
                }
            }

            return totalPrice;
        }
    }
}