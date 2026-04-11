using ExaminationSystem.Domin.Comman;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Entities
{
    public class OtpRecord : AuditableEntity
    {
        [Required]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        [Required]
        public string OtpHash { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        public bool IsUsed { get; set; } = false;

        public int AttemptCount { get; set; } = 0;

        public bool IsLocked { get; set; } = false;

        public DateTime? LockedUntil { get; set; }

        public virtual User User { get; set; } = null!;
    }
}