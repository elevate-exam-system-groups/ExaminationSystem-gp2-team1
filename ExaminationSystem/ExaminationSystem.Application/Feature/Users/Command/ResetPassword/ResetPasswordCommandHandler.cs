
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Feature.Users.Command.ResetPassword
{
    public sealed class ResetPasswordCommandHandler(IConnectionMultiplexer redis,
        IGenericRepository<User> _userRepository
        ) : IRequestHandler<ResetPasswordCommand, Result<string>>
    {
        private readonly IDatabase _database = redis.GetDatabase();
        public async Task<Result<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.NewPassword != request.ConfirmPassword)
            {
                return new Result<string>(new Error(ErrorCode.PasswordNotMatch, "Passwords do not match"));
            }
            var key = $"ForgetPasswordToken:{request.Email}";
            var storedToken = _database.StringGet(key);
            if (storedToken.IsNullOrEmpty || storedToken != request.Token)
            {
                return new Result<string>(new Error(ErrorCode.NoResetRequest, "No Reset Request"));
            }
            var id = _userRepository.Find(u => u.Email == request.Email).Select(u => u.Id).FirstOrDefault();
            var user = new User
            {
                Id = id,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword)
            }
            ;
            var result = await _userRepository.UpdateIncludeAsync(user, x => x.PasswordHash);
            if (result)
            {
                _database.KeyDelete(key);
                return new Result<string>("Password reset successfully");
            }
            return new Result<string>(new Error(ErrorCode.PasswordResetFailed, "Failed to reset password"));

        }
    }
}
