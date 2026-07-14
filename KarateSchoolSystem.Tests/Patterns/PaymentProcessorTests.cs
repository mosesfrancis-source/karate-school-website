using KarateSchoolSystem.Enums;
using KarateSchoolSystem.Interfaces;
using KarateSchoolSystem.Models;
using KarateSchoolSystem.Patterns;

namespace KarateSchoolSystem.Tests.Patterns;

[TestClass]
public class PaymentProcessorTests
{
    private class FakeStrategy : IPaymentStrategy
    {
        public string StrategyName => "Fake";
        public decimal? LastAmount { get; private set; }

        public string ProcessPayment(decimal amount)
        {
            LastAmount = amount;
            return $"Fake processed {amount}";
        }
    }

    private static Payment CreatePayment(PaymentMethod method = PaymentMethod.Cash, decimal amount = 100m) =>
        new(1, 1, 1, amount, DateTime.Today, method);

    [TestMethod]
    public void Process_Payment_NullPayment_ThrowsArgumentNullException()
    {
        var processor = new PaymentProcessor();
        Assert.ThrowsExactly<ArgumentNullException>(() => processor.Process((Payment)null!));
    }

    [TestMethod]
    public void Process_Payment_DelegatesToCorrectStrategy()
    {
        var processor = new PaymentProcessor();
        var payment = CreatePayment(PaymentMethod.Cash, 50m);

        string result = processor.Process(payment);

        Assert.IsTrue(result.Contains("Cash payment"));
    }

    [TestMethod]
    public void Process_AmountAndStrategy_NullStrategy_ThrowsArgumentNullException()
    {
        var processor = new PaymentProcessor();
        Assert.ThrowsExactly<ArgumentNullException>(() => processor.Process(50m, null!));
    }

    [TestMethod]
    public void Process_AmountAndStrategy_DelegatesToProvidedStrategy()
    {
        var processor = new PaymentProcessor();
        var strategy = new FakeStrategy();

        string result = processor.Process(75m, strategy);

        Assert.AreEqual(75m, strategy.LastAmount);
        Assert.AreEqual("Fake processed 75", result);
    }
}
