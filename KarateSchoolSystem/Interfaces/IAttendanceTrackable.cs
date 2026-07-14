using KarateSchoolSystem.Models;

namespace KarateSchoolSystem.Interfaces;

/// <summary>
/// Implemented by entities that keep a history of attendance records, such as Student.
/// </summary>
public interface IAttendanceTrackable
{
    IReadOnlyList<Attendance> AttendanceHistory { get; }
    void RecordAttendance(Attendance attendance);
    double GetAttendanceRate();
}
