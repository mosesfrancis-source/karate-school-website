using KarateSchoolSystem.Models;

namespace KarateSchoolSystem.Tests.Models;

[TestClass]
public class AnnouncementTests
{
    private static Announcement CreateAnnouncement(
        int announcementId = 1,
        int adminId = 1,
        string title = "Test Title",
        string body = "Test Body",
        DateTime? publishedDate = null)
    {
        return new Announcement(announcementId, adminId, title, body, publishedDate ?? DateTime.Today);
    }

    [TestMethod]
    public void Constructor_AnnouncementIdZeroOrNegative_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateAnnouncement(announcementId: 0));
        Assert.ThrowsExactly<ArgumentException>(() => CreateAnnouncement(announcementId: -1));
    }

    [TestMethod]
    public void Constructor_AdminIdZeroOrNegative_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateAnnouncement(adminId: 0));
        Assert.ThrowsExactly<ArgumentException>(() => CreateAnnouncement(adminId: -1));
    }

    [TestMethod]
    public void Constructor_TitleNullOrWhitespace_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateAnnouncement(title: ""));
        Assert.ThrowsExactly<ArgumentException>(() => CreateAnnouncement(title: "   "));
    }

    [TestMethod]
    public void Constructor_BodyNullOrWhitespace_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreateAnnouncement(body: ""));
        Assert.ThrowsExactly<ArgumentException>(() => CreateAnnouncement(body: "   "));
    }

    [TestMethod]
    public void Constructor_ValidArguments_SetsPropertiesAndIsActiveTrue()
    {
        var announcement = CreateAnnouncement(announcementId: 2, adminId: 3, title: "Holiday", body: "No class Monday");
        Assert.AreEqual(2, announcement.AnnouncementId);
        Assert.AreEqual(3, announcement.AdminId);
        Assert.AreEqual("Holiday", announcement.Title);
        Assert.AreEqual("No class Monday", announcement.Body);
        Assert.IsTrue(announcement.IsActive);
    }

    [TestMethod]
    public void Deactivate_WhenActive_SetsIsActiveFalse()
    {
        var announcement = CreateAnnouncement();
        announcement.Deactivate();
        Assert.IsFalse(announcement.IsActive);
    }

    [TestMethod]
    public void Deactivate_WhenAlreadyInactive_ThrowsInvalidOperationException()
    {
        var announcement = CreateAnnouncement();
        announcement.Deactivate();
        Assert.ThrowsExactly<InvalidOperationException>(() => announcement.Deactivate());
    }

    [TestMethod]
    public void ToString_WhenActive_ShowsActive()
    {
        var announcement = CreateAnnouncement(announcementId: 4, title: "Event");
        string result = announcement.ToString();
        Assert.IsTrue(result.Contains("Announcement #4"));
        Assert.IsTrue(result.Contains("Event"));
        Assert.IsTrue(result.Contains("Active"));
        Assert.IsFalse(result.Contains("Inactive"));
    }

    [TestMethod]
    public void ToString_WhenInactive_ShowsInactive()
    {
        var announcement = CreateAnnouncement(announcementId: 4, title: "Event");
        announcement.Deactivate();
        string result = announcement.ToString();
        Assert.IsTrue(result.Contains("Inactive"));
    }
}
