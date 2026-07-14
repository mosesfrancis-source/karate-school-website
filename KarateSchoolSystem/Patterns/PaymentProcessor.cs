using KarateSchoolSystem.Interfaces;
using KarateSchoolSystem.Models;

namespace KarateSchoolSystem.Patterns;

/// <summary>
/// GoF Strategy pattern: the context that delegates payment processing to
/// whichever IPaymentStrategy corresponds to a Payment's method.
/// </summary>
public class PaymentProcessor
{
    public string Process(Payment payment)
    {
        if (payment is null)
        {
            throw new ArgumentNullException(nameof(payment));
        }

        IPaymentStrategy strategy = PaymentStrategyFactory.GetStrategy(payment.Method);
        return strategy.ProcessPayment(payment.Amount);
    }

    public string Process(decimal amount, IPaymentStrategy strategy)
    {
        if (strategy is null)
        {
            throw new ArgumentNullException(nameof(strategy));
        }

        return strategy.ProcessPayment(amount);
    }
}
