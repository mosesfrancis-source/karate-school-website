using KarateSchoolSystem.Enums;
using KarateSchoolSystem.Models;

namespace KarateSchoolSystem.Tests.Models;

[TestClass]
public class StudentClassEnrollmentTests
{
    private static StudentClassEnrollment CreateEnrollment(
        int enrollmentId = 1,
        int studentId = 1,
        int classId = 1,
        DateTime? enrollmentDate = null)
    {
        return new StudentClassEnrollment(enrollmentId, studentId, classId, enrollmentDate ?? DateTime.Today);
    }

    [TestMethod]
    public void Constructor_EnrollmentIdZeroOrNegative_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateEnrollment(enrollmentId: 0));
        Assert.ThrowsExactly<ArgumentException>(() => CreateEnrollment(enrollmentId: -1));
    }

    [TestMethod]
    public void Constructor_StudentIdZeroOrNegative_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateEnrollment(studentId: 0));
        Assert.ThrowsExactly<ArgumentException>(() => CreateEnrollment(studentId: -1));
    }

    [TestMethod]
    public void Constructor_ClassIdZeroOrNegative_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateEnrollment(classId: 0));
        Assert.ThrowsExactly<ArgumentException>(() => CreateEnrollment(classId: -1));
    }

    [TestMethod]
    public void Constructor_ValidArguments_SetsPropertiesAndStatusActive()
    {
        var enrollment = CreateEnrollment(enrollmentId: 2, studentId: 3, classId: 4);
        Assert.AreEqual(2, enrollment.EnrollmentId);
        Assert.AreEqual(3, enrollment.StudentId);
        Assert.AreEqual(4, enrollment.ClassId);
        Assert.AreEqual(EnrollmentStatus.Active, enrollment.Status);
    }

    [TestMethod]
    public void Withdraw_WhenActive_SetsStatusWithdrawn()
    {
        var enrollment = CreateEnrollment();
        enrollment.Withdraw();
        Assert.AreEqual(EnrollmentStatus.Withdrawn, enrollment.Status);
    }

    [TestMethod]
    public void Withdraw_WhenNotActive_ThrowsInvalidOperationException()
    {
        var enrollment = CreateEnrollment();
        enrollment.Withdraw();
        Assert.ThrowsExactly<InvalidOperationException>(() => enrollment.Withdraw());
    }

    [TestMethod]
    public void Complete_WhenActive_SetsStatusCompleted()
    {
        var enrollment = CreateEnrollment();
        enrollment.Complete();
        Assert.AreEqual(EnrollmentStatus.Completed, enrollment.Status);
    }

    [TestMethod]
    public void Complete_WhenNotActive_ThrowsInvalidOperationException()
    {
        var enrollment = CreateEnrollment();
        enrollment.Complete();
        Assert.ThrowsExactly<InvalidOperationException>(() => enrollment.Complete());
    }

    [TestMethod]
    public void Complete_AfterWithdrawn_ThrowsInvalidOperationException()
    {
        var enrollment = CreateEnrollment();
        enrollment.Withdraw();
        Assert.ThrowsExactly<InvalidOperationException>(() => enrollment.Complete());
    }

    [TestMethod]
    public void ToString_ReturnsFormattedSummary()
    {
        var enrollment = CreateEnrollment(enrollmentId: 9, studentId: 1, classId: 2);
        string result = enrollment.ToString();
        Assert.IsTrue(result.Contains("Enrollment #9"));
        Assert.IsTrue(result.Contains("Student 1"));
        Assert.IsTrue(result.Contains("Class 2"));
        Assert.IsTrue(result.Contains("Active"));
    }
}
