using System;
using System.Collections.Generic;

using KATA.Services.Contracts;
using KATA.Services.Model;

namespace KATA.Services {
    /// <summary>
    ///     Implements the <see cref="ICheckoutService" />
    /// </summary>
    public class CheckoutService : ICheckoutService {
        /// <summary>
        ///     Instantiates the <see cref="CheckoutService" /> that depends on the specified <see cref="Price" /> dictionary.
        /// </summary>
        /// <param name="prices"></param>
        public CheckoutService(IReadOnlyDictionary<string, Price> prices) {
        }

        public void Scan(string sku, int amount) {
            throw new NotImplementedException();
        }

        public decimal GetTotal() {
            throw new NotImplementedException();
        }
    }
}