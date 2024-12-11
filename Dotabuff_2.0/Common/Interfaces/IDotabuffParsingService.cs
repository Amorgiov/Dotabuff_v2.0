using Dotabuff_2._0.Models;

namespace Dotabuff_2._0.Common.Interfaces
{
    public interface IDotabuffParsingService
    {
        Task<List<Hero>> GetHeroesAsync();
        Task<List<Item>> GetItemsAsync(string dateFilter = "all");
        Task ParseAndSaveAllAsync(string dateFilter = "all");
        Task ParseAndSaveHeroesAsync();
        Task ParseAndSaveItemsAsync(string dateFilter = "all");
    }
}