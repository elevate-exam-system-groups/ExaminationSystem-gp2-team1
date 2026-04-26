using ExaminationSystem.Application.Common.Inrastracture;
using StackExchange.Redis;
namespace ExaminationSystem.Infrastructure.Services;
public class RedisPasswordResetStore(IConnectionMultiplexer redis)
    : IPasswordResetStore
{
    private readonly IDatabase _db = redis.GetDatabase();

    public async Task SaveAsync(string email, string hashedToken, CancellationToken ct = default)
        => await _db.StringSetAsync($"ForgetPasswordToken:{email}", hashedToken, TimeSpan.FromHours(1));

    public async Task<string?> GetAsync(string email, CancellationToken ct = default)
    {
        var value = await _db.StringGetAsync($"ForgetPasswordToken:{email}");
        return value.HasValue ? value.ToString() : null;
    }

    public async Task DeleteAsync(string email, CancellationToken ct = default)
        => await _db.KeyDeleteAsync($"ForgetPasswordToken:{email}");
}
