using KarateSchoolSystem.Patterns;

namespace KarateSchoolSystem.Tests.Patterns;

[TestClass]
public class CashPaymentStrategyTests
{
    [TestMethod]
    public void StrategyName_ReturnsCash()
    {
        var strategy = new CashPaymentStrategy();
        Assert.AreEqual("Cash", strategy.StrategyName);
    }

    [TestMethod]
    public void ProcessPayment_NonPositiveAmount_ThrowsArgumentException()
    {
        var strategy = new CashPaymentStrategy();
        Assert.ThrowsExactly<ArgumentException>(() => strategy.ProcessPayment(0m));
        Assert.ThrowsExactly<ArgumentException>(() => strategy.ProcessPayment(-1m));
    }

    [TestMethod]
    public void ProcessPayment_PositiveAmount_ReturnsConfirmationMessage()
    {
        var strategy = new CashPaymentStrategy();
        string result = strategy.ProcessPayment(100m);
        Assert.IsTrue(result.Contains("Cash payment"));
        Assert.IsTrue(result.Contains("received in full"));
    }
}
