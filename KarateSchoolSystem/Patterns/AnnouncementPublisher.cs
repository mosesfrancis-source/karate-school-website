using KarateSchoolSystem.Interfaces;
using KarateSchoolSystem.Models;

namespace KarateSchoolSystem.Patterns;

/// <summary>
/// GoF Observer pattern: the subject that notifies subscribed
/// IAnnouncementObserver instances (students) whenever an announcement is published.
/// </summary>
public class AnnouncementPublisher : IAnnouncementPublisher
{
    private readonly List<IAnnouncementObserver> _observers = new();
    private readonly List<Announcement> _publishedAnnouncements = new();

    public IReadOnlyList<Announcement> PublishedAnnouncements => _publishedAnnouncements.AsReadOnly();

    public void Subscribe(IAnnouncementObserver observer)
    {
        if (observer is null)
        {
            throw new ArgumentNullException(nameof(observer));
        }

        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    public void Unsubscribe(IAnnouncementObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Publish(Announcement announcement)
    {
        if (announcement is null)
        {
            throw new ArgumentNullException(nameof(announcement));
        }

        if (!announcement.IsActive)
        {
            throw new InvalidOperationException("Only an active announcement can be published.");
        }

        foreach (var observer in _observers)
        {
            observer.ReceiveAnnouncement(announcement);
        }

        _publishedAnnouncements.Add(announcement);
    }
}
