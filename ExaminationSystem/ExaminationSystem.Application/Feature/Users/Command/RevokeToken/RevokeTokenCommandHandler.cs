using ExaminationSystem.Application.Common.Interfaces;
using ExaminationSystem.Domin.Common.Result;
using MediatR;

namespace ExaminationSystem.Application.Feature.Users.Command.RevokeToken
{
    public sealed class RevokeTokenCommandHandler(ITokenService _tokenService)
        : IRequestHandler<RevokeTokenCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            return await _tokenService.RevokeTokenAsync(request.RawRefreshToken, cancellationToken);
        }
    }
}
