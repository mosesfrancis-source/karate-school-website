using KarateSchoolSystem.Models;

namespace KarateSchoolSystem.Tests.Models;

[TestClass]
public class AdministratorTests
{
    private static Administrator CreateAdministrator(
        int adminId = 1,
        string firstName = "Alice",
        string lastName = "Admin",
        string email = "alice.admin@example.com",
        string password = "password123",
        string department = "Operations",
        string accessLevel = "Standard")
    {
        return new Administrator(adminId, firstName, lastName, email, password, department, accessLevel);
    }

    [TestMethod]
    public void Constructor_DepartmentNullOrWhitespace_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateAdministrator(department: ""));
        Assert.ThrowsExactly<ArgumentException>(() => CreateAdministrator(department: "   "));
    }

    [TestMethod]
    public void Constructor_InvalidAccessLevel_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateAdministrator(accessLevel: "SuperUser"));
    }

    [TestMethod]
    public void Constructor_EachAllowedAccessLevel_Succeeds()
    {
        foreach (var level in new[] { "Standard", "Elevated", "SuperAdmin" })
        {
            var admin = CreateAdministrator(accessLevel: level);
            Assert.AreEqual(level, admin.AccessLevel);
        }
    }

    [TestMethod]
    public void Constructor_ValidArguments_SetsProperties()
    {
        var admin = CreateAdministrator(adminId: 3, department: "Finance", accessLevel: "Elevated");
        Assert.AreEqual(3, admin.AdminId);
        Assert.AreEqual("Finance", admin.Department);
        Assert.AreEqual("Elevated", admin.AccessLevel);
    }

    [TestMethod]
    public void CanApproveActions_StandardAccess_ReturnsFalse()
    {
        var admin = CreateAdministrator(accessLevel: "Standard");
        Assert.IsFalse(admin.CanApproveActions);
    }

    [TestMethod]
    public void CanApproveActions_ElevatedAccess_ReturnsTrue()
    {
        var admin = CreateAdministrator(accessLevel: "Elevated");
        Assert.IsTrue(admin.CanApproveActions);
    }

    [TestMethod]
    public void CanApproveActions_SuperAdminAccess_ReturnsTrue()
    {
        var admin = CreateAdministrator(accessLevel: "SuperAdmin");
        Assert.IsTrue(admin.CanApproveActions);
    }

    [TestMethod]
    public void GetRoleDescription_ReturnsAccessLevel()
    {
        var admin = CreateAdministrator(accessLevel: "SuperAdmin");
        Assert.AreEqual("Administrator (SuperAdmin)", admin.GetRoleDescription());
    }

    [TestMethod]
    public void GenerateReport_ContainsAdministratorDetails()
    {
        var admin = CreateAdministrator(adminId: 5, department: "HR", accessLevel: "Elevated");
        string report = admin.GenerateReport();
        Assert.IsTrue(report.Contains("Admin ID: 5"));
        Assert.IsTrue(report.Contains("Department: HR"));
        Assert.IsTrue(report.Contains("Access Level: Elevated"));
    }

    [TestMethod]
    public void ToString_ReturnsFormattedSummary()
    {
        var admin = CreateAdministrator(adminId: 5, department: "HR", accessLevel: "Elevated");
        string result = admin.ToString();
        Assert.IsTrue(result.Contains("Administrator #5"));
        Assert.IsTrue(result.Contains("HR"));
        Assert.IsTrue(result.Contains("Elevated"));
    }
}
