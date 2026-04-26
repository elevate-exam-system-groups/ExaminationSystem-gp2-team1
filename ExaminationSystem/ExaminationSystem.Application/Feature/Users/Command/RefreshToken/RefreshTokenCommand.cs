using ExaminationSystem.Application.Feature.Users;
using ExaminationSystem.Domin.Common.Result;
using MediatR;

namespace ExaminationSystem.Application.Feature.Users.Command.RefreshToken
{
    public sealed record RefreshTokenCommand(string RawRefreshToken)
        : IRequest<Result<TokenResponse>>;
}
