using ExaminationSystem.Domin.Comman;
using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Entities
{
    public class Diploma : AuditableEntity
    {
        [Required]
        [MinLength(3)]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        /// <summary>
        /// Status: "draft" or "published"
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "draft";

        // Navigation properties
        public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
        public virtual ICollection<StudentDiplomaEnrollment> Enrollments { get; set; } = new List<StudentDiplomaEnrollment>();
    }
}