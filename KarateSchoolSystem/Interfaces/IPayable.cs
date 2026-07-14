namespace KarateSchoolSystem.Interfaces;

/// <summary>
/// Implemented by entities that carry a balance and can be charged or pay it down, such as Student.
/// </summary>
public interface IPayable
{
    decimal OutstandingBalance { get; }
    void ApplyCharge(decimal amount);
    void MakePayment(decimal amount);
}
