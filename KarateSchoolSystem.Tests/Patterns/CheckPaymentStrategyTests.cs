using KarateSchoolSystem.Patterns;

namespace KarateSchoolSystem.Tests.Patterns;

[TestClass]
public class CheckPaymentStrategyTests
{
    [TestMethod]
    public void StrategyName_ReturnsCheck()
    {
        var strategy = new CheckPaymentStrategy();
        Assert.AreEqual("Check", strategy.StrategyName);
    }

    [TestMethod]
    public void ProcessPayment_NonPositiveAmount_ThrowsArgumentException()
    {
        var strategy = new CheckPaymentStrategy();
        Assert.ThrowsExactly<ArgumentException>(() => strategy.ProcessPayment(0m));
        Assert.ThrowsExactly<ArgumentException>(() => strategy.ProcessPayment(-1m));
    }

    [TestMethod]
    public void ProcessPayment_PositiveAmount_ReturnsConfirmationMessage()
    {
        var strategy = new CheckPaymentStrategy();
        string result = strategy.ProcessPayment(100m);
        Assert.IsTrue(result.Contains("Check payment"));
        Assert.IsTrue(result.Contains("pending clearance"));
    }
}
