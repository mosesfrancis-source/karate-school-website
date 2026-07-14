using System.Text;
using KarateSchoolSystem.Enums;
using KarateSchoolSystem.Interfaces;

namespace KarateSchoolSystem.Models;

public class Student : User, IAttendanceTrackable, IPayable, IAnnouncementObserver
{
    private readonly List<Attendance> _attendanceHistory = new();
    private readonly List<Announcement> _receivedAnnouncements = new();

    public int StudentId { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public BeltLevel BeltLevel { get; private set; }
    public DateTime EnrollmentDate { get; private set; }
    public string? GuardianEmail { get; private set; }
    public decimal OutstandingBalance { get; private set; }

    public IReadOnlyList<Attendance> AttendanceHistory => _attendanceHistory.AsReadOnly();
    public IReadOnlyList<Announcement> ReceivedAnnouncements => _receivedAnnouncements.AsReadOnly();

    public int Age
    {
        get
        {
            int age = DateTime.Now.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > DateTime.Now.AddYears(-age).Date)
            {
                age--;
            }

            return age;
        }
    }

    public Student(
        int studentId,
        string firstName,
        string lastName,
        string email,
        string password,
        DateTime dateOfBirth,
        BeltLevel beltLevel,
        DateTime enrollmentDate,
        string? guardianEmail = null)
        : base(studentId, firstName, lastName, email, password, "Student")
    {
        if (dateOfBirth.Date >= DateTime.Now.Date)
        {
            throw new ArgumentException("Date of birth must be a real date in the past.", nameof(dateOfBirth));
        }

        if (enrollmentDate.Date > DateTime.Now.Date)
        {
            throw new ArgumentException("Enrollment date cannot be in the future.", nameof(enrollmentDate));
        }

        StudentId = studentId;
        DateOfBirth = dateOfBirth;
        BeltLevel = beltLevel;
        EnrollmentDate = enrollmentDate;

        if (Age < 4)
        {
            throw new ArgumentException("Student must be at least 4 years old.", nameof(dateOfBirth));
        }

        if (Age < 18 && string.IsNullOrWhiteSpace(guardianEmail))
        {
            throw new ArgumentException(
                "A guardian email is required for students under 18.", nameof(guardianEmail));
        }

        GuardianEmail = guardianEmail;
        OutstandingBalance = 0m;
    }

    public void Promote(BeltLevel newBeltLevel)
    {
        if ((int)newBeltLevel != (int)BeltLevel + 1)
        {
            throw new InvalidOperationException(
                "Belt promotions must advance exactly one rank at a time; skipping ranks is not allowed.");
        }

        BeltLevel = newBeltLevel;
    }

    public void RecordAttendance(Attendance attendance)
    {
        if (attendance is null)
        {
            throw new ArgumentNullException(nameof(attendance));
        }

        if (attendance.StudentId != StudentId)
        {
            throw new ArgumentException(
                "Attendance record does not belong to this student.", nameof(attendance));
        }

        _attendanceHistory.Add(attendance);
    }

    public double GetAttendanceRate()
    {
        if (_attendanceHistory.Count == 0)
        {
            return 0.0;
        }

        int presentOrLate = _attendanceHistory.Count(a =>
            a.Status == AttendanceStatus.Present || a.Status == AttendanceStatus.Late);

        return (double)presentOrLate / _attendanceHistory.Count;
    }

    public void ApplyCharge(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Charge amount must be positive.", nameof(amount));
        }

        OutstandingBalance += amount;
    }

    public void MakePayment(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Payment amount must be positive.", nameof(amount));
        }

        if (amount > OutstandingBalance)
        {
            throw new InvalidOperationException("Payment amount cannot exceed the outstanding balance.");
        }

        OutstandingBalance -= amount;
    }

    public void ReceiveAnnouncement(Announcement announcement)
    {
        if (announcement is null)
        {
            throw new ArgumentNullException(nameof(announcement));
        }

        _receivedAnnouncements.Add(announcement);
    }

    public override string GetRoleDescription() => $"Student ({BeltLevel} Belt)";

    public override string GenerateReport()
    {
        var sb = new StringBuilder();
        sb.Append(base.GenerateReport());
        sb.AppendLine($"Student ID: {StudentId}");
        sb.AppendLine($"Age: {Age}");
        sb.AppendLine($"Belt Level: {BeltLevel}");
        sb.AppendLine($"Enrollment Date: {EnrollmentDate:d}");
        sb.AppendLine($"Guardian Email: {GuardianEmail ?? "N/A"}");
        sb.AppendLine($"Outstanding Balance: {OutstandingBalance:C}");
        return sb.ToString();
    }

    public override string ToString() =>
        $"Student #{StudentId}: {FullName} ({BeltLevel} Belt, Age {Age})";
}
