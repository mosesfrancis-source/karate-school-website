using KarateSchoolSystem.Enums;
using KarateSchoolSystem.Models;

namespace KarateSchoolSystem.Tests.Models;

[TestClass]
public class PaymentTests
{
    private static Payment CreatePayment(
        int paymentId = 1,
        int studentId = 1,
        int adminId = 1,
        decimal amount = 100m,
        DateTime? paymentDate = null,
        PaymentMethod method = PaymentMethod.Cash,
        string? description = null)
    {
        return new Payment(paymentId, studentId, adminId, amount, paymentDate ?? DateTime.Today, method, description);
    }

    [TestMethod]
    public void Constructor_PaymentIdZeroOrNegative_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreatePayment(paymentId: 0));
        Assert.ThrowsExactly<ArgumentException>(() => CreatePayment(paymentId: -1));
    }

    [TestMethod]
    public void Constructor_StudentIdZeroOrNegative_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreatePayment(studentId: 0));
        Assert.ThrowsExactly<ArgumentException>(() => CreatePayment(studentId: -1));
    }

    [TestMethod]
    public void Constructor_AdminIdZeroOrNegative_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreatePayment(adminId: 0));
        Assert.ThrowsExactly<ArgumentException>(() => CreatePayment(adminId: -1));
    }

    [TestMethod]
    public void Constructor_AmountZeroOrNegative_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreatePayment(amount: 0m));
        Assert.ThrowsExactly<ArgumentException>(() => CreatePayment(amount: -5m));
    }

    [TestMethod]
    public void Constructor_FuturePaymentDate_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreatePayment(paymentDate: DateTime.Now.AddDays(1)));
    }

    [TestMethod]
    public void Constructor_TodayPaymentDate_Succeeds()
    {
        var payment = CreatePayment(paymentDate: DateTime.Today);
        Assert.AreEqual(DateTime.Today, payment.PaymentDate);
    }

    [TestMethod]
    public void Constructor_ValidArguments_SetsPropertiesAndStatusCompleted()
    {
        var payment = CreatePayment(paymentId: 2, studentId: 3, adminId: 4, amount: 50m, method: PaymentMethod.CreditCard, description: "monthly fee");
        Assert.AreEqual(2, payment.PaymentId);
        Assert.AreEqual(3, payment.StudentId);
        Assert.AreEqual(4, payment.AdminId);
        Assert.AreEqual(50m, payment.Amount);
        Assert.AreEqual(PaymentMethod.CreditCard, payment.Method);
        Assert.AreEqual(PaymentStatus.Completed, payment.Status);
        Assert.AreEqual("monthly fee", payment.Description);
    }

    [TestMethod]
    public void Void_AlreadyVoided_ThrowsInvalidOperationException()
    {
        var payment = CreatePayment();
        payment.Void("first reason");
        Assert.ThrowsExactly<InvalidOperationException>(() => payment.Void("second reason"));
    }

    [TestMethod]
    public void Void_NullOrWhitespaceReason_ThrowsArgumentException()
    {
        var payment = CreatePayment();
        Assert.ThrowsExactly<ArgumentException>(() => payment.Void(""));
        Assert.ThrowsExactly<ArgumentException>(() => payment.Void("   "));
    }

    [TestMethod]
    public void Void_WithNoExistingDescription_SetsVoidedDescriptionOnly()
    {
        var payment = CreatePayment(description: null);
        payment.Void("customer request");
        Assert.AreEqual(PaymentStatus.Voided, payment.Status);
        Assert.AreEqual("Voided: customer request", payment.Description);
    }

    [TestMethod]
    public void Void_WithExistingDescription_AppendsVoidedReason()
    {
        var payment = CreatePayment(description: "monthly fee");
        payment.Void("customer request");
        Assert.AreEqual(PaymentStatus.Voided, payment.Status);
        Assert.AreEqual("monthly fee | Voided: customer request", payment.Description);
    }

    [TestMethod]
    public void ToString_ReturnsFormattedSummary()
    {
        var payment = CreatePayment(paymentId: 6, studentId: 1, amount: 75m, method: PaymentMethod.Check);
        string result = payment.ToString();
        Assert.IsTrue(result.Contains("Payment #6"));
        Assert.IsTrue(result.Contains("Student 1"));
        Assert.IsTrue(result.Contains("Check"));
    }
}
