namespace KarateSchoolSystem.Exceptions;

/// <summary>
/// Thrown when enrolling a student in a KarateClass would exceed its capacity.
/// </summary>
public class ClassCapacityExceededException : InvalidOperationException
{
    public ClassCapacityExceededException()
        : base("This class has reached its maximum capacity.")
    {
    }

    public ClassCapacityExceededException(string message)
        : base(message)
    {
    }

    public ClassCapacityExceededException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
