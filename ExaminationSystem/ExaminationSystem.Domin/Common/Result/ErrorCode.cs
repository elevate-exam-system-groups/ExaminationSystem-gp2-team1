using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Domin.Common.Result
{
    public enum ErrorCode
    {
        ValidationError,
        EmailIsAlreadyUsed,
        RegistrationFailed,
        NoError = 0,
        /// for Start Quiz 
        ///   Limit reached: 403 
        ///   Student already has an in-progress attempt for this quiz
        LimitReached = 403,
        NotFound = 404,
        ExistingAttempt = 409,
        OtpExpired = 410,
        OtpAlreadyUsed = 411,
        OtpInvalid = 412,
        OtpRateLimitExceeded = 413,
        OtpMaxAttemptsExceeded = 414,
        NotificationFailed = 415,
        InCorrectPassword = 416,
    }
}
