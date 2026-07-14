using System.Globalization;
using KarateSchoolSystem.Enums;
using KarateSchoolSystem.Exceptions;
using KarateSchoolSystem.Management;
using KarateSchoolSystem.Models;
using KarateSchoolSystem.Patterns;

CultureInfo.CurrentCulture = new CultureInfo("en-US");

var manager = new SchoolManager();

Console.WriteLine("=== Karate School Management System Demo ===\n");

// --- Registration ---
var admin = UserFactory.CreateAdministrator(
    1, "Alice", "Admin", "alice.admin@dojo.com", "Password1", "Operations", "SuperAdmin");
manager.RegisterAdministrator(admin);

var instructor = UserFactory.CreateInstructor(
    1, "Bruce", "Lee", "bruce.lee@dojo.com", "Password1", "Kickboxing", 15, "Master Certified");
manager.RegisterInstructor(instructor);

var minorStudent = UserFactory.CreateStudent(
    1, "Timmy", "Tanaka", "timmy.t@dojo.com", "Password1",
    DateTime.Now.AddYears(-10), BeltLevel.White, DateTime.Now.AddMonths(-1),
    "guardian.tanaka@example.com");
manager.RegisterStudent(minorStudent);

var adultStudent = UserFactory.CreateStudent(
    2, "Diana", "Diaz", "diana.diaz@dojo.com", "Password1",
    DateTime.Now.AddYears(-25), BeltLevel.White, DateTime.Now.AddMonths(-2));
manager.RegisterStudent(adultStudent);

Console.WriteLine("Registered 1 administrator, 1 instructor, and 2 students.\n");

// --- Class + enrollment ---
var karateClass = new KarateClass(
    1, instructor.InstructorId, "Beginner Karate", DateTime.Now.AddDays(1),
    "Main Dojo", 20, BeltLevel.White);
manager.AddClass(karateClass);

manager.EnrollStudentInClass(1, minorStudent.StudentId, karateClass.ClassId, DateTime.Now);
manager.EnrollStudentInClass(2, adultStudent.StudentId, karateClass.ClassId, DateTime.Now);
Console.WriteLine("Enrolled both students in 'Beginner Karate'.\n");

// --- Attendance ---
manager.RecordAttendance(1, minorStudent.StudentId, karateClass.ClassId, DateTime.Now, AttendanceStatus.Present);
manager.RecordAttendance(2, adultStudent.StudentId, karateClass.ClassId, DateTime.Now, AttendanceStatus.Late, "Traffic");
Console.WriteLine("Recorded attendance for both students.\n");

// --- Belt promotion ---
var promotion = manager.SubmitBeltPromotion(
    1, minorStudent.StudentId, instructor.InstructorId, BeltLevel.Yellow, DateTime.Now);
manager.ApproveBeltPromotion(promotion.PromotionId, admin.AdminId);
Console.WriteLine($"Promotion approved: {promotion}\n");

// --- Payment ---
manager.ChargeStudent(adultStudent.StudentId, 150m);
var (payment, confirmation) = manager.ProcessPayment(
    1, adultStudent.StudentId, admin.AdminId, 100m, PaymentMethod.CreditCard, "Monthly tuition (partial)");
Console.WriteLine($"Payment recorded: {payment}");
Console.WriteLine($"Processor confirmation: {confirmation}");
Console.WriteLine($"Diana's outstanding balance: {adultStudent.OutstandingBalance:C}\n");

// --- Announcement ---
manager.PublishAnnouncement(1, admin.AdminId, "Holiday Schedule",
    "The dojo will be closed on national holidays this month.", DateTime.Now);
Console.WriteLine("Published announcement. Students who received it:");
Console.WriteLine($" - {minorStudent.FullName}: {minorStudent.ReceivedAnnouncements.Count} announcement(s)");
Console.WriteLine($" - {adultStudent.FullName}: {adultStudent.ReceivedAnnouncements.Count} announcement(s)\n");

// --- Polymorphic reporting ---
Console.WriteLine("=== Reports (polymorphic via base User) ===\n");
User[] allUsers = { admin, instructor, minorStudent, adultStudent };
foreach (User user in allUsers)
{
    Console.WriteLine(user.GenerateReport());
}

// --- Exception demonstrations ---
Console.WriteLine("=== Exception Demonstrations ===\n");

try
{
    manager.RegisterAdministrator(admin);
}
catch (DuplicateUserException ex)
{
    Console.WriteLine($"DuplicateUserException: {ex.Message}");
}

try
{
    manager.GetStudent(999);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"InvalidOperationException: {ex.Message}");
}

try
{
    UserFactory.CreateStudent(
        3, "Sam", "Small", "sam.small@dojo.com", "Password1",
        DateTime.Now.AddYears(-12), BeltLevel.White, DateTime.Now);
}
catch (ArgumentException ex)
{
    Console.WriteLine($"ArgumentException: {ex.Message}");
}

Console.WriteLine("\n=== Demo complete ===");
