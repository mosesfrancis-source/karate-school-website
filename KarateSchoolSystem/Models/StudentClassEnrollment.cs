using KarateSchoolSystem.Enums;

namespace KarateSchoolSystem.Models;

public class StudentClassEnrollment
{
    public int EnrollmentId { get; private set; }
    public int StudentId { get; private set; }
    public int ClassId { get; private set; }
    public DateTime EnrollmentDate { get; private set; }
    public EnrollmentStatus Status { get; private set; }

    public StudentClassEnrollment(int enrollmentId, int studentId, int classId, DateTime enrollmentDate)
    {
        if (enrollmentId <= 0)
        {
            throw new ArgumentException("Enrollment ID must be greater than zero.", nameof(enrollmentId));
        }

        if (studentId <= 0)
        {
            throw new ArgumentException("Student ID must be greater than zero.", nameof(studentId));
        }

        if (classId <= 0)
        {
            throw new ArgumentException("Class ID must be greater than zero.", nameof(classId));
        }

        EnrollmentId = enrollmentId;
        StudentId = studentId;
        ClassId = classId;
        EnrollmentDate = enrollmentDate;
        Status = EnrollmentStatus.Active;
    }

    public void Withdraw()
    {
        if (Status != EnrollmentStatus.Active)
        {
            throw new InvalidOperationException("Only an active enrollment can be withdrawn.");
        }

        Status = EnrollmentStatus.Withdrawn;
    }

    public void Complete()
    {
        if (Status != EnrollmentStatus.Active)
        {
            throw new InvalidOperationException("Only an active enrollment can be completed.");
        }

        Status = EnrollmentStatus.Completed;
    }

    public override string ToString() =>
        $"Enrollment #{EnrollmentId}: Student {StudentId} in Class {ClassId} ({Status})";
}
