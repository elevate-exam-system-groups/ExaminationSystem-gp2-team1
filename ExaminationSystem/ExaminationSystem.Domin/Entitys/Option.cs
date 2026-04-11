using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Domin.Common;

namespace ExaminationSystem.Domin.Entitys
{
    public class Option : BaseEntity
    {
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public string Text { get; set; } 
        public bool IsCorrect { get; set; }


        public virtual ICollection<AttemptAnswer> AttemptAnswers { get; set; } = new HashSet<AttemptAnswer>();

    }


}
