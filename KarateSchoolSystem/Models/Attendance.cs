using KarateSchoolSystem.Enums;

namespace KarateSchoolSystem.Models;

public class Attendance
{
    public int AttendanceId { get; private set; }
    public int StudentId { get; private set; }
    public int ClassId { get; private set; }
    public DateTime Date { get; private set; }
    public AttendanceStatus Status { get; private set; }
    public string? Notes { get; private set; }

    public Attendance(int attendanceId, int studentId, int classId, DateTime date, AttendanceStatus status, string? notes = null)
    {
        if (attendanceId <= 0)
        {
            throw new ArgumentException("Attendance ID must be greater than zero.", nameof(attendanceId));
        }

        if (studentId <= 0)
        {
            throw new ArgumentException("Student ID must be greater than zero.", nameof(studentId));
        }

        if (classId <= 0)
        {
            throw new ArgumentException("Class ID must be greater than zero.", nameof(classId));
        }

        if (date.Date > DateTime.Now.Date)
        {
            throw new ArgumentException("Attendance cannot be recorded for a future date.", nameof(date));
        }

        AttendanceId = attendanceId;
        StudentId = studentId;
        ClassId = classId;
        Date = date;
        Status = status;
        Notes = notes;
    }

    public void UpdateNotes(string notes)
    {
        Notes = notes;
    }

    public override string ToString()
    {
        string baseText = $"Attendance #{AttendanceId}: Student {StudentId}, Class {ClassId}, {Date:d}, {Status}";
        return string.IsNullOrWhiteSpace(Notes) ? baseText : $"{baseText} - Notes: {Notes}";
    }
}
