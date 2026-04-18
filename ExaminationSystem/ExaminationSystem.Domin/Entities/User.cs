using ExaminationSystem.Domin.Common;
using ExaminationSystem.Domin.Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        /// <summary>
        /// FK to UserRole table ("Student" or "Admin")
        /// </summary>
        [Required]
        [ForeignKey(nameof(Role))]
        public Guid RoleId { get; set; } 

        /// <summary>
        /// Account status: "pending", "active", or "locked"
        /// </summary>
        [Required]
        public AccountStatus Status { get; set; } = AccountStatus.pending;

        public bool IsEmailVerified { get; set; } = false;

        public int FailedLoginAttempts { get; set; } = 0;

        public DateTime? LockedUntil { get; set; }

        public int FailedOtpAttempts { get; set; } = 0;

        // Navigation properties
        public virtual UserRole Role { get; set; } = null!;
        public virtual ICollection<OtpRecord> OtpRecords { get; set; } = new HashSet<OtpRecord>();
        public virtual ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new HashSet<PasswordResetToken>();
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();
        public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new HashSet<QuizAttempt>();
        public virtual ICollection<StudentDiplomaEnrollment> DiplomaEnrollments { get; set; } = new HashSet<StudentDiplomaEnrollment>();
    }
}
