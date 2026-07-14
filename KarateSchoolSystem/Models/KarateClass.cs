using System.Text;
using KarateSchoolSystem.Enums;
using KarateSchoolSystem.Exceptions;
using KarateSchoolSystem.Interfaces;

namespace KarateSchoolSystem.Models;

public class KarateClass : IReportable
{
    private readonly HashSet<int> _enrolledStudentIds = new();

    public int ClassId { get; private set; }
    public int InstructorId { get; private set; }
    public string ClassName { get; private set; }
    public DateTime Schedule { get; private set; }
    public string Location { get; private set; }
    public int Capacity { get; private set; }
    public BeltLevel BeltLevelRequired { get; private set; }

    public int CurrentEnrollment => _enrolledStudentIds.Count;
    public bool HasAvailableCapacity => CurrentEnrollment < Capacity;

    public KarateClass(
        int classId,
        int instructorId,
        string className,
        DateTime schedule,
        string location,
        int capacity,
        BeltLevel beltLevelRequired)
    {
        if (classId <= 0)
        {
            throw new ArgumentException("Class ID must be greater than zero.", nameof(classId));
        }

        if (instructorId <= 0)
        {
            throw new ArgumentException("Instructor ID must be greater than zero.", nameof(instructorId));
        }

        if (string.IsNullOrWhiteSpace(className))
        {
            throw new ArgumentException("Class name cannot be null or whitespace.", nameof(className));
        }

        if (string.IsNullOrWhiteSpace(location))
        {
            throw new ArgumentException("Location cannot be null or whitespace.", nameof(location));
        }

        if (capacity <= 0)
        {
            throw new ArgumentException("Capacity must be greater than zero.", nameof(capacity));
        }

        ClassId = classId;
        InstructorId = instructorId;
        ClassName = className;
        Schedule = schedule;
        Location = location;
        Capacity = capacity;
        BeltLevelRequired = beltLevelRequired;
    }

    public void EnrollStudent(int studentId)
    {
        if (_enrolledStudentIds.Contains(studentId))
        {
            throw new InvalidOperationException("This student is already enrolled in this class.");
        }

        if (!HasAvailableCapacity)
        {
            throw new ClassCapacityExceededException(
                $"Class '{ClassName}' has reached its capacity of {Capacity}.");
        }

        _enrolledStudentIds.Add(studentId);
    }

    public void WithdrawStudent(int studentId)
    {
        if (!_enrolledStudentIds.Contains(studentId))
        {
            throw new InvalidOperationException("This student is not enrolled in this class.");
        }

        _enrolledStudentIds.Remove(studentId);
    }

    public bool IsStudentEnrolled(int studentId) => _enrolledStudentIds.Contains(studentId);

    public string GenerateReport()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Class ID: {ClassId}");
        sb.AppendLine($"Class Name: {ClassName}");
        sb.AppendLine($"Instructor ID: {InstructorId}");
        sb.AppendLine($"Schedule: {Schedule:g}");
        sb.AppendLine($"Location: {Location}");
        sb.AppendLine($"Belt Level Required: {BeltLevelRequired}");
        sb.AppendLine($"Enrollment: {CurrentEnrollment}/{Capacity}");
        return sb.ToString();
    }

    public override string ToString() =>
        $"Class #{ClassId}: {ClassName} ({CurrentEnrollment}/{Capacity}, requires {BeltLevelRequired} belt)";
}
