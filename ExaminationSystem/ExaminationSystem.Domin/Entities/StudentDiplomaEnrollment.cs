using ExaminationSystem.Domin.Comman;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Entities
{
    public class StudentDiplomaEnrollment : AuditableEntity
    {
        [Required]
        [ForeignKey(nameof(Student))]
        public Guid StudentId { get; set; }

        [Required]
        [ForeignKey(nameof(Diploma))]
        public Guid DiplomaId { get; set; }

        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

        public int? CompletedQuizzesCount { get; set; } = 0;

        public double? AverageScore { get; set; }

        // Navigation properties
        public virtual User Student { get; set; } = null!;
        public virtual Diploma Diploma { get; set; } = null!;
    }
}