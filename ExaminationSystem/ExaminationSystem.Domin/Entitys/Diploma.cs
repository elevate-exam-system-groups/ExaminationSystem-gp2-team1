using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Domin.Common;
using ExaminationSystem.Domin.Enums;

namespace ExaminationSystem.Domin.Entitys
{
    public class Diploma : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; } = null!;

        public DiplomaStatus Status { get; set; } // draft / published

        public int UserId { get; set; } // who create diploma (user id)
        public User Creator { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }


        public virtual ICollection<Enrollment> Enrollments { get; set; } = new HashSet<Enrollment>();
        public virtual ICollection<Quiz> Quizzes { get; set; } = new HashSet<Quiz>();



    }
}
