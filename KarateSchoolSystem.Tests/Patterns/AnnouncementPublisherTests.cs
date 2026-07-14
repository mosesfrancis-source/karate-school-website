using KarateSchoolSystem.Interfaces;
using KarateSchoolSystem.Models;
using KarateSchoolSystem.Patterns;

namespace KarateSchoolSystem.Tests.Patterns;

[TestClass]
public class AnnouncementPublisherTests
{
    private class FakeObserver : IAnnouncementObserver
    {
        public List<Announcement> Received { get; } = new();

        public void ReceiveAnnouncement(Announcement announcement)
        {
            Received.Add(announcement);
        }
    }

    private static Announcement CreateAnnouncement(int id = 1) =>
        new(id, 1, "Title", "Body", DateTime.Today);

    [TestMethod]
    public void Subscribe_NullObserver_ThrowsArgumentNullException()
    {
        var publisher = new AnnouncementPublisher();
        Assert.ThrowsExactly<ArgumentNullException>(() => publisher.Subscribe(null!));
    }

    [TestMethod]
    public void Subscribe_SameObserverTwice_OnlyAddedOnce()
    {
        var publisher = new AnnouncementPublisher();
        var observer = new FakeObserver();
        publisher.Subscribe(observer);
        publisher.Subscribe(observer);

        var announcement = CreateAnnouncement();
        publisher.Publish(announcement);

        Assert.AreEqual(1, observer.Received.Count);
    }

    [TestMethod]
    public void Unsubscribe_RemovesObserver()
    {
        var publisher = new AnnouncementPublisher();
        var observer = new FakeObserver();
        publisher.Subscribe(observer);
        publisher.Unsubscribe(observer);

        publisher.Publish(CreateAnnouncement());

        Assert.AreEqual(0, observer.Received.Count);
    }

    [TestMethod]
    public void Publish_NullAnnouncement_ThrowsArgumentNullException()
    {
        var publisher = new AnnouncementPublisher();
        Assert.ThrowsExactly<ArgumentNullException>(() => publisher.Publish(null!));
    }

    [TestMethod]
    public void Publish_InactiveAnnouncement_ThrowsInvalidOperationException()
    {
        var publisher = new AnnouncementPublisher();
        var announcement = CreateAnnouncement();
        announcement.Deactivate();

        Assert.ThrowsExactly<InvalidOperationException>(() => publisher.Publish(announcement));
    }

    [TestMethod]
    public void Publish_ActiveAnnouncement_NotifiesAllObserversAndRecordsIt()
    {
        var publisher = new AnnouncementPublisher();
        var observer1 = new FakeObserver();
        var observer2 = new FakeObserver();
        publisher.Subscribe(observer1);
        publisher.Subscribe(observer2);

        var announcement = CreateAnnouncement();
        publisher.Publish(announcement);

        Assert.AreEqual(1, observer1.Received.Count);
        Assert.AreEqual(1, observer2.Received.Count);
        Assert.AreSame(announcement, observer1.Received[0]);
        Assert.AreEqual(1, publisher.PublishedAnnouncements.Count);
        Assert.AreSame(announcement, publisher.PublishedAnnouncements[0]);
    }
}
