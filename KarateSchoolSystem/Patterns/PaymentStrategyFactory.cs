using KarateSchoolSystem.Enums;
using KarateSchoolSystem.Interfaces;

namespace KarateSchoolSystem.Patterns;

/// <summary>
/// GoF Factory Method pattern: maps a PaymentMethod enum value to the
/// concrete IPaymentStrategy that knows how to process it.
/// </summary>
public static class PaymentStrategyFactory
{
    public static IPaymentStrategy GetStrategy(PaymentMethod method)
    {
        return method switch
        {
            PaymentMethod.Cash => new CashPaymentStrategy(),
            PaymentMethod.CreditCard => new CreditCardPaymentStrategy(),
            PaymentMethod.BankTransfer => new BankTransferPaymentStrategy(),
            PaymentMethod.Check => new CheckPaymentStrategy(),
            _ => throw new ArgumentException($"Unrecognized payment method: {method}.", nameof(method))
        };
    }
}
