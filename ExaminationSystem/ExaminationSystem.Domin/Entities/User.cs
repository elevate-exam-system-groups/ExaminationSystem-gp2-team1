using ExaminationSystem.Domin.Comman;
using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Entities
{
    public class User : AuditableEntity
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = "Student";

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "pending";

        public bool IsEmailVerified { get; set; } = false;

        public int FailedLoginAttempts { get; set; } = 0;

        public DateTime? LockedUntil { get; set; }

        public int FailedOtpAttempts { get; set; } = 0;

        // Navigation properties
        public virtual ICollection<OtpRecord> OtpRecords { get; set; } = new List<OtpRecord>();
        public virtual ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
    }
}
