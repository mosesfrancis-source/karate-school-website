using KarateSchoolSystem.Enums;
using KarateSchoolSystem.Models;

namespace KarateSchoolSystem.Tests.Models;

[TestClass]
public class AttendanceTests
{
    private static Attendance CreateAttendance(
        int attendanceId = 1,
        int studentId = 1,
        int classId = 1,
        DateTime? date = null,
        AttendanceStatus status = AttendanceStatus.Present,
        string? notes = null)
    {
        return new Attendance(attendanceId, studentId, classId, date ?? DateTime.Today, status, notes);
    }

    [TestMethod]
    public void Constructor_AttendanceIdZeroOrNegative_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateAttendance(attendanceId: 0));
        Assert.ThrowsExactly<ArgumentException>(() => CreateAttendance(attendanceId: -1));
    }

    [TestMethod]
    public void Constructor_StudentIdZeroOrNegative_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateAttendance(studentId: 0));
        Assert.ThrowsExactly<ArgumentException>(() => CreateAttendance(studentId: -1));
    }

    [TestMethod]
    public void Constructor_ClassIdZeroOrNegative_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateAttendance(classId: 0));
        Assert.ThrowsExactly<ArgumentException>(() => CreateAttendance(classId: -1));
    }

    [TestMethod]
    public void Constructor_FutureDate_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateAttendance(date: DateTime.Now.AddDays(1)));
    }

    [TestMethod]
    public void Constructor_TodayDate_Succeeds()
    {
        var attendance = CreateAttendance(date: DateTime.Today);
        Assert.AreEqual(DateTime.Today, attendance.Date);
    }

    [TestMethod]
    public void Constructor_ValidArguments_SetsProperties()
    {
        var attendance = CreateAttendance(
            attendanceId: 2, studentId: 3, classId: 4, status: AttendanceStatus.Late, notes: "traffic");
        Assert.AreEqual(2, attendance.AttendanceId);
        Assert.AreEqual(3, attendance.StudentId);
        Assert.AreEqual(4, attendance.ClassId);
        Assert.AreEqual(AttendanceStatus.Late, attendance.Status);
        Assert.AreEqual("traffic", attendance.Notes);
    }

    [TestMethod]
    public void UpdateNotes_SetsNotes()
    {
        var attendance = CreateAttendance();
        attendance.UpdateNotes("updated note");
        Assert.AreEqual("updated note", attendance.Notes);
    }

    [TestMethod]
    public void ToString_WithoutNotes_DoesNotIncludeNotesSuffix()
    {
        var attendance = CreateAttendance(notes: null);
        string result = attendance.ToString();
        Assert.IsFalse(result.Contains("Notes:"));
    }

    [TestMethod]
    public void ToString_WithWhitespaceNotes_DoesNotIncludeNotesSuffix()
    {
        var attendance = CreateAttendance(notes: "   ");
        string result = attendance.ToString();
        Assert.IsFalse(result.Contains("Notes:"));
    }

    [TestMethod]
    public void ToString_WithNotes_IncludesNotesSuffix()
    {
        var attendance = CreateAttendance(notes: "left early");
        string result = attendance.ToString();
        Assert.IsTrue(result.Contains("Notes: left early"));
    }
}
