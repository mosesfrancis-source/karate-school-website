using KarateSchoolSystem.Enums;
using KarateSchoolSystem.Exceptions;
using KarateSchoolSystem.Models;

namespace KarateSchoolSystem.Tests.Models;

[TestClass]
public class KarateClassTests
{
    private static KarateClass CreateClass(
        int classId = 1,
        int instructorId = 1,
        string className = "Beginner Kata",
        DateTime? schedule = null,
        string location = "Main Dojo",
        int capacity = 2,
        BeltLevel beltLevelRequired = BeltLevel.White)
    {
        return new KarateClass(
            classId, instructorId, className, schedule ?? DateTime.Today, location, capacity, beltLevelRequired);
    }

    [TestMethod]
    public void Constructor_ClassIdZeroOrNegative_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateClass(classId: 0));
        Assert.ThrowsExactly<ArgumentException>(() => CreateClass(classId: -1));
    }

    [TestMethod]
    public void Constructor_InstructorIdZeroOrNegative_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateClass(instructorId: 0));
        Assert.ThrowsExactly<ArgumentException>(() => CreateClass(instructorId: -1));
    }

    [TestMethod]
    public void Constructor_ClassNameNullOrWhitespace_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateClass(className: ""));
        Assert.ThrowsExactly<ArgumentException>(() => CreateClass(className: "   "));
    }

    [TestMethod]
    public void Constructor_LocationNullOrWhitespace_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateClass(location: ""));
        Assert.ThrowsExactly<ArgumentException>(() => CreateClass(location: "   "));
    }

    [TestMethod]
    public void Constructor_CapacityZeroOrNegative_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateClass(capacity: 0));
        Assert.ThrowsExactly<ArgumentException>(() => CreateClass(capacity: -1));
    }

    [TestMethod]
    public void Constructor_ValidArguments_SetsProperties()
    {
        var karateClass = CreateClass(classId: 7, capacity: 10, beltLevelRequired: BeltLevel.Green);
        Assert.AreEqual(7, karateClass.ClassId);
        Assert.AreEqual(10, karateClass.Capacity);
        Assert.AreEqual(BeltLevel.Green, karateClass.BeltLevelRequired);
        Assert.AreEqual(0, karateClass.CurrentEnrollment);
        Assert.IsTrue(karateClass.HasAvailableCapacity);
    }

    [TestMethod]
    public void EnrollStudent_AlreadyEnrolled_ThrowsInvalidOperationException()
    {
        var karateClass = CreateClass(capacity: 5);
        karateClass.EnrollStudent(1);
        Assert.ThrowsExactly<InvalidOperationException>(() => karateClass.EnrollStudent(1));
    }

    [TestMethod]
    public void EnrollStudent_AtCapacity_ThrowsClassCapacityExceededException()
    {
        var karateClass = CreateClass(capacity: 1);
        karateClass.EnrollStudent(1);
        Assert.ThrowsExactly<ClassCapacityExceededException>(() => karateClass.EnrollStudent(2));
    }

    [TestMethod]
    public void EnrollStudent_WithinCapacity_IncrementsEnrollment()
    {
        var karateClass = CreateClass(capacity: 2);
        karateClass.EnrollStudent(1);
        Assert.AreEqual(1, karateClass.CurrentEnrollment);
        Assert.IsTrue(karateClass.IsStudentEnrolled(1));
        Assert.IsTrue(karateClass.HasAvailableCapacity);
    }

    [TestMethod]
    public void HasAvailableCapacity_WhenFull_ReturnsFalse()
    {
        var karateClass = CreateClass(capacity: 1);
        karateClass.EnrollStudent(1);
        Assert.IsFalse(karateClass.HasAvailableCapacity);
    }

    [TestMethod]
    public void WithdrawStudent_NotEnrolled_ThrowsInvalidOperationException()
    {
        var karateClass = CreateClass();
        Assert.ThrowsExactly<InvalidOperationException>(() => karateClass.WithdrawStudent(1));
    }

    [TestMethod]
    public void WithdrawStudent_Enrolled_RemovesStudent()
    {
        var karateClass = CreateClass();
        karateClass.EnrollStudent(1);
        karateClass.WithdrawStudent(1);
        Assert.IsFalse(karateClass.IsStudentEnrolled(1));
        Assert.AreEqual(0, karateClass.CurrentEnrollment);
    }

    [TestMethod]
    public void IsStudentEnrolled_NotEnrolled_ReturnsFalse()
    {
        var karateClass = CreateClass();
        Assert.IsFalse(karateClass.IsStudentEnrolled(1));
    }

    [TestMethod]
    public void GenerateReport_ContainsClassDetails()
    {
        var karateClass = CreateClass(classId: 3, className: "Advanced Sparring", capacity: 4);
        string report = karateClass.GenerateReport();
        Assert.IsTrue(report.Contains("Class ID: 3"));
        Assert.IsTrue(report.Contains("Advanced Sparring"));
        Assert.IsTrue(report.Contains("Enrollment: 0/4"));
    }

    [TestMethod]
    public void ToString_ReturnsFormattedSummary()
    {
        var karateClass = CreateClass(classId: 3, className: "Advanced Sparring", capacity: 4);
        string result = karateClass.ToString();
        Assert.IsTrue(result.Contains("Class #3"));
        Assert.IsTrue(result.Contains("Advanced Sparring"));
        Assert.IsTrue(result.Contains("0/4"));
    }
}
