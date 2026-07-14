using KarateSchoolSystem.Interfaces;

namespace KarateSchoolSystem.Patterns;

/// <summary>
/// GoF Strategy pattern: processes a payment made by check, with no fee but pending clearance.
/// </summary>
public class CheckPaymentStrategy : IPaymentStrategy
{
    public string StrategyName => "Check";

    public string ProcessPayment(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Payment amount must be positive.", nameof(amount));
        }

        return $"Check payment of {amount:C} received, pending clearance.";
    }
}
