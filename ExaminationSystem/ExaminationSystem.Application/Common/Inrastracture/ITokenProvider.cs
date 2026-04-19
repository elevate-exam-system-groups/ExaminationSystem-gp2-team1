

using ExaminationSystem.Application.Feature.User;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Entities;

namespace ExaminationSystem.Application.Common.Interfaces;

public interface ITokenProvider
{
   Task<Result<TokenResponse>> GenerateJwtTokenAsync(User user, CancellationToken ct = default);

   /* ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);*/
}