

using ExaminationSystem.Application.Feature.User;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Entities;

namespace ExaminationSystem.Application.Common.Interfaces;

public interface ITokenService
{
   Task<Result<TokenResponse>> GenerateJwtTokenAsync(Guid id, string email, List<string> roles, CancellationToken ct = default);

   /* ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);*/
}