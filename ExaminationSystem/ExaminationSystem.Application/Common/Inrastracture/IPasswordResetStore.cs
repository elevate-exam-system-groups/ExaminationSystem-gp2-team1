namespace ExaminationSystem.Application.Common.Inrastracture;
public interface IPasswordResetStore
{
    Task SaveAsync(string email, string hashedToken, CancellationToken ct = default);
    Task<string?> GetAsync(string email, CancellationToken ct = default);
    Task DeleteAsync(string email, CancellationToken ct = default);
}
