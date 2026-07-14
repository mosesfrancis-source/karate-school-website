using KarateSchoolSystem.Enums;

namespace KarateSchoolSystem.Models;

public class BeltPromotion
{
    public int PromotionId { get; private set; }
    public int StudentId { get; private set; }
    public int InstructorId { get; private set; }
    public int? AdminId { get; private set; }
    public BeltLevel CurrentBelt { get; private set; }
    public BeltLevel TargetBelt { get; private set; }
    public DateTime RecommendedDate { get; private set; }
    public PromotionStatus Status { get; private set; }
    public string? AdminDecision { get; private set; }

    public BeltPromotion(
        int promotionId,
        int studentId,
        int instructorId,
        BeltLevel currentBelt,
        BeltLevel targetBelt,
        DateTime recommendedDate)
    {
        if (promotionId <= 0)
        {
            throw new ArgumentException("Promotion ID must be greater than zero.", nameof(promotionId));
        }

        if (studentId <= 0)
        {
            throw new ArgumentException("Student ID must be greater than zero.", nameof(studentId));
        }

        if (instructorId <= 0)
        {
            throw new ArgumentException("Instructor ID must be greater than zero.", nameof(instructorId));
        }

        if ((int)targetBelt != (int)currentBelt + 1)
        {
            throw new ArgumentException(
                "Belt promotions must advance exactly one rank at a time; skipping ranks is not allowed.",
                nameof(targetBelt));
        }

        PromotionId = promotionId;
        StudentId = studentId;
        InstructorId = instructorId;
        CurrentBelt = currentBelt;
        TargetBelt = targetBelt;
        RecommendedDate = recommendedDate;
        Status = PromotionStatus.Pending;
        AdminId = null;
        AdminDecision = null;
    }

    public void Approve(int adminId)
    {
        if (Status != PromotionStatus.Pending)
        {
            throw new InvalidOperationException("Only a pending promotion can be approved.");
        }

        AdminId = adminId;
        Status = PromotionStatus.Approved;
        AdminDecision = $"Approved by admin #{adminId}: promotion from {CurrentBelt} to {TargetBelt}.";
    }

    public void Deny(int adminId, string reason)
    {
        if (Status != PromotionStatus.Pending)
        {
            throw new InvalidOperationException("Only a pending promotion can be denied.");
        }

        AdminId = adminId;
        Status = PromotionStatus.Denied;
        AdminDecision = $"Denied by admin #{adminId}: {reason}";
    }

    public override string ToString() =>
        $"Promotion #{PromotionId}: Student {StudentId} {CurrentBelt}->{TargetBelt} ({Status})";
}
