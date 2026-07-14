using System.Text;

namespace KarateSchoolSystem.Models;

public class Administrator : User
{
    private static readonly string[] AllowedAccessLevels = { "Standard", "Elevated", "SuperAdmin" };

    public int AdminId { get; private set; }
    public string Department { get; private set; }
    public string AccessLevel { get; private set; }

    public bool CanApproveActions => AccessLevel is "Elevated" or "SuperAdmin";

    public Administrator(
        int adminId,
        string firstName,
        string lastName,
        string email,
        string password,
        string department,
        string accessLevel)
        : base(adminId, firstName, lastName, email, password, "Administrator")
    {
        if (string.IsNullOrWhiteSpace(department))
        {
            throw new ArgumentException("Department cannot be null or whitespace.", nameof(department));
        }

        if (!AllowedAccessLevels.Contains(accessLevel))
        {
            throw new ArgumentException(
                $"Access level must be one of: {string.Join(", ", AllowedAccessLevels)}.",
                nameof(accessLevel));
        }

        AdminId = adminId;
        Department = department;
        AccessLevel = accessLevel;
    }

    public override string GetRoleDescription() => $"Administrator ({AccessLevel})";

    public override string GenerateReport()
    {
        var sb = new StringBuilder();
        sb.Append(base.GenerateReport());
        sb.AppendLine($"Admin ID: {AdminId}");
        sb.AppendLine($"Department: {Department}");
        sb.AppendLine($"Access Level: {AccessLevel}");
        return sb.ToString();
    }

    public override string ToString() =>
        $"Administrator #{AdminId}: {FullName} ({Department}, {AccessLevel})";
}
