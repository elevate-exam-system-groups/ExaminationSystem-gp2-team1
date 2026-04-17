using ExaminationSystem.Domin.Common;
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
        public bool IsPublished { get; set; } = false;

        // Navigation properties
        public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
        public virtual ICollection<StudentDiplomaEnrollment> Enrollments { get; set; } = new HashSet<StudentDiplomaEnrollment>();
    }
}