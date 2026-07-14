using KarateSchoolSystem.Models;

namespace KarateSchoolSystem.Interfaces;

/// <summary>
/// Observer pattern: implemented by the subject that notifies subscribed observers of new announcements.
/// </summary>
public interface IAnnouncementPublisher
{
    void Subscribe(IAnnouncementObserver observer);
    void Unsubscribe(IAnnouncementObserver observer);
    void Publish(Announcement announcement);
}
