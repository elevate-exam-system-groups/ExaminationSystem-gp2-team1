using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Domin.Comman;
using ExaminationSystem.Domin.Enums;

namespace ExaminationSystem.Domin.Entitys
{
    public class Quiz : BaseEntity
    {
        public string Title { get; set; } = null!;

        public int DiplomaId { get; set; }
        public Diploma Diploma { get; set; }

        public int DurationMinutes { get; set; }
        public int PassScore { get; set; }
        public int? MaxAttempts { get; set; }

        public QuizStatus Status { get; set; }
        public string ? Instructions { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

 

        public virtual ICollection<Question> Questions { get; set; } = new HashSet<Question>();
        public virtual ICollection<Attempt> Attempts { get; set; } = new HashSet<Attempt>();

    }
}
