using ExaminationSystem.Domin.Comman;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Entities
{
    public class QuizAttempt : AuditableEntity
    {
        [Required]
        [ForeignKey(nameof(Student))]
        public Guid StudentId { get; set; }

        [Required]
        [ForeignKey(nameof(Quiz))]
        public Guid QuizId { get; set; }

        /// <summary>
        /// Status: "in_progress", "submitted", "timed_out"
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "in_progress";

        public DateTime StartedAt { get; set; } = DateTime.UtcNow;

        public DateTime? SubmittedAt { get; set; }

        public DateTime? DeadlineAt { get; set; }

        /// <summary>
        /// Final score (percentage)
        /// </summary>
        public int? Score { get; set; }

        /// <summary>
        /// Whether the student passed (score >= quiz.PassScore)
        /// </summary>
        public bool? IsPassed { get; set; }

        /// <summary>
        /// Number of correct answers
        /// </summary>
        public int? CorrectAnswerCount { get; set; }

        /// <summary>
        /// Total number of questions in this attempt
        /// </summary>
        public int TotalQuestions { get; set; }

        // Navigation properties
        public virtual User Student { get; set; } = null!;
        public virtual Quiz Quiz { get; set; } = null!;
        public virtual ICollection<AttemptAnswer> Answers { get; set; } = new List<AttemptAnswer>();
    }
}