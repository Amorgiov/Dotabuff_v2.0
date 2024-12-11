using DotaBuffClone.Models;

namespace DotaBuffClone.Common.Interfaces
{
    public interface IDotabuffParsingService
    {
        Task<List<Hero>> GetHeroesAsync();
        Task<List<Item>> GetItemsAsync(string date);
    }
}