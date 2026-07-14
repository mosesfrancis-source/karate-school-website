using KarateSchoolSystem.Models;

namespace KarateSchoolSystem.Tests.Models;

[TestClass]
public class InstructorTests
{
    private static Instructor CreateInstructor(
        int instructorId = 1,
        string firstName = "John",
        string lastName = "Smith",
        string email = "john.smith@example.com",
        string password = "password123",
        string specialization = "Kumite",
        int yearsExperience = 10,
        string certificationLevel = "Level 1")
    {
        return new Instructor(
            instructorId, firstName, lastName, email, password,
            specialization, yearsExperience, certificationLevel);
    }

    [TestMethod]
    public void Constructor_SpecializationNullOrWhitespace_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateInstructor(specialization: ""));
        Assert.ThrowsExactly<ArgumentException>(() => CreateInstructor(specialization: "   "));
    }

    [TestMethod]
    public void Constructor_NegativeYearsExperience_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateInstructor(yearsExperience: -1));
    }

    [TestMethod]
    public void Constructor_ZeroYearsExperience_Succeeds()
    {
        var instructor = CreateInstructor(yearsExperience: 0);
        Assert.AreEqual(0, instructor.YearsExperience);
    }

    [TestMethod]
    public void Constructor_InvalidCertificationLevel_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateInstructor(certificationLevel: "Not A Real Level"));
    }

    [TestMethod]
    public void Constructor_EachAllowedCertificationLevel_Succeeds()
    {
        foreach (var level in new[] { "Level 1", "Level 2", "Level 3", "Black Belt Certified", "Master Certified" })
        {
            var instructor = CreateInstructor(certificationLevel: level);
            Assert.AreEqual(level, instructor.CertificationLevel);
        }
    }

    [TestMethod]
    public void Constructor_ValidArguments_SetsProperties()
    {
        var instructor = CreateInstructor(
            instructorId: 42,
            specialization: "Kata",
            yearsExperience: 5,
            certificationLevel: "Level 2");

        Assert.AreEqual(42, instructor.InstructorId);
        Assert.AreEqual("Kata", instructor.Specialization);
        Assert.AreEqual(5, instructor.YearsExperience);
        Assert.AreEqual("Level 2", instructor.CertificationLevel);
    }

    [TestMethod]
    public void GetRoleDescription_ReturnsCertificationLevel()
    {
        var instructor = CreateInstructor(certificationLevel: "Master Certified");
        Assert.AreEqual("Instructor (Master Certified)", instructor.GetRoleDescription());
    }

    [TestMethod]
    public void GenerateReport_ContainsInstructorDetails()
    {
        var instructor = CreateInstructor(instructorId: 9, specialization: "Self-Defense", yearsExperience: 3);
        string report = instructor.GenerateReport();
        Assert.IsTrue(report.Contains("Instructor ID: 9"));
        Assert.IsTrue(report.Contains("Specialization: Self-Defense"));
        Assert.IsTrue(report.Contains("Years Experience: 3"));
    }

    [TestMethod]
    public void ToString_ReturnsFormattedSummary()
    {
        var instructor = CreateInstructor(instructorId: 9, specialization: "Self-Defense", certificationLevel: "Level 3");
        string result = instructor.ToString();
        Assert.IsTrue(result.Contains("Instructor #9"));
        Assert.IsTrue(result.Contains("Self-Defense"));
        Assert.IsTrue(result.Contains("Level 3"));
    }
}
