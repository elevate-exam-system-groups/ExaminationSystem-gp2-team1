namespace ExaminationSystem.Application.Abstractions
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan expiration);
        void Remove(string key);
    }
}
