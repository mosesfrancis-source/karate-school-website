using KarateSchoolSystem.Enums;

namespace KarateSchoolSystem.Models;

public class Payment
{
    public int PaymentId { get; private set; }
    public int StudentId { get; private set; }
    public int AdminId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime PaymentDate { get; private set; }
    public PaymentMethod Method { get; private set; }
    public PaymentStatus Status { get; private set; }
    public string? Description { get; private set; }

    public Payment(
        int paymentId,
        int studentId,
        int adminId,
        decimal amount,
        DateTime paymentDate,
        PaymentMethod method,
        string? description = null)
    {
        if (paymentId <= 0)
        {
            throw new ArgumentException("Payment ID must be greater than zero.", nameof(paymentId));
        }

        if (studentId <= 0)
        {
            throw new ArgumentException("Student ID must be greater than zero.", nameof(studentId));
        }

        if (adminId <= 0)
        {
            throw new ArgumentException("Admin ID must be greater than zero.", nameof(adminId));
        }

        if (amount <= 0)
        {
            throw new ArgumentException("Payment amount must be positive.", nameof(amount));
        }

        if (paymentDate.Date > DateTime.Now.Date)
        {
            throw new ArgumentException("Payment date cannot be in the future.", nameof(paymentDate));
        }

        PaymentId = paymentId;
        StudentId = studentId;
        AdminId = adminId;
        Amount = amount;
        PaymentDate = paymentDate;
        Method = method;
        Status = PaymentStatus.Completed;
        Description = description;
    }

    public void Void(string reason)
    {
        if (Status == PaymentStatus.Voided)
        {
            throw new InvalidOperationException("This payment has already been voided.");
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException("A reason is required to void a payment.", nameof(reason));
        }

        Status = PaymentStatus.Voided;
        Description = string.IsNullOrWhiteSpace(Description)
            ? $"Voided: {reason}"
            : $"{Description} | Voided: {reason}";
    }

    public override string ToString() =>
        $"Payment #{PaymentId}: Student {StudentId}, {Amount:C} via {Method} ({Status})";
}
