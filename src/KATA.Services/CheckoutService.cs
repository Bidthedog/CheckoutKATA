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
        ///     Private field for price list
        /// </summary>
        private readonly IReadOnlyDictionary<string, Price> _prices;

        /// <summary>
        ///     Contains the total price of all the items scanned at the checkout
        /// </summary>
        private decimal _totalPrice;

        /// <summary>
        ///     Instantiates the <see cref="CheckoutService" /> that depends on the specified <see cref="Price" /> dictionary.
        /// </summary>
        /// <param name="prices"></param>
        public CheckoutService(IReadOnlyDictionary<string, Price> prices) {
            _prices = prices;
        }

        /// <summary>
        ///     Finds the specified <paramref name="sku" /> in the checkout price list, then scans the specified
        ///     <see cref="amount" />
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="amount"></param>
        public void Scan(string sku, int amount) {
            if(_prices.TryGetValue(sku, out var price)) {
                var discount = price.Discounts?.FirstOrDefault(d => d.Amount == amount);

                if(discount != null) {
                    _totalPrice += discount.TotalPrice;
                } else {
                    _totalPrice += price.UnitPrice;
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
            return _totalPrice;
        }
    }
}