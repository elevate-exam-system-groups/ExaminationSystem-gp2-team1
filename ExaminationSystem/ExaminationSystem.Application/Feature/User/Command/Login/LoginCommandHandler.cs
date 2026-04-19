using ExaminationSystem.Application.Common.Interfaces;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Feature.User.Command.Login
{
    public sealed class LoginCommandHandler(IGenericRepository<Entities.User> repo,
        ITokenProvider tokenProvider ) : IRequestHandler<LoginCommand, Result<TokenResponse>>
    {
        private readonly IGenericRepository<Entities.User> _repo = repo;
        private readonly ITokenProvider _tokenProvider = tokenProvider;
        public async  Task<Result<TokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = _repo.Find(u => u.Email == request.Email).FirstOrDefault();
            if (user == null)
            {
                return new Result<TokenResponse>(new Error(ErrorCode.InCorrectPassword, "The email or password is incorrect.") );
            }
            if(!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return new Result<TokenResponse>(new Error(ErrorCode.InCorrectPassword, "The email or password is incorrect.") );
            }

            var token = await _tokenProvider.GenerateJwtTokenAsync(user, cancellationToken);
            return new Result<TokenResponse>(token.Value);
        }
    }
}
