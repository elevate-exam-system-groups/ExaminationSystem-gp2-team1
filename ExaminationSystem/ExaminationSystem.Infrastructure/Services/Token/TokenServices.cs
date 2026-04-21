using ExaminationSystem.Application.Common.Interfaces;
using ExaminationSystem.Application.Common.Models;
using ExaminationSystem.Application.Feature.Users;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Entities;
using ExaminationSystem.Infrastructure._UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ExaminationSystem.Infrastructure.Services.Token
{
    public class TokenService(IOptions<JwtSettings> jwtSettings,
        IGenericRepository<RefreshToken> _refreshRepository,
        IUnitOfWork _unitOfWork) : ITokenService
    {
       private readonly JwtSettings _jwtSettings = jwtSettings.Value;
        public async Task<Result<TokenResponse>> GenerateJwtTokenAsync(Guid id, string email, List<string> roles, CancellationToken ct = default)
        {
            var tokenResult = await CreateAsync(id, email, roles, ct);

            if (tokenResult.IsError)
            {
                return new Result<TokenResponse>(tokenResult.Errors);
            }

            return new Result<TokenResponse>(tokenResult.Value);
        }
        private async Task<Result<TokenResponse>> CreateAsync(Guid id, string email, List<string> roles, CancellationToken ct = default)
        {

            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var Claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Email, email),
                
            };
            foreach (string role in roles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var token = new SecurityTokenDescriptor
            {

                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Expires = DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
                Subject = new ClaimsIdentity(Claims),
                SigningCredentials = creds

            };

            var securityToken = new JwtSecurityTokenHandler().CreateToken(token);

            await _refreshRepository
                  .Find(rt => rt.UserId == id && rt.ExpiresAt < DateTime.UtcNow)
                  .ExecuteDeleteAsync();


            var rawToken = GenerateRefreshToken();
            var refreshToken = new RefreshToken
            {
                TokenHash = Convert.ToHexString(
                    SHA256.HashData(Encoding.UTF8.GetBytes(rawToken))),
                UserId = id,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            _refreshRepository.Add(refreshToken);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result == 0)
            {
                return new Result<TokenResponse>(new Error(ErrorCode.FaildSaveRefreshToken, "Failed to save refresh token"));
            }

            var tokenResponse = new TokenResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(securityToken),
                RefreshToken = refreshToken.TokenHash,
                ExpiresOnUtc = token.Expires.Value.ToUniversalTime()
            };

            return new Result<TokenResponse>(tokenResponse);
        }

        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
               .Replace("+", "-")
               .Replace("/", "_")
               .Replace("=", "");
        }

    }
}