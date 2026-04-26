using ExaminationSystem.Domin.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Entities
{
    public class RefreshToken : AuditableEntity
    {
        [Required]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        [Required]
        public string TokenHash { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        public bool IsRevoked { get; set; } = false;

        public DateTime? RevokedAt { get; set; }

        [MaxLength(45)]
        public string? IpAddress { get; set; }

        public virtual User User { get; set; } = null!;
    }
}