
using HtmlAgilityPack;
using DotaBuffClone.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DotaBuffClone.Common.Interfaces;
using System.Net;

namespace DotaBuffClone.Services
{
    public class DotabuffParsingService : IDotabuffParsingService
    {
        private readonly HttpClient _httpClient;

        public DotabuffParsingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Hero>> GetHeroesAsync()
        {
            var heroes = new List<Hero>();
            var url = "https://www.dotabuff.com/heroes";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);

            // Ќайти все узлы с классом ссылки на геро€
            var heroNodes = htmlDoc.DocumentNode.SelectNodes("//a[contains(@class, 'tw-group')]");
            if (heroNodes == null)
            {
                Console.WriteLine("Hero nodes not found");
                return heroes; // ¬озвращает пустой список, если ничего не найдено
            }

            foreach (var node in heroNodes)
            {
                // »звлечь им€ геро€
                var nameNode = node.SelectSingleNode(".//div[contains(@class, 'tw-text-white')]");
                var name = nameNode?.InnerText.Trim();

                // »звлечь ссылку на изображение геро€
                var imgNode = node.SelectSingleNode(".//img");
                var imageUrl = imgNode?.GetAttributeValue("src", "");

                // ƒобавл€ем базовый URL, если изображение Ч относительна€ ссылка
                if (!string.IsNullOrEmpty(imageUrl) && imageUrl.StartsWith("/"))
                {
                    imageUrl = "https://www.dotabuff.com" + imageUrl;
                }

                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(imageUrl))
                {
                    heroes.Add(new Hero { Name = name, ImageUrl = imageUrl });
                }
            }

            return heroes;
        }

        public async Task<List<Item>> GetItemsAsync(string dateFilter = "all")
        {
            var items = new List<Item>();
            var url = $"https://www.dotabuff.com/items?date={dateFilter}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);

            var itemNodes = htmlDoc.DocumentNode.SelectNodes("//tr[td[@class='cell-icon']]");

            if (itemNodes == null)
            {
                Console.WriteLine("Item nodes not found");
                return items;
            }

            foreach (var node in itemNodes)
            {
                var nameNode = node.SelectSingleNode(".//td[@class='cell-xlarge']/a");
                var name = nameNode != null ? WebUtility.HtmlDecode(nameNode.InnerText.Trim()) : "";

                var imgNode = node.SelectSingleNode(".//img");
                var imageUrl = imgNode?.GetAttributeValue("src", "");

                var timesUsedNode = node.SelectSingleNode(".//td[3]");
                var timesUsed = timesUsedNode != null ? timesUsedNode.InnerText.Trim().Replace(",", "") : "N/A";

                var useRateNode = node.SelectSingleNode(".//td[4]");
                var useRate = useRateNode != null ? Math.Round(double.Parse(useRateNode.GetAttributeValue("data-value", "0"), System.Globalization.CultureInfo.InvariantCulture), 2).ToString() + "%" : "N/A";

                var winrateNode = node.SelectSingleNode(".//td[5]");
                var winrate = winrateNode != null ? Math.Round(double.Parse(winrateNode.GetAttributeValue("data-value", "0"), System.Globalization.CultureInfo.InvariantCulture), 2).ToString() + "%" : "N/A";

                if (!string.IsNullOrEmpty(imageUrl) && imageUrl.StartsWith("/"))
                {
                    imageUrl = "https://www.dotabuff.com" + imageUrl;
                }

                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(imageUrl))
                {
                    items.Add(new Item
                    {
                        Name = name,
                        ImageUrl = imageUrl,
                        TimesUsed = timesUsed,
                        UseRate = useRate,
                        Winrate = winrate
                    });
                }
            }

            return items;
        }
    }
}
