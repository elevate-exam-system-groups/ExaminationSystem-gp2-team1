using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Domin.Common;

namespace ExaminationSystem.Domin.Entitys
{
    public class Attempt : BaseEntity
    {
        public int UserId { get; set; }
        public int QuizId { get; set; }

        public string Status { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public decimal Score { get; set; }
        public bool Passed { get; set; }

        public User User { get; set; }
        public Quiz Quiz { get; set; }

        public virtual ICollection<AttemptAnswer> Answers { get; set; } = new HashSet<AttemptAnswer>();


    }
}
