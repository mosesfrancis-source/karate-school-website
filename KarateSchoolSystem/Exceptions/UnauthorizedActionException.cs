namespace KarateSchoolSystem.Exceptions;

/// <summary>
/// Thrown when a user's role or access level does not permit the requested action,
/// e.g. a Standard-access administrator attempting to approve a belt promotion.
/// </summary>
public class UnauthorizedActionException : InvalidOperationException
{
    public UnauthorizedActionException()
        : base("This user is not authorized to perform this action.")
    {
    }

    public UnauthorizedActionException(string message)
        : base(message)
    {
    }

    public UnauthorizedActionException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
