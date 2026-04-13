using ExaminationSystem.Domin.Comman;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Entities
{
    /// <summary>
    /// Tracks which students are enrolled in which diploma programs
    /// </summary>
    public class StudentDiplomaEnrollment : Entity
    {
        [Required]
        [ForeignKey(nameof(Student))]
        public Guid StudentId { get; set; }

        [Required]
        [ForeignKey(nameof(Diploma))]
        public Guid DiplomaId { get; set; }

        public DateTime EnrolledAt { get; set; }

        // Navigation properties
        public virtual User Student { get; set; } = null!;
        public virtual Diploma Diploma { get; set; } = null!;
    }
}