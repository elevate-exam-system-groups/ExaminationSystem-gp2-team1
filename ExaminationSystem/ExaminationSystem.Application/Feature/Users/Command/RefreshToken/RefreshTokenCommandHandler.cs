using ExaminationSystem.Application.Common.Interfaces;
using ExaminationSystem.Application.Feature.Users;
using ExaminationSystem.Domin.Common.Result;
using MediatR;

namespace ExaminationSystem.Application.Feature.Users.Command.RefreshToken
{
    public sealed class RefreshTokenCommandHandler(ITokenService _tokenService)
        : IRequestHandler<RefreshTokenCommand, Result<TokenResponse>>
    {
        public async Task<Result<TokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            return await _tokenService.RefreshTokenAsync(request.RawRefreshToken, cancellationToken);
        }
    }
}
