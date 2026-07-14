using KarateSchoolSystem.Models;

namespace KarateSchoolSystem.Interfaces;

/// <summary>
/// Observer pattern: implemented by entities that react when an announcement is published.
/// </summary>
public interface IAnnouncementObserver
{
    void ReceiveAnnouncement(Announcement announcement);
}
