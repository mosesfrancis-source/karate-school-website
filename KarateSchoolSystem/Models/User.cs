using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using KarateSchoolSystem.Interfaces;

namespace KarateSchoolSystem.Models;

/// <summary>
/// Abstract base class for every person who has an account in the system.
/// </summary>
public abstract class User : IReportable
{
    private static readonly Regex EmailPattern = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public int UserId { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string Role { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public string FullName => $"{FirstName} {LastName}";

    protected User(int userId, string firstName, string lastName, string email, string password, string role)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("User ID must be greater than zero.", nameof(userId));
        }

        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("First name cannot be null or whitespace.", nameof(firstName));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Last name cannot be null or whitespace.", nameof(lastName));
        }

        if (string.IsNullOrWhiteSpace(email) || !EmailPattern.IsMatch(email))
        {
            throw new ArgumentException("Email must be a non-empty, validly formatted address.", nameof(email));
        }

        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
        {
            throw new ArgumentException("Password must be at least 8 characters long.", nameof(password));
        }

        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Role = role;
        // A real system would use bcrypt with per-user salting; SHA256 is used here
        // to keep this assignment dependency-free and deterministic for unit testing.
        PasswordHash = HashPassword(password);
        IsActive = true;
        CreatedAt = DateTime.Now;
    }

    private static string HashPassword(string rawPassword)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawPassword));
        return Convert.ToHexString(bytes);
    }

    public bool VerifyPassword(string rawPassword)
    {
        if (string.IsNullOrEmpty(rawPassword))
        {
            return false;
        }

        return HashPassword(rawPassword) == PasswordHash;
    }

    public virtual void Deactivate()
    {
        IsActive = false;
    }

    public virtual void Activate()
    {
        IsActive = true;
    }

    public abstract string GetRoleDescription();

    public virtual string GenerateReport()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"User ID: {UserId}");
        sb.AppendLine($"Name: {FullName}");
        sb.AppendLine($"Email: {Email}");
        sb.AppendLine($"Role: {GetRoleDescription()}");
        sb.AppendLine($"Active: {IsActive}");
        sb.AppendLine($"Created At: {CreatedAt:g}");
        return sb.ToString();
    }
}
