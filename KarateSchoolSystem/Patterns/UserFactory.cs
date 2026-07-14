using KarateSchoolSystem.Enums;
using KarateSchoolSystem.Models;

namespace KarateSchoolSystem.Patterns;

/// <summary>
/// GoF Factory Method pattern: centralizes creation of the different User
/// subclasses behind a single API, so callers don't need to know which
/// concrete constructor to call.
/// </summary>
public static class UserFactory
{
    public static Student CreateStudent(
        int studentId,
        string firstName,
        string lastName,
        string email,
        string password,
        DateTime dateOfBirth,
        BeltLevel beltLevel,
        DateTime enrollmentDate,
        string? guardianEmail = null)
    {
        return new Student(
            studentId, firstName, lastName, email, password,
            dateOfBirth, beltLevel, enrollmentDate, guardianEmail);
    }

    public static Instructor CreateInstructor(
        int instructorId,
        string firstName,
        string lastName,
        string email,
        string password,
        string specialization,
        int yearsExperience,
        string certificationLevel)
    {
        return new Instructor(
            instructorId, firstName, lastName, email, password,
            specialization, yearsExperience, certificationLevel);
    }

    public static Administrator CreateAdministrator(
        int adminId,
        string firstName,
        string lastName,
        string email,
        string password,
        string department,
        string accessLevel)
    {
        return new Administrator(
            adminId, firstName, lastName, email, password, department, accessLevel);
    }
}
