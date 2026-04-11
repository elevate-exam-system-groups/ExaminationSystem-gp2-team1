using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Domin.Common;

namespace ExaminationSystem.Domin.Entitys
{
    public class AttemptAnswer : BaseEntity
    {
        public int AttemptId { get; set; }
        public int QuestionId { get; set; }
        public int SelectedOptionId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Attempt Attempt { get; set; }
        public Question Question { get; set; }
        public Option Option { get; set; }
    }
}
