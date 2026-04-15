using ExaminationSystem.Domin.Common.Result;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExaminationSystem.Domin.Common.Result
{
    public class Result 
    {
        public static Success Success => default;
        public static Created Created => default;
        public static Deleted Deleted => default;
        public static Updated Updated => default;
    }
    public class Result<TValue> : IResult<TValue> where TValue : AuditableEntity 
    {
        private readonly TValue? _value = default;

        private readonly List<Error>? _errors = null;

        public bool IsSuccess { get; }
        
         string Message { get; set; }

        public Result()
        {
            
        }

        public Result(TValue value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            _value = value;

            IsSuccess = true;
        }


        public Result(Error error)
        {
            _errors = [error];
        }

        public Result(List<Error> errors)
        {
            if (errors is null || errors.Count == 0)
            {
                throw new ArgumentException("Cannot create an ErrorOr<TValue> from an empty collection of errors. Provide at least one error.", nameof(errors));
            }

            _errors = errors;

            IsSuccess = false;
        }
        public bool IsError => !IsSuccess;

        public List<Error> Errors => IsError ? _errors! : [];

        public TValue Value => IsSuccess ? _value! : default!;

        public Error TopError => (_errors?.Count > 0) ? _errors[0] : default;

        public TNextValue Match<TNextValue>(Func<TValue, TNextValue> onValue, Func<List<Error>, TNextValue> onError)
            => IsSuccess ? onValue(Value!) : onError(Errors);

      

    }
    public readonly record struct Success;
    public readonly record struct Created;
    public readonly record struct Deleted;
    public readonly record struct Updated;
}
