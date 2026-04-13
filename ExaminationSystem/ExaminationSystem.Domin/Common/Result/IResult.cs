using ExaminationSystem.Domin.Comman.Result;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Domin.Common.Result
{
    public interface IResult
    {
        List<Error>? Errors { get; }

        bool IsSuccess { get; }
    }

    public interface IResult<out TValue> : IResult
    {
        TValue Value { get; }
    }
}
