using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ExaminationSystem.Domin.Comman.Result
{
    public  class Result<T>  where T : AuditableEntity
    {
        public T? Data { get; set; }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public ErrorCode? ErrorCode { get; set; }
        protected Result(T? data, bool isSuccess, string message, ErrorCode? errorCode)
        {
            Data = data;
            IsSuccess = isSuccess;
            Message = message;
            ErrorCode = errorCode;
        }
        public static Result<T> Success(T data, string? message = null)
        {
            return new Result<T>(data, true, message ?? "Success", null);
        }

        public static Result<T> Fail(ErrorCode? errorCode, string message)
        {
            return new Result<T>(default, false, message, errorCode);
        }
       /* public static Result<T> ValidationFail(ValidationResult validationResult)
        {
            var errorMessage = string.Join("; ",
                validationResult.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));

            return Fail(
                 Domain.Enums.ErrorCode.ValidationError, $"Validation Failed \n {errorMessage}");
        }*/
    }
}
