using KarateSchoolSystem.Enums;
using KarateSchoolSystem.Exceptions;
using KarateSchoolSystem.Management;
using KarateSchoolSystem.Models;

namespace KarateSchoolSystem.Tests.Management;

[TestClass]
public class SchoolManagerTests
{
    private static Student CreateStudent(
        int studentId,
        BeltLevel beltLevel = BeltLevel.White,
        string email = "student@example.com") =>
        new(studentId, "Stu", "Dent", email, "password123",
            DateTime.Now.AddYears(-30), beltLevel, DateTime.Today);

    private static Instructor CreateInstructor(int instructorId, string email = "instructor@example.com") =>
        new(instructorId, "In", "Structor", email, "password123",
            "Kata", 5, "Level 1");

    private static Administrator CreateAdministrator(
        int adminId, string accessLevel = "Elevated", string email = "admin@example.com") =>
        new(adminId, "Ad", "Min", email, "password123", "Operations", accessLevel);

    private static KarateClass CreateClass(
        int classId, int instructorId, BeltLevel beltLevelRequired = BeltLevel.White, int capacity = 5) =>
        new(classId, instructorId, "Test Class", DateTime.Today, "Main Dojo", capacity, beltLevelRequired);

    // ----- RegisterStudent -----

    [TestMethod]
    public void RegisterStudent_NewStudent_AddsToAllStudents()
    {
        var manager = new SchoolManager();
        var student = CreateStudent(1);
        manager.RegisterStudent(student);

        Assert.AreEqual(1, manager.AllStudents.Count);
        Assert.AreSame(student, manager.GetStudent(1));
    }

    [TestMethod]
    public void RegisterStudent_DuplicateId_ThrowsDuplicateUserException()
    {
        var manager = new SchoolManager();
        manager.RegisterStudent(CreateStudent(1));
        Assert.ThrowsExactly<DuplicateUserException>(() => manager.RegisterStudent(CreateStudent(1, email: "other@example.com")));
    }

    // ----- RegisterInstructor -----

    [TestMethod]
    public void RegisterInstructor_NewInstructor_AddsToAllInstructors()
    {
        var manager = new SchoolManager();
        var instructor = CreateInstructor(1);
        manager.RegisterInstructor(instructor);

        Assert.AreEqual(1, manager.AllInstructors.Count);
        Assert.AreSame(instructor, manager.GetInstructor(1));
    }

    [TestMethod]
    public void RegisterInstructor_DuplicateId_ThrowsDuplicateUserException()
    {
        var manager = new SchoolManager();
        manager.RegisterInstructor(CreateInstructor(1));
        Assert.ThrowsExactly<DuplicateUserException>(() => manager.RegisterInstructor(CreateInstructor(1, "other@example.com")));
    }

    // ----- RegisterAdministrator -----

    [TestMethod]
    public void RegisterAdministrator_NewAdministrator_AddsToAllAdministrators()
    {
        var manager = new SchoolManager();
        var admin = CreateAdministrator(1);
        manager.RegisterAdministrator(admin);

        Assert.AreEqual(1, manager.AllAdministrators.Count);
        Assert.AreSame(admin, manager.GetAdministrator(1));
    }

    [TestMethod]
    public void RegisterAdministrator_DuplicateId_ThrowsDuplicateUserException()
    {
        var manager = new SchoolManager();
        manager.RegisterAdministrator(CreateAdministrator(1));
        Assert.ThrowsExactly<DuplicateUserException>(() => manager.RegisterAdministrator(CreateAdministrator(1, email: "other@example.com")));
    }

    // ----- AddClass -----

    [TestMethod]
    public void AddClass_DuplicateId_ThrowsDuplicateUserException()
    {
        var manager = new SchoolManager();
        manager.RegisterInstructor(CreateInstructor(1));
        manager.AddClass(CreateClass(1, 1));

        Assert.ThrowsExactly<DuplicateUserException>(() => manager.AddClass(CreateClass(1, 1)));
    }

    [TestMethod]
    public void AddClass_UnregisteredInstructor_ThrowsInvalidOperationException()
    {
        var manager = new SchoolManager();
        Assert.ThrowsExactly<InvalidOperationException>(() => manager.AddClass(CreateClass(1, 99)));
    }

    [TestMethod]
    public void AddClass_ValidClass_AddsToAllClasses()
    {
        var manager = new SchoolManager();
        manager.RegisterInstructor(CreateInstructor(1));
        var karateClass = CreateClass(1, 1);
        manager.AddClass(karateClass);

        Assert.AreEqual(1, manager.AllClasses.Count);
        Assert.AreSame(karateClass, manager.GetClass(1));
    }

    // ----- EnrollStudentInClass -----

    [TestMethod]
    public void EnrollStudentInClass_StudentNotFound_ThrowsInvalidOperationException()
    {
        var manager = new SchoolManager();
        manager.RegisterInstructor(CreateInstructor(1));
        manager.AddClass(CreateClass(1, 1));

        Assert.ThrowsExactly<InvalidOperationException>(() =>
            manager.EnrollStudentInClass(1, 999, 1, DateTime.Today));
    }

    [TestMethod]
    public void EnrollStudentInClass_ClassNotFound_ThrowsInvalidOperationException()
    {
        var manager = new SchoolManager();
        manager.RegisterStudent(CreateStudent(1));

        Assert.ThrowsExactly<InvalidOperationException>(() =>
            manager.EnrollStudentInClass(1, 1, 999, DateTime.Today));
    }

    [TestMethod]
    public void EnrollStudentInClass_BeltLevelTooLow_ThrowsInvalidOperationException()
    {
        var manager = new SchoolManager();
        manager.RegisterStudent(CreateStudent(1, BeltLevel.White));
        manager.RegisterInstructor(CreateInstructor(1));
        manager.AddClass(CreateClass(1, 1, BeltLevel.Green));

        Assert.ThrowsExactly<InvalidOperationException>(() =>
            manager.EnrollStudentInClass(1, 1, 1, DateTime.Today));
    }

    [TestMethod]
    public void EnrollStudentInClass_MeetsBeltRequirement_CreatesEnrollmentAndEnrollsInClass()
    {
        var manager = new SchoolManager();
        manager.RegisterStudent(CreateStudent(1, BeltLevel.Green));
        manager.RegisterInstructor(CreateInstructor(1));
        var karateClass = CreateClass(1, 1, BeltLevel.Green);
        manager.AddClass(karateClass);

        var enrollment = manager.EnrollStudentInClass(1, 1, 1, DateTime.Today);

        Assert.AreEqual(1, enrollment.StudentId);
        Assert.AreEqual(1, enrollment.ClassId);
        Assert.IsTrue(karateClass.IsStudentEnrolled(1));
        Assert.AreEqual(1, manager.AllEnrollments.Count);
    }

    // ----- RecordAttendance -----

    [TestMethod]
    public void RecordAttendance_StudentNotFound_ThrowsInvalidOperationException()
    {
        var manager = new SchoolManager();
        manager.RegisterInstructor(CreateInstructor(1));
        manager.AddClass(CreateClass(1, 1));

        Assert.ThrowsExactly<InvalidOperationException>(() =>
            manager.RecordAttendance(1, 999, 1, DateTime.Today, AttendanceStatus.Present));
    }

    [TestMethod]
    public void RecordAttendance_ClassNotFound_ThrowsInvalidOperationException()
    {
        var manager = new SchoolManager();
        manager.RegisterStudent(CreateStudent(1));

        Assert.ThrowsExactly<InvalidOperationException>(() =>
            manager.RecordAttendance(1, 1, 999, DateTime.Today, AttendanceStatus.Present));
    }

    [TestMethod]
    public void RecordAttendance_StudentNotEnrolled_ThrowsInvalidOperationException()
    {
        var manager = new SchoolManager();
        manager.RegisterStudent(CreateStudent(1));
        manager.RegisterInstructor(CreateInstructor(1));
        manager.AddClass(CreateClass(1, 1));

        Assert.ThrowsExactly<InvalidOperationException>(() =>
            manager.RecordAttendance(1, 1, 1, DateTime.Today, AttendanceStatus.Present));
    }

    [TestMethod]
    public void RecordAttendance_StudentEnrolled_AddsAttendanceToStudentHistory()
    {
        var manager = new SchoolManager();
        var student = CreateStudent(1);
        manager.RegisterStudent(student);
        manager.RegisterInstructor(CreateInstructor(1));
        manager.AddClass(CreateClass(1, 1));
        manager.EnrollStudentInClass(1, 1, 1, DateTime.Today);

        var attendance = manager.RecordAttendance(1, 1, 1, DateTime.Today, AttendanceStatus.Present, "on time");

        Assert.AreEqual(1, student.AttendanceHistory.Count);
        Assert.AreSame(attendance, student.AttendanceHistory[0]);
    }

    // ----- SubmitBeltPromotion -----

    [TestMethod]
    public void SubmitBeltPromotion_StudentNotFound_ThrowsInvalidOperationException()
    {
        var manager = new SchoolManager();
        manager.RegisterInstructor(CreateInstructor(1));

        Assert.ThrowsExactly<InvalidOperationException>(() =>
            manager.SubmitBeltPromotion(1, 999, 1, BeltLevel.Yellow, DateTime.Today));
    }

    [TestMethod]
    public void SubmitBeltPromotion_InstructorNotRegistered_ThrowsInvalidOperationException()
    {
        var manager = new SchoolManager();
        manager.RegisterStudent(CreateStudent(1, BeltLevel.White));

        Assert.ThrowsExactly<InvalidOperationException>(() =>
            manager.SubmitBeltPromotion(1, 1, 999, BeltLevel.Yellow, DateTime.Today));
    }

    [TestMethod]
    public void SubmitBeltPromotion_ValidRequest_AddsToAllPromotions()
    {
        var manager = new SchoolManager();
        manager.RegisterStudent(CreateStudent(1, BeltLevel.White));
        manager.RegisterInstructor(CreateInstructor(1));

        var promotion = manager.SubmitBeltPromotion(1, 1, 1, BeltLevel.Yellow, DateTime.Today);

        Assert.AreEqual(1, manager.AllPromotions.Count);
        Assert.AreSame(promotion, manager.GetPromotion(1));
        Assert.AreEqual(PromotionStatus.Pending, promotion.Status);
    }

    // ----- ApproveBeltPromotion -----

    [TestMethod]
    public void ApproveBeltPromotion_PromotionNotFound_ThrowsInvalidOperationException()
    {
        var manager = new SchoolManager();
        manager.RegisterAdministrator(CreateAdministrator(1, "Elevated"));

        Assert.ThrowsExactly<InvalidOperationException>(() => manager.ApproveBeltPromotion(999, 1));
    }

    [TestMethod]
    public void ApproveBeltPromotion_AdminNotFound_ThrowsInvalidOperationException()
    {
        var manager = new SchoolManager();
        manager.RegisterStudent(CreateStudent(1, BeltLevel.White));
        manager.RegisterInstructor(CreateInstructor(1));
        manager.SubmitBeltPromotion(1, 1, 1, BeltLevel.Yellow, DateTime.Today);

        Assert.ThrowsExactly<InvalidOperationException>(() => manager.ApproveBeltPromotion(1, 999));
    }

    [TestMethod]
    public void ApproveBeltPromotion_StandardAccessAdmin_ThrowsUnauthorizedActionException()
    {
        var manager = new SchoolManager();
        manager.RegisterStudent(CreateStudent(1, BeltLevel.White));
        manager.RegisterInstructor(CreateInstructor(1));
        manager.RegisterAdministrator(CreateAdministrator(1, "Standard"));
        manager.SubmitBeltPromotion(1, 1, 1, BeltLevel.Yellow, DateTime.Today);

        Assert.ThrowsExactly<UnauthorizedActionException>(() => manager.ApproveBeltPromotion(1, 1));
    }

    [TestMethod]
    public void ApproveBeltPromotion_ElevatedAdmin_ApprovesAndPromotesStudent()
    {
        var manager = new SchoolManager();
        var student = CreateStudent(1, BeltLevel.White);
        manager.RegisterStudent(student);
        manager.RegisterInstructor(CreateInstructor(1));
        manager.RegisterAdministrator(CreateAdministrator(1, "Elevated"));
        var promotion = manager.SubmitBeltPromotion(1, 1, 1, BeltLevel.Yellow, DateTime.Today);

        manager.ApproveBeltPromotion(1, 1);

        Assert.AreEqual(PromotionStatus.Approved, promotion.Status);
        Assert.AreEqual(BeltLevel.Yellow, student.BeltLevel);
    }

    // ----- DenyBeltPromotion -----

    [TestMethod]
    public void DenyBeltPromotion_PromotionNotFound_ThrowsInvalidOperationException()
    {
        var manager = new SchoolManager();
        manager.RegisterAdministrator(CreateAdministrator(1, "Elevated"));

        Assert.ThrowsExactly<InvalidOperationException>(() => manager.DenyBeltPromotion(999, 1, "reason"));
    }

    [TestMethod]
    public void DenyBeltPromotion_AdminNotFound_ThrowsInvalidOperationException()
    {
        var manager = new SchoolManager();
        manager.RegisterStudent(CreateStudent(1, BeltLevel.White));
        manager.RegisterInstructor(CreateInstructor(1));
        manager.SubmitBeltPromotion(1, 1, 1, BeltLevel.Yellow, DateTime.Today);

        Assert.ThrowsExactly<InvalidOperationException>(() => manager.DenyBeltPromotion(1, 999, "reason"));
    }

    [TestMethod]
    public void DenyBeltPromotion_StandardAccessAdmin_ThrowsUnauthorizedActionException()
    {
        var manager = new SchoolManager();
        manager.RegisterStudent(CreateStudent(1, BeltLevel.White));
        manager.RegisterInstructor(CreateInstructor(1));
        manager.RegisterAdministrator(CreateAdministrator(1, "Standard"));
        manager.SubmitBeltPromotion(1, 1, 1, BeltLevel.Yellow, DateTime.Today);

        Assert.ThrowsExactly<UnauthorizedActionException>(() => manager.DenyBeltPromotion(1, 1, "reason"));
    }

    [TestMethod]
    public void DenyBeltPromotion_ElevatedAdmin_DeniesPromotion()
    {
        var manager = new SchoolManager();
        manager.RegisterStudent(CreateStudent(1, BeltLevel.White));
        manager.RegisterInstructor(CreateInstructor(1));
        manager.RegisterAdministrator(CreateAdministrator(1, "Elevated"));
        var promotion = manager.SubmitBeltPromotion(1, 1, 1, BeltLevel.Yellow, DateTime.Today);

        manager.DenyBeltPromotion(1, 1, "not ready");

        Assert.AreEqual(PromotionStatus.Denied, promotion.Status);
    }

    // ----- ChargeStudent -----

    [TestMethod]
    public void ChargeStudent_StudentNotFound_ThrowsInvalidOperationException()
    {
        var manager = new SchoolManager();
        Assert.ThrowsExactly<InvalidOperationException>(() => manager.ChargeStudent(999, 50m));
    }

    [TestMethod]
    public void ChargeStudent_ValidStudent_AppliesCharge()
    {
        var manager = new SchoolManager();
        var student = CreateStudent(1);
        manager.RegisterStudent(student);

        manager.ChargeStudent(1, 50m);

        Assert.AreEqual(50m, student.OutstandingBalance);
    }

    // ----- ProcessPayment -----

    [TestMethod]
    public void ProcessPayment_StudentNotFound_ThrowsInvalidOperationException()
    {
        var manager = new SchoolManager();
        manager.RegisterAdministrator(CreateAdministrator(1, "Elevated"));

        Assert.ThrowsExactly<InvalidOperationException>(() =>
            manager.ProcessPayment(1, 999, 1, 50m, PaymentMethod.Cash));
    }

    [TestMethod]
    public void ProcessPayment_AdminNotFound_ThrowsInvalidOperationException()
    {
        var manager = new SchoolManager();
        manager.RegisterStudent(CreateStudent(1));

        Assert.ThrowsExactly<InvalidOperationException>(() =>
            manager.ProcessPayment(1, 1, 999, 50m, PaymentMethod.Cash));
    }

    [TestMethod]
    public void ProcessPayment_StandardAccessAdmin_ThrowsUnauthorizedActionException()
    {
        var manager = new SchoolManager();
        var student = CreateStudent(1);
        manager.RegisterStudent(student);
        manager.ChargeStudent(1, 50m);
        manager.RegisterAdministrator(CreateAdministrator(1, "Standard"));

        Assert.ThrowsExactly<UnauthorizedActionException>(() =>
            manager.ProcessPayment(1, 1, 1, 50m, PaymentMethod.Cash));
    }

    [TestMethod]
    public void ProcessPayment_ElevatedAdmin_RecordsPaymentAndReducesBalance()
    {
        var manager = new SchoolManager();
        var student = CreateStudent(1);
        manager.RegisterStudent(student);
        manager.ChargeStudent(1, 50m);
        manager.RegisterAdministrator(CreateAdministrator(1, "Elevated"));

        var (payment, confirmation) = manager.ProcessPayment(1, 1, 1, 50m, PaymentMethod.Cash, "monthly fee");

        Assert.AreEqual(0m, student.OutstandingBalance);
        Assert.AreEqual(1, manager.AllPayments.Count);
        Assert.AreSame(payment, manager.AllPayments[0]);
        Assert.IsTrue(confirmation.Contains("Cash payment"));
    }

    // ----- PublishAnnouncement -----

    [TestMethod]
    public void PublishAnnouncement_AdminNotFound_ThrowsInvalidOperationException()
    {
        var manager = new SchoolManager();
        Assert.ThrowsExactly<InvalidOperationException>(() =>
            manager.PublishAnnouncement(1, 999, "Title", "Body", DateTime.Today));
    }

    [TestMethod]
    public void PublishAnnouncement_ValidAdmin_PublishesAndNotifiesRegisteredStudents()
    {
        var manager = new SchoolManager();
        var student = CreateStudent(1);
        manager.RegisterStudent(student);
        manager.RegisterAdministrator(CreateAdministrator(1, "Standard"));

        var announcement = manager.PublishAnnouncement(1, 1, "Holiday", "No class Monday", DateTime.Today);

        Assert.AreEqual(1, manager.AllAnnouncements.Count);
        Assert.AreEqual(1, student.ReceivedAnnouncements.Count);
        Assert.AreSame(announcement, student.ReceivedAnnouncements[0]);
    }

    // ----- Get* lookups -----

    [TestMethod]
    public void GetStudent_NotFound_ThrowsInvalidOperationException()
    {
        var manager = new SchoolManager();
        Assert.ThrowsExactly<InvalidOperationException>(() => manager.GetStudent(999));
    }

    [TestMethod]
    public void GetStudent_Found_ReturnsStudent()
    {
        var manager = new SchoolManager();
        var student = CreateStudent(1);
        manager.RegisterStudent(student);
        Assert.AreSame(student, manager.GetStudent(1));
    }

    [TestMethod]
    public void GetInstructor_NotFound_ThrowsInvalidOperationException()
    {
        var manager = new SchoolManager();
        Assert.ThrowsExactly<InvalidOperationException>(() => manager.GetInstructor(999));
    }

    [TestMethod]
    public void GetInstructor_Found_ReturnsInstructor()
    {
        var manager = new SchoolManager();
        var instructor = CreateInstructor(1);
        manager.RegisterInstructor(instructor);
        Assert.AreSame(instructor, manager.GetInstructor(1));
    }

    [TestMethod]
    public void GetAdministrator_NotFound_ThrowsInvalidOperationException()
    {
        var manager = new SchoolManager();
        Assert.ThrowsExactly<InvalidOperationException>(() => manager.GetAdministrator(999));
    }

    [TestMethod]
    public void GetAdministrator_Found_ReturnsAdministrator()
    {
        var manager = new SchoolManager();
        var admin = CreateAdministrator(1);
        manager.RegisterAdministrator(admin);
        Assert.AreSame(admin, manager.GetAdministrator(1));
    }

    [TestMethod]
    public void GetClass_NotFound_ThrowsInvalidOperationException()
    {
        var manager = new SchoolManager();
        Assert.ThrowsExactly<InvalidOperationException>(() => manager.GetClass(999));
    }

    [TestMethod]
    public void GetClass_Found_ReturnsClass()
    {
        var manager = new SchoolManager();
        manager.RegisterInstructor(CreateInstructor(1));
        var karateClass = CreateClass(1, 1);
        manager.AddClass(karateClass);
        Assert.AreSame(karateClass, manager.GetClass(1));
    }

    [TestMethod]
    public void GetPromotion_NotFound_ThrowsInvalidOperationException()
    {
        var manager = new SchoolManager();
        Assert.ThrowsExactly<InvalidOperationException>(() => manager.GetPromotion(999));
    }

    [TestMethod]
    public void GetPromotion_Found_ReturnsPromotion()
    {
        var manager = new SchoolManager();
        manager.RegisterStudent(CreateStudent(1, BeltLevel.White));
        manager.RegisterInstructor(CreateInstructor(1));
        var promotion = manager.SubmitBeltPromotion(1, 1, 1, BeltLevel.Yellow, DateTime.Today);
        Assert.AreSame(promotion, manager.GetPromotion(1));
    }
}
