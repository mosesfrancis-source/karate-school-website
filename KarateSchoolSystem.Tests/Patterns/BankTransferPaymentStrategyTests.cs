using KarateSchoolSystem.Patterns;

namespace KarateSchoolSystem.Tests.Patterns;

[TestClass]
public class BankTransferPaymentStrategyTests
{
    [TestMethod]
    public void StrategyName_ReturnsBankTransfer()
    {
        var strategy = new BankTransferPaymentStrategy();
        Assert.AreEqual("Bank Transfer", strategy.StrategyName);
    }

    [TestMethod]
    public void ProcessPayment_NonPositiveAmount_ThrowsArgumentException()
    {
        var strategy = new BankTransferPaymentStrategy();
        Assert.ThrowsExactly<ArgumentException>(() => strategy.ProcessPayment(0m));
        Assert.ThrowsExactly<ArgumentException>(() => strategy.ProcessPayment(-1m));
    }

    [TestMethod]
    public void ProcessPayment_BelowMinimum_ThrowsArgumentException()
    {
        var strategy = new BankTransferPaymentStrategy();
        Assert.ThrowsExactly<ArgumentException>(() => strategy.ProcessPayment(9.99m));
    }

    [TestMethod]
    public void ProcessPayment_ExactlyMinimum_Succeeds()
    {
        var strategy = new BankTransferPaymentStrategy();
        string result = strategy.ProcessPayment(10m);
        Assert.IsTrue(result.Contains("Bank transfer"));
        Assert.IsTrue(result.Contains("$10.00"));
    }

    [TestMethod]
    public void ProcessPayment_AboveMinimum_ReturnsConfirmationMessage()
    {
        var strategy = new BankTransferPaymentStrategy();
        string result = strategy.ProcessPayment(50m);
        Assert.IsTrue(result.Contains("Bank transfer of $50.00 initiated."));
    }
}
