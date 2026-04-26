using ExaminationSystem.Application.Common.Interfaces;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Domin.Entities;
using ExaminationSystem.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace ExaminationSystem.Infrastructure.Identity
{
    internal class UserService(IGenericRepository<UserRole> roleRepository , IHttpContextAccessor httpContextAccessor) : IUserService
    {
        private readonly IGenericRepository<UserRole> _roleRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
      
        /*public async Task<Guid> GetRoleIdByNameAsync(string roleName)
        {
            return await _roleRepository.GetAll()
                .Where(r => r.Name == roleName)
                .Select(r => r.Id)
                .FirstOrDefaultAsync<Guid>();
        }*/

        public Guid? GetUserId()
        {
            var currentUserIdString = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Guid? currentUserId = null;
            if (Guid.TryParse(currentUserIdString, out var parsedGuid))
            {
                currentUserId = parsedGuid;
            }

            return currentUserId;
        }
        public List<string> GetUserRole()
        {
            var roles = _httpContextAccessor.HttpContext?.User
                .FindAll(ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList() ?? new List<string>();
            return roles;
        }
    }
}
