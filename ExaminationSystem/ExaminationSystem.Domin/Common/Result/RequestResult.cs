using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Domin.Comman.Result;

namespace ExaminationSystem.Domin.Common.Result
{
    public record RequestResult<TResult>(TResult Data, bool IsSucess, ErrorCode ErrorCode, string Message = null)
    {
        public static RequestResult<TResult> Sucess(TResult data) => new RequestResult<TResult>(data, true, ErrorCode.NoError);

        public static RequestResult<TResult> Failure(ErrorCode errorCode, string message) => new RequestResult<TResult>(default(TResult), false, errorCode, message);
    }
}
