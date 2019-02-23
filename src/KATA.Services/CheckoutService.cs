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

            // This implementation does not cater for discounts that need to be calculated at Scan time (which will depend on the policy of the seller).
            foreach(var item in _cart) {
                var sku = item.Key;
                var amount = item.Value;
                if(!_prices.TryGetValue(sku, out var unitPrice)) {
                    // Unlikely to hit this unless price list is mutated, as
                    // the Scan() method checks for existing SKUs
                    continue;
                }

                // Extract all discounts for this SKU
                // This should be abstracted to a DiscountService and mocked
                var discounts = _discounts?.FirstOrDefault(d => d.Key == sku).Value;

                // Find the specific discount for the selected amount.
                // The below lookup may change for date-based discounts
                var discount = discounts?.FirstOrDefault(d => d.Amount == amount);
                if(discount != null) {
                    totalPrice += discount.TotalPrice;
                } else {
                    totalPrice += unitPrice * amount;
                }
            }

            return totalPrice;
        }
    }
}