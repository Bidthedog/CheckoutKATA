using System;
using System.Collections.Generic;

using KATA.Services.Contracts;
using KATA.Services.Model;

namespace KATA.Services {
    /// <summary>
    ///     Implements the <see cref="ICheckoutService" />
    /// </summary>
    public class CheckoutService : ICheckoutService {
        public CheckoutService(IEnumerable<Price> prices) {
        }

        public void Scan(string sku, int amount) {
            throw new NotImplementedException();
        }

        public decimal GetTotal() {
            throw new NotImplementedException();
        }
    }
}