namespace ExaminationSystem.Application.Common.Interfaces;

public interface IUserService
{
    Guid? GetUserId();
    Task<Guid> GetRoleIdByNameAsync(string roleName);
}