using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Domin.Comman;

namespace ExaminationSystem.Domin.Entitys
{
    public class Question : BaseEntity
    {
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }


        public string Text { get; set; }
        public string Type { get; set; } // multiple-choice .  can scale to => true/false

        public string ? Explanation { get; set; } // Optional text shown to students after submitting, explaining the correct answer
        public int OrderIndex { get; set; } // Controls display order 


        public virtual ICollection<AttemptAnswer> AttemptAnswers { get; set; } = new HashSet<AttemptAnswer>();
        public virtual ICollection<Option> Options { get; set; } = new HashSet<Option>();


    }
}
