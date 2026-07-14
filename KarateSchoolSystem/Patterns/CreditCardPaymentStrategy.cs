using KarateSchoolSystem.Interfaces;

namespace KarateSchoolSystem.Patterns;

/// <summary>
/// GoF Strategy pattern: processes a payment made by credit card, adding a 2.9% processing fee.
/// </summary>
public class CreditCardPaymentStrategy : IPaymentStrategy
{
    private static readonly decimal FeeRate = 0.029m;

    public string StrategyName => "Credit Card";

    public string ProcessPayment(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Payment amount must be positive.", nameof(amount));
        }

        decimal fee = Math.Round(amount * FeeRate, 2);
        decimal total = amount + fee;
        return $"Credit card payment of {amount:C} processed with a {fee:C} fee (total charged: {total:C}).";
    }
}
