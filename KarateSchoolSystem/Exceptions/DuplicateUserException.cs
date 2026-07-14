namespace KarateSchoolSystem.Exceptions;

/// <summary>
/// Thrown when attempting to register a Student, Instructor, or Administrator
/// whose ID is already registered in the system.
/// </summary>
public class DuplicateUserException : InvalidOperationException
{
    public DuplicateUserException()
        : base("A user with this ID is already registered.")
    {
    }

    public DuplicateUserException(string message)
        : base(message)
    {
    }

    public DuplicateUserException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
