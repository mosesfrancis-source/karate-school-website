namespace KarateSchoolSystem.Models;

public class Announcement
{
    public int AnnouncementId { get; private set; }
    public int AdminId { get; private set; }
    public string Title { get; private set; }
    public string Body { get; private set; }
    public DateTime PublishedDate { get; private set; }
    public bool IsActive { get; private set; }

    public Announcement(int announcementId, int adminId, string title, string body, DateTime publishedDate)
    {
        if (announcementId <= 0)
        {
            throw new ArgumentException("Announcement ID must be greater than zero.", nameof(announcementId));
        }

        if (adminId <= 0)
        {
            throw new ArgumentException("Admin ID must be greater than zero.", nameof(adminId));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be null or whitespace.", nameof(title));
        }

        if (string.IsNullOrWhiteSpace(body))
        {
            throw new ArgumentException("Body cannot be null or whitespace.", nameof(body));
        }

        AnnouncementId = announcementId;
        AdminId = adminId;
        Title = title;
        Body = body;
        PublishedDate = publishedDate;
        IsActive = true;
    }

    public void Deactivate()
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("This announcement is already inactive.");
        }

        IsActive = false;
    }

    public override string ToString() =>
        $"Announcement #{AnnouncementId}: {Title} ({PublishedDate:d}) - {(IsActive ? "Active" : "Inactive")}";
}
