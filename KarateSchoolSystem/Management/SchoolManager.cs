using KarateSchoolSystem.Enums;
using KarateSchoolSystem.Exceptions;
using KarateSchoolSystem.Models;
using KarateSchoolSystem.Patterns;

namespace KarateSchoolSystem.Management;

/// <summary>
/// In-memory orchestrator for the whole system; plays the role of a
/// repository/service layer since this console app has no real database.
/// </summary>
public class SchoolManager
{
    private readonly Dictionary<int, Student> _students = new();
    private readonly Dictionary<int, Instructor> _instructors = new();
    private readonly Dictionary<int, Administrator> _administrators = new();
    private readonly Dictionary<int, KarateClass> _classes = new();

    private readonly List<StudentClassEnrollment> _enrollments = new();
    private readonly List<BeltPromotion> _promotions = new();
    private readonly List<Payment> _payments = new();
    private readonly List<Announcement> _announcements = new();

    private readonly AnnouncementPublisher _announcementPublisher = new();
    private readonly PaymentProcessor _paymentProcessor = new();

    public IReadOnlyCollection<Student> AllStudents => _students.Values;
    public IReadOnlyCollection<Instructor> AllInstructors => _instructors.Values;
    public IReadOnlyCollection<Administrator> AllAdministrators => _administrators.Values;
    public IReadOnlyCollection<KarateClass> AllClasses => _classes.Values;
    public IReadOnlyList<StudentClassEnrollment> AllEnrollments => _enrollments.AsReadOnly();
    public IReadOnlyList<BeltPromotion> AllPromotions => _promotions.AsReadOnly();
    public IReadOnlyList<Payment> AllPayments => _payments.AsReadOnly();
    public IReadOnlyList<Announcement> AllAnnouncements => _announcements.AsReadOnly();

    public void RegisterStudent(Student student)
    {
        if (_students.ContainsKey(student.StudentId))
        {
            throw new DuplicateUserException($"A student with ID {student.StudentId} is already registered.");
        }

        _students.Add(student.StudentId, student);
        _announcementPublisher.Subscribe(student);
    }

    public void RegisterInstructor(Instructor instructor)
    {
        if (_instructors.ContainsKey(instructor.InstructorId))
        {
            throw new DuplicateUserException($"An instructor with ID {instructor.InstructorId} is already registered.");
        }

        _instructors.Add(instructor.InstructorId, instructor);
    }

    public void RegisterAdministrator(Administrator administrator)
    {
        if (_administrators.ContainsKey(administrator.AdminId))
        {
            throw new DuplicateUserException($"An administrator with ID {administrator.AdminId} is already registered.");
        }

        _administrators.Add(administrator.AdminId, administrator);
    }

    public void AddClass(KarateClass karateClass)
    {
        if (_classes.ContainsKey(karateClass.ClassId))
        {
            throw new DuplicateUserException($"A class with ID {karateClass.ClassId} already exists.");
        }

        if (!_instructors.ContainsKey(karateClass.InstructorId))
        {
            throw new InvalidOperationException(
                $"Instructor with ID {karateClass.InstructorId} is not registered.");
        }

        _classes.Add(karateClass.ClassId, karateClass);
    }

    public StudentClassEnrollment EnrollStudentInClass(int enrollmentId, int studentId, int classId, DateTime date)
    {
        Student student = GetStudent(studentId);
        KarateClass karateClass = GetClass(classId);

        if ((int)student.BeltLevel < (int)karateClass.BeltLevelRequired)
        {
            throw new InvalidOperationException(
                $"Student's belt level ({student.BeltLevel}) does not meet the class requirement ({karateClass.BeltLevelRequired}).");
        }

        karateClass.EnrollStudent(studentId);

        var enrollment = new StudentClassEnrollment(enrollmentId, studentId, classId, date);
        _enrollments.Add(enrollment);
        return enrollment;
    }

    public Attendance RecordAttendance(int attendanceId, int studentId, int classId, DateTime date, AttendanceStatus status, string? notes = null)
    {
        Student student = GetStudent(studentId);
        KarateClass karateClass = GetClass(classId);

        if (!karateClass.IsStudentEnrolled(studentId))
        {
            throw new InvalidOperationException("Student is not enrolled in this class.");
        }

        var attendance = new Attendance(attendanceId, studentId, classId, date, status, notes);
        student.RecordAttendance(attendance);
        return attendance;
    }

    public BeltPromotion SubmitBeltPromotion(
        int promotionId, int studentId, int instructorId, BeltLevel targetBelt, DateTime recommendedDate)
    {
        Student student = GetStudent(studentId);
        if (!_instructors.ContainsKey(instructorId))
        {
            throw new InvalidOperationException($"Instructor with ID {instructorId} is not registered.");
        }

        var promotion = new BeltPromotion(promotionId, studentId, instructorId, student.BeltLevel, targetBelt, recommendedDate);
        _promotions.Add(promotion);
        return promotion;
    }

    public void ApproveBeltPromotion(int promotionId, int adminId)
    {
        BeltPromotion promotion = GetPromotion(promotionId);
        Administrator admin = GetAdministrator(adminId);

        if (!admin.CanApproveActions)
        {
            throw new UnauthorizedActionException(
                $"Administrator {adminId} does not have sufficient access to approve promotions.");
        }

        promotion.Approve(adminId);

        Student student = GetStudent(promotion.StudentId);
        student.Promote(promotion.TargetBelt);
    }

    public void DenyBeltPromotion(int promotionId, int adminId, string reason)
    {
        BeltPromotion promotion = GetPromotion(promotionId);
        Administrator admin = GetAdministrator(adminId);

        if (!admin.CanApproveActions)
        {
            throw new UnauthorizedActionException(
                $"Administrator {adminId} does not have sufficient access to deny promotions.");
        }

        promotion.Deny(adminId, reason);
    }

    public void ChargeStudent(int studentId, decimal amount)
    {
        Student student = GetStudent(studentId);
        student.ApplyCharge(amount);
    }

    public (Payment payment, string confirmation) ProcessPayment(
        int paymentId, int studentId, int adminId, decimal amount, PaymentMethod method, string? description = null)
    {
        Student student = GetStudent(studentId);
        Administrator admin = GetAdministrator(adminId);

        if (!admin.CanApproveActions)
        {
            throw new UnauthorizedActionException(
                $"Administrator {adminId} does not have sufficient access to process payments.");
        }

        var payment = new Payment(paymentId, studentId, adminId, amount, DateTime.Now, method, description);
        string confirmation = _paymentProcessor.Process(payment);
        student.MakePayment(amount);
        _payments.Add(payment);

        return (payment, confirmation);
    }

    public Announcement PublishAnnouncement(int announcementId, int adminId, string title, string body, DateTime publishedDate)
    {
        GetAdministrator(adminId);

        var announcement = new Announcement(announcementId, adminId, title, body, publishedDate);
        _announcementPublisher.Publish(announcement);
        _announcements.Add(announcement);
        return announcement;
    }

    public Student GetStudent(int studentId)
    {
        if (!_students.TryGetValue(studentId, out Student? student))
        {
            throw new InvalidOperationException($"Student with ID {studentId} was not found.");
        }

        return student;
    }

    public Instructor GetInstructor(int instructorId)
    {
        if (!_instructors.TryGetValue(instructorId, out Instructor? instructor))
        {
            throw new InvalidOperationException($"Instructor with ID {instructorId} was not found.");
        }

        return instructor;
    }

    public Administrator GetAdministrator(int adminId)
    {
        if (!_administrators.TryGetValue(adminId, out Administrator? administrator))
        {
            throw new InvalidOperationException($"Administrator with ID {adminId} was not found.");
        }

        return administrator;
    }

    public KarateClass GetClass(int classId)
    {
        if (!_classes.TryGetValue(classId, out KarateClass? karateClass))
        {
            throw new InvalidOperationException($"Class with ID {classId} was not found.");
        }

        return karateClass;
    }

    public BeltPromotion GetPromotion(int promotionId)
    {
        BeltPromotion? promotion = _promotions.FirstOrDefault(p => p.PromotionId == promotionId);
        if (promotion is null)
        {
            throw new InvalidOperationException($"Promotion with ID {promotionId} was not found.");
        }

        return promotion;
    }
}
