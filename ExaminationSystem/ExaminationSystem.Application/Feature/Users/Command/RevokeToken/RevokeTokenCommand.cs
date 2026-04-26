using ExaminationSystem.Domin.Common.Result;
using MediatR;

namespace ExaminationSystem.Application.Feature.Users.Command.RevokeToken
{
    public sealed record RevokeTokenCommand(string RawRefreshToken)
        : IRequest<Result<bool>>;
}
