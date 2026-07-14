using KarateSchoolSystem.Enums;
using KarateSchoolSystem.Patterns;

namespace KarateSchoolSystem.Tests.Patterns;

[TestClass]
public class UserFactoryTests
{
    [TestMethod]
    public void CreateStudent_ValidArguments_ReturnsStudent()
    {
        var student = UserFactory.CreateStudent(
            1, "Jane", "Doe", "jane@example.com", "password123",
            DateTime.Now.AddYears(-30), BeltLevel.White, DateTime.Today);

        Assert.IsInstanceOfType<KarateSchoolSystem.Models.Student>(student);
        Assert.AreEqual(1, student.StudentId);
    }

    [TestMethod]
    public void CreateStudent_InvalidArguments_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => UserFactory.CreateStudent(
            0, "Jane", "Doe", "jane@example.com", "password123",
            DateTime.Now.AddYears(-30), BeltLevel.White, DateTime.Today));
    }

    [TestMethod]
    public void CreateInstructor_ValidArguments_ReturnsInstructor()
    {
        var instructor = UserFactory.CreateInstructor(
            1, "John", "Smith", "john@example.com", "password123",
            "Kata", 5, "Level 1");

        Assert.IsInstanceOfType<KarateSchoolSystem.Models.Instructor>(instructor);
        Assert.AreEqual(1, instructor.InstructorId);
    }

    [TestMethod]
    public void CreateInstructor_InvalidArguments_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => UserFactory.CreateInstructor(
            1, "John", "Smith", "john@example.com", "password123",
            "", 5, "Level 1"));
    }

    [TestMethod]
    public void CreateAdministrator_ValidArguments_ReturnsAdministrator()
    {
        var admin = UserFactory.CreateAdministrator(
            1, "Alice", "Admin", "alice@example.com", "password123",
            "Operations", "Standard");

        Assert.IsInstanceOfType<KarateSchoolSystem.Models.Administrator>(admin);
        Assert.AreEqual(1, admin.AdminId);
    }

    [TestMethod]
    public void CreateAdministrator_InvalidArguments_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => UserFactory.CreateAdministrator(
            1, "Alice", "Admin", "alice@example.com", "password123",
            "Operations", "NotALevel"));
    }
}
