using KarateSchoolSystem.Enums;
using KarateSchoolSystem.Models;

namespace KarateSchoolSystem.Tests.Models;

[TestClass]
public class StudentTests
{
    private static Student CreateAdultStudent(
        int studentId = 1,
        string firstName = "Jane",
        string lastName = "Doe",
        string email = "jane.doe@example.com",
        string password = "password123",
        DateTime? dateOfBirth = null,
        BeltLevel beltLevel = BeltLevel.White,
        DateTime? enrollmentDate = null,
        string? guardianEmail = null)
    {
        return new Student(
            studentId,
            firstName,
            lastName,
            email,
            password,
            dateOfBirth ?? DateTime.Now.AddYears(-30),
            beltLevel,
            enrollmentDate ?? DateTime.Today,
            guardianEmail);
    }

    // ----- Base User validation (exercised through Student) -----

    [TestMethod]
    public void Constructor_UserIdZeroOrNegative_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateAdultStudent(studentId: 0));
        Assert.ThrowsExactly<ArgumentException>(() => CreateAdultStudent(studentId: -1));
    }

    [TestMethod]
    public void Constructor_FirstNameNullOrWhitespace_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateAdultStudent(firstName: ""));
        Assert.ThrowsExactly<ArgumentException>(() => CreateAdultStudent(firstName: "   "));
    }

    [TestMethod]
    public void Constructor_LastNameNullOrWhitespace_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateAdultStudent(lastName: ""));
        Assert.ThrowsExactly<ArgumentException>(() => CreateAdultStudent(lastName: "   "));
    }

    [TestMethod]
    public void Constructor_EmailNullOrInvalidFormat_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateAdultStudent(email: ""));
        Assert.ThrowsExactly<ArgumentException>(() => CreateAdultStudent(email: "not-an-email"));
        Assert.ThrowsExactly<ArgumentException>(() => CreateAdultStudent(email: "missing.dot@example"));
        Assert.ThrowsExactly<ArgumentException>(() => CreateAdultStudent(email: "has space@example.com"));
    }

    [TestMethod]
    public void Constructor_ValidEmail_Succeeds()
    {
        var student = CreateAdultStudent(email: "valid@example.com");
        Assert.AreEqual("valid@example.com", student.Email);
    }

    [TestMethod]
    public void Constructor_PasswordTooShort_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateAdultStudent(password: "short1"));
    }

    [TestMethod]
    public void Constructor_PasswordNullOrWhitespace_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateAdultStudent(password: null!));
        Assert.ThrowsExactly<ArgumentException>(() => CreateAdultStudent(password: ""));
        Assert.ThrowsExactly<ArgumentException>(() => CreateAdultStudent(password: "   "));
    }

    [TestMethod]
    public void Constructor_PasswordExactlyEightChars_Succeeds()
    {
        var student = CreateAdultStudent(password: "eightchr");
        Assert.IsTrue(student.VerifyPassword("eightchr"));
    }

    // ----- Student-specific validation -----

    [TestMethod]
    public void Constructor_DateOfBirthTodayOrFuture_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateAdultStudent(dateOfBirth: DateTime.Today));
        Assert.ThrowsExactly<ArgumentException>(() => CreateAdultStudent(dateOfBirth: DateTime.Now.AddDays(1)));
    }

    [TestMethod]
    public void Constructor_EnrollmentDateInFuture_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() =>
            CreateAdultStudent(enrollmentDate: DateTime.Now.AddDays(1)));
    }

    [TestMethod]
    public void Constructor_EnrollmentDateToday_Succeeds()
    {
        var student = CreateAdultStudent(enrollmentDate: DateTime.Today);
        Assert.AreEqual(DateTime.Today, student.EnrollmentDate);
    }

    [TestMethod]
    public void Constructor_AgeUnderFour_ThrowsArgumentException()
    {
        // One day short of turning 4 years old.
        DateTime dob = DateTime.Now.AddYears(-4).AddDays(1);
        Assert.ThrowsExactly<ArgumentException>(() => CreateAdultStudent(dateOfBirth: dob, guardianEmail: "g@example.com"));
    }

    [TestMethod]
    public void Constructor_AgeExactlyFour_Succeeds()
    {
        DateTime dob = DateTime.Now.AddYears(-4);
        var student = CreateAdultStudent(dateOfBirth: dob, guardianEmail: "g@example.com");
        Assert.AreEqual(4, student.Age);
    }

    [TestMethod]
    public void Constructor_UnderEighteenWithoutGuardianEmail_ThrowsArgumentException()
    {
        DateTime dob = DateTime.Now.AddYears(-17);
        Assert.ThrowsExactly<ArgumentException>(() => CreateAdultStudent(dateOfBirth: dob, guardianEmail: null));
    }

    [TestMethod]
    public void Constructor_UnderEighteenWithGuardianEmail_Succeeds()
    {
        DateTime dob = DateTime.Now.AddYears(-17);
        var student = CreateAdultStudent(dateOfBirth: dob, guardianEmail: "guardian@example.com");
        Assert.AreEqual("guardian@example.com", student.GuardianEmail);
    }

    [TestMethod]
    public void Constructor_ExactlyEighteen_DoesNotRequireGuardianEmail()
    {
        DateTime dob = DateTime.Now.AddYears(-18);
        var student = CreateAdultStudent(dateOfBirth: dob, guardianEmail: null);
        Assert.AreEqual(18, student.Age);
        Assert.IsNull(student.GuardianEmail);
    }

    [TestMethod]
    public void Constructor_ValidAdult_InitializesOutstandingBalanceToZero()
    {
        var student = CreateAdultStudent();
        Assert.AreEqual(0m, student.OutstandingBalance);
    }

    // ----- Promote -----

    [TestMethod]
    public void Promote_ExactlyOneRankUp_Succeeds()
    {
        var student = CreateAdultStudent(beltLevel: BeltLevel.White);
        student.Promote(BeltLevel.Yellow);
        Assert.AreEqual(BeltLevel.Yellow, student.BeltLevel);
    }

    [TestMethod]
    public void Promote_SkippingRank_ThrowsInvalidOperationException()
    {
        var student = CreateAdultStudent(beltLevel: BeltLevel.White);
        Assert.ThrowsExactly<InvalidOperationException>(() => student.Promote(BeltLevel.Orange));
    }

    [TestMethod]
    public void Promote_SameOrLowerRank_ThrowsInvalidOperationException()
    {
        var student = CreateAdultStudent(beltLevel: BeltLevel.Yellow);
        Assert.ThrowsExactly<InvalidOperationException>(() => student.Promote(BeltLevel.Yellow));
        Assert.ThrowsExactly<InvalidOperationException>(() => student.Promote(BeltLevel.White));
    }

    // ----- RecordAttendance / GetAttendanceRate -----

    [TestMethod]
    public void RecordAttendance_NullAttendance_ThrowsArgumentNullException()
    {
        var student = CreateAdultStudent(studentId: 5);
        Assert.ThrowsExactly<ArgumentNullException>(() => student.RecordAttendance(null!));
    }

    [TestMethod]
    public void RecordAttendance_WrongStudentId_ThrowsArgumentException()
    {
        var student = CreateAdultStudent(studentId: 5);
        var attendance = new Attendance(1, 999, 1, DateTime.Today, AttendanceStatus.Present);
        Assert.ThrowsExactly<ArgumentException>(() => student.RecordAttendance(attendance));
    }

    [TestMethod]
    public void RecordAttendance_MatchingStudentId_AddsToHistory()
    {
        var student = CreateAdultStudent(studentId: 5);
        var attendance = new Attendance(1, 5, 1, DateTime.Today, AttendanceStatus.Present);
        student.RecordAttendance(attendance);
        Assert.AreEqual(1, student.AttendanceHistory.Count);
        Assert.AreSame(attendance, student.AttendanceHistory[0]);
    }

    [TestMethod]
    public void GetAttendanceRate_NoRecords_ReturnsZero()
    {
        var student = CreateAdultStudent(studentId: 5);
        Assert.AreEqual(0.0, student.GetAttendanceRate());
    }

    [TestMethod]
    public void GetAttendanceRate_MixOfStatuses_ComputesPresentAndLateRatio()
    {
        var student = CreateAdultStudent(studentId: 5);
        student.RecordAttendance(new Attendance(1, 5, 1, DateTime.Today, AttendanceStatus.Present));
        student.RecordAttendance(new Attendance(2, 5, 1, DateTime.Today, AttendanceStatus.Late));
        student.RecordAttendance(new Attendance(3, 5, 1, DateTime.Today, AttendanceStatus.Absent));
        student.RecordAttendance(new Attendance(4, 5, 1, DateTime.Today, AttendanceStatus.Excused));

        Assert.AreEqual(0.5, student.GetAttendanceRate());
    }

    // ----- ApplyCharge / MakePayment -----

    [TestMethod]
    public void ApplyCharge_NonPositiveAmount_ThrowsArgumentException()
    {
        var student = CreateAdultStudent();
        Assert.ThrowsExactly<ArgumentException>(() => student.ApplyCharge(0m));
        Assert.ThrowsExactly<ArgumentException>(() => student.ApplyCharge(-10m));
    }

    [TestMethod]
    public void ApplyCharge_PositiveAmount_IncreasesBalance()
    {
        var student = CreateAdultStudent();
        student.ApplyCharge(50m);
        Assert.AreEqual(50m, student.OutstandingBalance);
    }

    [TestMethod]
    public void MakePayment_NonPositiveAmount_ThrowsArgumentException()
    {
        var student = CreateAdultStudent();
        student.ApplyCharge(50m);
        Assert.ThrowsExactly<ArgumentException>(() => student.MakePayment(0m));
        Assert.ThrowsExactly<ArgumentException>(() => student.MakePayment(-5m));
    }

    [TestMethod]
    public void MakePayment_ExceedsBalance_ThrowsInvalidOperationException()
    {
        var student = CreateAdultStudent();
        student.ApplyCharge(50m);
        Assert.ThrowsExactly<InvalidOperationException>(() => student.MakePayment(51m));
    }

    [TestMethod]
    public void MakePayment_ValidAmount_DecreasesBalance()
    {
        var student = CreateAdultStudent();
        student.ApplyCharge(50m);
        student.MakePayment(20m);
        Assert.AreEqual(30m, student.OutstandingBalance);
    }

    [TestMethod]
    public void MakePayment_ExactBalance_ZeroesBalance()
    {
        var student = CreateAdultStudent();
        student.ApplyCharge(50m);
        student.MakePayment(50m);
        Assert.AreEqual(0m, student.OutstandingBalance);
    }

    // ----- ReceiveAnnouncement -----

    [TestMethod]
    public void ReceiveAnnouncement_Null_ThrowsArgumentNullException()
    {
        var student = CreateAdultStudent();
        Assert.ThrowsExactly<ArgumentNullException>(() => student.ReceiveAnnouncement(null!));
    }

    [TestMethod]
    public void ReceiveAnnouncement_Valid_AddsToReceivedList()
    {
        var student = CreateAdultStudent();
        var announcement = new Announcement(1, 1, "Title", "Body", DateTime.Today);
        student.ReceiveAnnouncement(announcement);
        Assert.AreEqual(1, student.ReceivedAnnouncements.Count);
        Assert.AreSame(announcement, student.ReceivedAnnouncements[0]);
    }

    // ----- VerifyPassword -----

    [TestMethod]
    public void VerifyPassword_CorrectPassword_ReturnsTrue()
    {
        var student = CreateAdultStudent(password: "correctpw");
        Assert.IsTrue(student.VerifyPassword("correctpw"));
    }

    [TestMethod]
    public void VerifyPassword_IncorrectPassword_ReturnsFalse()
    {
        var student = CreateAdultStudent(password: "correctpw");
        Assert.IsFalse(student.VerifyPassword("wrongpassword"));
    }

    [TestMethod]
    public void VerifyPassword_NullOrEmpty_ReturnsFalse()
    {
        var student = CreateAdultStudent(password: "correctpw");
        Assert.IsFalse(student.VerifyPassword(null!));
        Assert.IsFalse(student.VerifyPassword(""));
    }

    // ----- Activate / Deactivate -----

    [TestMethod]
    public void Deactivate_SetsIsActiveFalse()
    {
        var student = CreateAdultStudent();
        student.Deactivate();
        Assert.IsFalse(student.IsActive);
    }

    [TestMethod]
    public void Activate_SetsIsActiveTrue()
    {
        var student = CreateAdultStudent();
        student.Deactivate();
        student.Activate();
        Assert.IsTrue(student.IsActive);
    }

    // ----- Descriptions / reports -----

    [TestMethod]
    public void GetRoleDescription_ReturnsBeltLevel()
    {
        var student = CreateAdultStudent(beltLevel: BeltLevel.Green);
        Assert.AreEqual("Student (Green Belt)", student.GetRoleDescription());
    }

    [TestMethod]
    public void GenerateReport_ContainsStudentDetails()
    {
        var student = CreateAdultStudent(studentId: 7, beltLevel: BeltLevel.Blue);
        string report = student.GenerateReport();
        Assert.IsTrue(report.Contains("Student ID: 7"));
        Assert.IsTrue(report.Contains("Belt Level: Blue"));
        Assert.IsTrue(report.Contains("Outstanding Balance"));
    }

    [TestMethod]
    public void GenerateReport_NoGuardianEmail_ShowsNotApplicable()
    {
        var student = CreateAdultStudent(guardianEmail: null);
        string report = student.GenerateReport();
        Assert.IsTrue(report.Contains("Guardian Email: N/A"));
    }

    [TestMethod]
    public void GenerateReport_WithGuardianEmail_ShowsGuardianEmail()
    {
        DateTime dob = DateTime.Now.AddYears(-10);
        var student = CreateAdultStudent(dateOfBirth: dob, guardianEmail: "guardian@example.com");
        string report = student.GenerateReport();
        Assert.IsTrue(report.Contains("Guardian Email: guardian@example.com"));
    }

    [TestMethod]
    public void ToString_ReturnsFormattedSummary()
    {
        var student = CreateAdultStudent(studentId: 3, beltLevel: BeltLevel.Black);
        string result = student.ToString();
        Assert.IsTrue(result.Contains("Student #3"));
        Assert.IsTrue(result.Contains("Black Belt"));
    }
}
