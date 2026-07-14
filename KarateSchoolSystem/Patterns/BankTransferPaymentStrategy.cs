using KarateSchoolSystem.Interfaces;

namespace KarateSchoolSystem.Patterns;

/// <summary>
/// GoF Strategy pattern: processes a payment made by bank transfer; rejects amounts under $10.
/// </summary>
public class BankTransferPaymentStrategy : IPaymentStrategy
{
    private const decimal MinimumAmount = 10m;

    public string StrategyName => "Bank Transfer";

    public string ProcessPayment(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Payment amount must be positive.", nameof(amount));
        }

        if (amount < MinimumAmount)
        {
            throw new ArgumentException(
                $"Bank transfer payments must be at least {MinimumAmount:C}.", nameof(amount));
        }

        return $"Bank transfer of {amount:C} initiated.";
    }
}
