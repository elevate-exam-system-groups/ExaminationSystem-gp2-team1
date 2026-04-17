using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Domin.Common.Result
{
    public class Error
    {
        public ErrorCode Code { get; set; }
        public string Description { get; set; }
        public Error(ErrorCode code, string description)
        {
            Code = code;
            Description = description;
        }
    }
}
