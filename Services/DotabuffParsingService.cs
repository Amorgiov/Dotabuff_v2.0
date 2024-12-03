
using HtmlAgilityPack;
using DotaBuffClone.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DotaBuffClone.Services
{
    public class DotabuffParsingService
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

        public async Task<List<Item>> GetItemsAsync()
        {
            var items = new List<Item>();
            var url = "https://www.dotabuff.com/items";

            var response = await _httpClient.GetStringAsync(url);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(response);

            var itemNodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='item-grid']//a");

            foreach (var node in itemNodes)
            {
                var item = new Item
                {
                    Name = node.GetAttributeValue("title", ""),
                    ImageUrl = node.SelectSingleNode(".//img").GetAttributeValue("src", "")
                };
                items.Add(item);
            }

            return items;
        }
    }
}
