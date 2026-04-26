

using ExaminationSystem.Application.Feature.Users;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Entities;

namespace ExaminationSystem.Application.Common.Interfaces;

public interface ITokenService
{
   Task<Result<TokenResponse>> GenerateJwtTokenAsync(Guid id, string email, List<string> roles, CancellationToken ct = default);
   Task<Result<TokenResponse>> RefreshTokenAsync(string rawRefreshToken, CancellationToken ct = default);
   Task<Result<bool>> RevokeTokenAsync(string rawRefreshToken, CancellationToken ct = default);

   /* ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);*/
}