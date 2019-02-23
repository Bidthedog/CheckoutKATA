namespace KATA.Services.Contracts {
    /// <summary>
    ///     Provides a contract for a checkout implementation
    /// </summary>
    public interface ICheckoutService {
        void Scan(string sku, int amount);

        decimal GetTotal();
    }
}