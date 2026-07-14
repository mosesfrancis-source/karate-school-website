using KarateSchoolSystem.Enums;
using KarateSchoolSystem.Patterns;

namespace KarateSchoolSystem.Tests.Patterns;

[TestClass]
public class PaymentStrategyFactoryTests
{
    [TestMethod]
    public void GetStrategy_Cash_ReturnsCashPaymentStrategy()
    {
        var strategy = PaymentStrategyFactory.GetStrategy(PaymentMethod.Cash);
        Assert.IsInstanceOfType<CashPaymentStrategy>(strategy);
    }

    [TestMethod]
    public void GetStrategy_CreditCard_ReturnsCreditCardPaymentStrategy()
    {
        var strategy = PaymentStrategyFactory.GetStrategy(PaymentMethod.CreditCard);
        Assert.IsInstanceOfType<CreditCardPaymentStrategy>(strategy);
    }

    [TestMethod]
    public void GetStrategy_BankTransfer_ReturnsBankTransferPaymentStrategy()
    {
        var strategy = PaymentStrategyFactory.GetStrategy(PaymentMethod.BankTransfer);
        Assert.IsInstanceOfType<BankTransferPaymentStrategy>(strategy);
    }

    [TestMethod]
    public void GetStrategy_Check_ReturnsCheckPaymentStrategy()
    {
        var strategy = PaymentStrategyFactory.GetStrategy(PaymentMethod.Check);
        Assert.IsInstanceOfType<CheckPaymentStrategy>(strategy);
    }

    [TestMethod]
    public void GetStrategy_UnrecognizedMethod_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => PaymentStrategyFactory.GetStrategy((PaymentMethod)999));
    }
}
