namespace KarateSchoolSystem.Interfaces;

/// <summary>
/// Strategy pattern: implemented by each supported way of processing a payment.
/// </summary>
public interface IPaymentStrategy
{
    string StrategyName { get; }
    string ProcessPayment(decimal amount);
}
