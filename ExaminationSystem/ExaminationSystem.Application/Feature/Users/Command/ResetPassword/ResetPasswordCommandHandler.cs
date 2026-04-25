
using ExaminationSystem.Application.Common.Inrastracture;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ExaminationSystem.Application.Feature.Users.Command.ResetPassword
{
    public sealed class ResetPasswordCommandHandler(IPasswordResetStore _passwordResetStore,
        IGenericRepository<User> _userRepository,
        IResetTokenService _resetTokenService
        ) : IRequestHandler<ResetPasswordCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.NewPassword != request.ConfirmPassword)
            {
                return new Result<string>(new Error(ErrorCode.PasswordNotMatch, "Passwords do not match"));
            }
            var storedToken = await _passwordResetStore.GetAsync(request.Email, cancellationToken);
            var isMatch = _resetTokenService.VerifyToken(request.Token, storedToken!);

            if (storedToken == null || !isMatch)
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
                await _passwordResetStore.DeleteAsync(request.Email, cancellationToken);
                return new Result<string>("Password reset successfully");
            }
            return new Result<string>(new Error(ErrorCode.PasswordResetFailed, "Failed to reset password"));

        }
    }
}
