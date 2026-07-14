using KarateSchoolSystem.Enums;
using KarateSchoolSystem.Models;

namespace KarateSchoolSystem.Tests.Models;

[TestClass]
public class BeltPromotionTests
{
    private static BeltPromotion CreatePromotion(
        int promotionId = 1,
        int studentId = 1,
        int instructorId = 1,
        BeltLevel currentBelt = BeltLevel.White,
        BeltLevel targetBelt = BeltLevel.Yellow,
        DateTime? recommendedDate = null)
    {
        return new BeltPromotion(promotionId, studentId, instructorId, currentBelt, targetBelt, recommendedDate ?? DateTime.Today);
    }

    [TestMethod]
    public void Constructor_PromotionIdZeroOrNegative_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreatePromotion(promotionId: 0));
        Assert.ThrowsExactly<ArgumentException>(() => CreatePromotion(promotionId: -1));
    }

    [TestMethod]
    public void Constructor_StudentIdZeroOrNegative_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreatePromotion(studentId: 0));
        Assert.ThrowsExactly<ArgumentException>(() => CreatePromotion(studentId: -1));
    }

    [TestMethod]
    public void Constructor_InstructorIdZeroOrNegative_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => CreatePromotion(instructorId: 0));
        Assert.ThrowsExactly<ArgumentException>(() => CreatePromotion(instructorId: -1));
    }

    [TestMethod]
    public void Constructor_SkippingRank_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() =>
            CreatePromotion(currentBelt: BeltLevel.White, targetBelt: BeltLevel.Orange));
    }

    [TestMethod]
    public void Constructor_SameOrLowerTargetBelt_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() =>
            CreatePromotion(currentBelt: BeltLevel.Yellow, targetBelt: BeltLevel.Yellow));
        Assert.ThrowsExactly<ArgumentException>(() =>
            CreatePromotion(currentBelt: BeltLevel.Yellow, targetBelt: BeltLevel.White));
    }

    [TestMethod]
    public void Constructor_ExactlyOneRankUp_Succeeds()
    {
        var promotion = CreatePromotion(currentBelt: BeltLevel.Green, targetBelt: BeltLevel.Blue);
        Assert.AreEqual(BeltLevel.Green, promotion.CurrentBelt);
        Assert.AreEqual(BeltLevel.Blue, promotion.TargetBelt);
        Assert.AreEqual(PromotionStatus.Pending, promotion.Status);
        Assert.IsNull(promotion.AdminId);
        Assert.IsNull(promotion.AdminDecision);
    }

    [TestMethod]
    public void Approve_WhenPending_SetsApprovedStatusAndAdminFields()
    {
        var promotion = CreatePromotion();
        promotion.Approve(5);
        Assert.AreEqual(PromotionStatus.Approved, promotion.Status);
        Assert.AreEqual(5, promotion.AdminId);
        Assert.IsTrue(promotion.AdminDecision!.Contains("Approved by admin #5"));
    }

    [TestMethod]
    public void Approve_WhenNotPending_ThrowsInvalidOperationException()
    {
        var promotion = CreatePromotion();
        promotion.Approve(5);
        Assert.ThrowsExactly<InvalidOperationException>(() => promotion.Approve(6));
    }

    [TestMethod]
    public void Deny_WhenPending_SetsDeniedStatusAndAdminFields()
    {
        var promotion = CreatePromotion();
        promotion.Deny(5, "Not ready");
        Assert.AreEqual(PromotionStatus.Denied, promotion.Status);
        Assert.AreEqual(5, promotion.AdminId);
        Assert.IsTrue(promotion.AdminDecision!.Contains("Denied by admin #5: Not ready"));
    }

    [TestMethod]
    public void Deny_WhenNotPending_ThrowsInvalidOperationException()
    {
        var promotion = CreatePromotion();
        promotion.Deny(5, "Not ready");
        Assert.ThrowsExactly<InvalidOperationException>(() => promotion.Deny(6, "Still not ready"));
    }

    [TestMethod]
    public void Deny_AfterApproved_ThrowsInvalidOperationException()
    {
        var promotion = CreatePromotion();
        promotion.Approve(5);
        Assert.ThrowsExactly<InvalidOperationException>(() => promotion.Deny(6, "reason"));
    }

    [TestMethod]
    public void ToString_ReturnsFormattedSummary()
    {
        var promotion = CreatePromotion(promotionId: 8, studentId: 1, currentBelt: BeltLevel.White, targetBelt: BeltLevel.Yellow);
        string result = promotion.ToString();
        Assert.IsTrue(result.Contains("Promotion #8"));
        Assert.IsTrue(result.Contains("White->Yellow"));
        Assert.IsTrue(result.Contains("Pending"));
    }
}
