using KarateSchoolSystem.Patterns;

namespace KarateSchoolSystem.Tests.Patterns;

[TestClass]
public class CreditCardPaymentStrategyTests
{
    [TestMethod]
    public void StrategyName_ReturnsCreditCard()
    {
        var strategy = new CreditCardPaymentStrategy();
        Assert.AreEqual("Credit Card", strategy.StrategyName);
    }

    [TestMethod]
    public void ProcessPayment_NonPositiveAmount_ThrowsArgumentException()
    {
        var strategy = new CreditCardPaymentStrategy();
        Assert.ThrowsExactly<ArgumentException>(() => strategy.ProcessPayment(0m));
        Assert.ThrowsExactly<ArgumentException>(() => strategy.ProcessPayment(-1m));
    }

    [TestMethod]
    public void ProcessPayment_PositiveAmount_AddsFeeAndReturnsConfirmation()
    {
        var strategy = new CreditCardPaymentStrategy();
        string result = strategy.ProcessPayment(100m);

        // fee = 100 * 0.029 = 2.9, total = 102.9
        Assert.IsTrue(result.Contains("Credit card payment"));
        Assert.IsTrue(result.Contains("$2.90"));
        Assert.IsTrue(result.Contains("$102.90"));
    }
}
