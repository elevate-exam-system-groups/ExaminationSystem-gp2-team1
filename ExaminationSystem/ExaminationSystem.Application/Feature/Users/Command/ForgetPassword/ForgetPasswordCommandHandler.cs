using ExaminationSystem.Application.Common.Inrastracture;
using ExaminationSystem.Application.Common.Interfaces;
using ExaminationSystem.Application.Common.Models;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Entities;
using MediatR;
using Microsoft.AspNetCore.Routing;
using StackExchange.Redis;
using System;
using System.Collections.Generic;


namespace ExaminationSystem.Application.Feature.Users.Command.ForgetPassword;

public sealed class ForgetPasswordCommandHandler(IGenericRepository<User> _userRepository,
    INotificationService _notification , 
    IResetTokenService _resetTokenService,
    IConnectionMultiplexer _redis
    ) : IRequestHandler<ForgetPasswordCommand, Result<string>>
{
    private readonly IDatabase _database = _redis.GetDatabase();
    public async Task<Result<string>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
    {
        var userExists = await _userRepository.ExistsAsync(u => u.Email == request.Email);
        if (!userExists)
        {
            return new Result<string>(new Error(ErrorCode.UserNotFound, "User with the provided email does not exist."));
        }

        
        var hashedToken = _resetTokenService.HashToken();
        var key = $"ForgetPasswordToken:{request.Email}";
        await _database.StringSetAsync(key, hashedToken, TimeSpan.FromHours(1));

        var resetLink = new Link(request.VerificationUri, hashedToken, request.Email);
        var result = await _notification.SendForgetPasswordEmailAsync(request.Email, resetLink);

        return result.IsSuccess 
            ? new Result<string>("Password reset email sent successfully. Check your email for the reset link.")
            : result;
    }
}

