using System.Text;

namespace KarateSchoolSystem.Models;

public class Instructor : User
{
    private static readonly string[] AllowedCertificationLevels =
    {
        "Level 1", "Level 2", "Level 3", "Black Belt Certified", "Master Certified"
    };

    public int InstructorId { get; private set; }
    public string Specialization { get; private set; }
    public int YearsExperience { get; private set; }
    public string CertificationLevel { get; private set; }

    public Instructor(
        int instructorId,
        string firstName,
        string lastName,
        string email,
        string password,
        string specialization,
        int yearsExperience,
        string certificationLevel)
        : base(instructorId, firstName, lastName, email, password, "Instructor")
    {
        if (string.IsNullOrWhiteSpace(specialization))
        {
            throw new ArgumentException("Specialization cannot be null or whitespace.", nameof(specialization));
        }

        if (yearsExperience < 0)
        {
            throw new ArgumentException("Years of experience cannot be negative.", nameof(yearsExperience));
        }

        if (!AllowedCertificationLevels.Contains(certificationLevel))
        {
            throw new ArgumentException(
                $"Certification level must be one of: {string.Join(", ", AllowedCertificationLevels)}.",
                nameof(certificationLevel));
        }

        InstructorId = instructorId;
        Specialization = specialization;
        YearsExperience = yearsExperience;
        CertificationLevel = certificationLevel;
    }

    public override string GetRoleDescription() => $"Instructor ({CertificationLevel})";

    public override string GenerateReport()
    {
        var sb = new StringBuilder();
        sb.Append(base.GenerateReport());
        sb.AppendLine($"Instructor ID: {InstructorId}");
        sb.AppendLine($"Specialization: {Specialization}");
        sb.AppendLine($"Years Experience: {YearsExperience}");
        sb.AppendLine($"Certification Level: {CertificationLevel}");
        return sb.ToString();
    }

    public override string ToString() =>
        $"Instructor #{InstructorId}: {FullName} ({Specialization}, {CertificationLevel})";
}
