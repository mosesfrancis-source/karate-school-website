using KarateSchoolSystem.Interfaces;

namespace KarateSchoolSystem.Patterns;

/// <summary>
/// GoF Strategy pattern: processes a payment made in cash, with no processing fee.
/// </summary>
public class CashPaymentStrategy : IPaymentStrategy
{
    public string StrategyName => "Cash";

    public string ProcessPayment(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Payment amount must be positive.", nameof(amount));
        }

        return $"Cash payment of {amount:C} received in full.";
    }
}
