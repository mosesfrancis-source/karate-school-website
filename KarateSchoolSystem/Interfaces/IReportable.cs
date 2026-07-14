namespace KarateSchoolSystem.Interfaces;

/// <summary>
/// Implemented by classes that can produce a human-readable summary of themselves.
/// </summary>
public interface IReportable
{
    string GenerateReport();
}
