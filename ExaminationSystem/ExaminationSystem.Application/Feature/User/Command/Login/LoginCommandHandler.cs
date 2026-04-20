using ExaminationSystem.Application.Common.Interfaces;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Feature.User.Dto;

namespace ExaminationSystem.Application.Feature.User.Command.Login
{
    public sealed class LoginCommandHandler(IGenericRepository<Entities.User> _repo,
        ITokenService _tokenService) : IRequestHandler<LoginCommand, Result<TokenResponse>>
    {

        public async Task<Result<TokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _repo
            .Find(u => u.Email == request.Email)
            .Select(u => new UserLoginDto(
                 u.Id,
                 u.Email,
                 u.PasswordHash,
                 u.Roles.Select(r => r.Role.Name.ToString()).ToList()
                ))
            .FirstOrDefaultAsync(cancellationToken); if (user == null)
            {
                return new Result<TokenResponse>(new Error(ErrorCode.InCorrectPassword, "The email or password is incorrect."));
            }
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return new Result<TokenResponse>(new Error(ErrorCode.InCorrectPassword, "The email or password is incorrect."));
            }
            var role = user.Roles;

            var tokenResult = await _tokenService.GenerateJwtTokenAsync(user.Id, user.Email, role, cancellationToken);
            if (tokenResult.IsError)
            {
                return new Result<TokenResponse>(tokenResult.Errors);
            }
            return new Result<TokenResponse>(tokenResult.Value);
        }
    }
}
