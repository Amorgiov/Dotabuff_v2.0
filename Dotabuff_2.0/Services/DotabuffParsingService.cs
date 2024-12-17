using Dotabuff_2._0.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Globalization;
using Dotabuff_2._0.Common.Interfaces;
using Dotabuff_2._0.Data;
using HtmlAgilityPack;

namespace Dotabuff_2._0.Services
{
    public class DotabuffParsingService : IDotabuffParsingService
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DotabuffParsingService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DotabuffParsingService(HttpClient httpClient, ApplicationDbContext context, ILogger<DotabuffParsingService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        // Метод для получения героев из БД или через парсинг
        public async Task<List<Hero>> GetHeroesAsync()
        {
            if (await _context.Heroes.AnyAsync())
            {
                _logger.LogInformation("Загрузка героев из базы данных.");
                return await _context.Heroes.ToListAsync();
            }

            _logger.LogInformation("Данные героев отсутствуют в базе данных. Начинается парсинг.");
            await ParseAndSaveHeroesAsync();
            return await _context.Heroes.ToListAsync();
        }

        // Метод для парсинга и сохранения героев
        public async Task ParseAndSaveHeroesAsync()
        {
            _logger.LogInformation("Начало парсинга героев.");

            var heroes = new List<Hero>();
            var url = "https://www.dotabuff.com/heroes";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);

            var heroNodes = htmlDoc.DocumentNode.SelectNodes("//a[contains(@class, 'tw-group')]");

            if (heroNodes != null)
            {
                foreach (var node in heroNodes)
                {
                    var nameNode = node.SelectSingleNode(".//div[contains(@class, 'tw-text-white')]");
                    var name = nameNode?.InnerText.Trim();

                    var imgNode = node.SelectSingleNode(".//img");
                    var imageUrl = imgNode?.GetAttributeValue("src", "");

                    if (!string.IsNullOrEmpty(imageUrl) && imageUrl.StartsWith("/"))
                    {
                        imageUrl = "https://www.dotabuff.com" + imageUrl;
                    }

                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(imageUrl))
                    {
                        heroes.Add(new Hero { Name = name, ImageUrl = imageUrl });
                    }
                }

                _logger.LogInformation("Герои успешно спарсены. Сохранение в базу данных.");
                _context.Heroes.RemoveRange(_context.Heroes);
                _context.Heroes.AddRange(heroes);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Герои успешно сохранены в базе данных.");
            }
            else
            {
                _logger.LogWarning("Не удалось найти героев на странице для парсинга.");
            }
        }

        // Метод для получения предметов из БД или через парсинг
        public async Task<List<Item>> GetItemsAsync(string dateFilter = "all")
        {
            _logger.LogInformation("Данные предметов отсутствуют в базе данных. Начинается парсинг.");
            await ParseAndSaveItemsAsync(dateFilter);
            return await _context.Items.ToListAsync();
        }

        // Метод для парсинга и сохранения предметов
        public async Task ParseAndSaveItemsAsync(string dateFilter = "all")
        {
            _logger.LogInformation($"Начало парсинга предметов для фильтра '{dateFilter}'.");

            var items = new List<Item>();
            var url = $"https://www.dotabuff.com/items?date={dateFilter}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);

            var itemNodes = htmlDoc.DocumentNode.SelectNodes("//tr[td[@class='cell-icon']]");

            if (itemNodes != null)
            {
                foreach (var node in itemNodes)
                {
                    var nameNode = node.SelectSingleNode(".//td[@class='cell-xlarge']/a");
                    var name = nameNode != null ? WebUtility.HtmlDecode(nameNode.InnerText.Trim()) : "";

                    var imgNode = node.SelectSingleNode(".//img");
                    var imageUrl = imgNode?.GetAttributeValue("src", "");

                    var timesUsedNode = node.SelectSingleNode(".//td[3]");
                    var timesUsed = timesUsedNode?.InnerText.Trim().Replace(",", "") ?? "N/A";

                    var useRateNode = node.SelectSingleNode(".//td[4]");
                    var useRate = useRateNode != null
                        ? Math.Round(double.Parse(useRateNode.GetAttributeValue("data-value", "0"), CultureInfo.InvariantCulture), 2).ToString(CultureInfo.InvariantCulture) + "%"
                        : "N/A";

                    var winrateNode = node.SelectSingleNode(".//td[5]");
                    var winrate = winrateNode != null
                        ? Math.Round(double.Parse(winrateNode.GetAttributeValue("data-value", "0"), CultureInfo.InvariantCulture), 2).ToString(CultureInfo.InvariantCulture) + "%"
                        : "N/A";

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

                _logger.LogInformation("Предметы успешно спарсены. Сохранение в базу данных.");
                _context.Items.RemoveRange(_context.Items);
                _context.Items.AddRange(items);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Предметы успешно сохранены в базе данных.");
            }
            else
            {
                _logger.LogWarning("Не удалось найти предметы на странице для парсинга.");
            }
        }

        public async Task<List<Match>> GetMatchesAsync(int page = 1, string dateFilter = "all")
        {
            _logger.LogInformation("Данные матчей отсутствуют в базе данных. Начинается парсинг.");
            await ParseAndSaveMatchesAsync(page, dateFilter);
            return await _context.Matches.ToListAsync();
        }

        public async Task ParseAndSaveMatchesAsync(int page = 1, string dateFilter = "all")
        {
            _logger.LogInformation($"Начало парсинга матчей для фильтра '{dateFilter}' на странице {page}.");

            var matches = new List<Match>();
            var url = $"https://www.dotabuff.com/esports/matches?date={dateFilter}&page={page}";

            _logger.LogInformation($"Парсинг страницы: {url}");

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.9");

            HttpResponseMessage response;

            try
            {
                response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Ошибка при запросе страницы {page}: {response.StatusCode}");
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при выполнении запроса: {ex.Message}");

                // Перенаправление на первую страницу
                _httpContextAccessor.HttpContext.Response.Redirect("https://localhost:5000/Matches?page=1");
                return;
            }

            var content = await response.Content.ReadAsStringAsync();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);

            var matchRows = htmlDoc.DocumentNode.SelectNodes("//table[contains(@class, 'recent-esports-matches')]//tbody//tr");
            if (matchRows != null && matchRows.Count > 0)
            {
                foreach (var row in matchRows)
                {
                    try
                    {
                        var leagueNode = row.SelectSingleNode(".//a[contains(@class, 'esports-league')]//span[contains(@class, 'league-text')]");
                        var matchIdNode = row.SelectSingleNode(".//td[3]//a");
                        var dateNode = row.SelectSingleNode(".//td[3]//time");
                        var seriesNode = row.SelectSingleNode(".//td[contains(@class, 'r-none-mobile')]//a");
                        var seriesAdditionalNode = row.SelectSingleNode(".//td[contains(@class, 'r-none-mobile')]//small");

                        // Ячейки команд. Предположим, что:
                        // [0] - Dire Team (winner или loser)
                        // [1] - Radiant Team
                        // В зависимости от того, как структурирована страница, возможно придется поменять местами индексы.
                        var teamCells = row.SelectNodes(".//td[contains(@class, 'cell-xlarge r-none-mobile')]");

                        var direCell = teamCells?[0];
                        var radiantCell = teamCells?[1];

                        // Для Series
                        var series = seriesNode?.InnerText.Trim();
                        var seriesAdditional = seriesAdditionalNode?.InnerText.Trim();
                        var fullSeries = !string.IsNullOrEmpty(seriesAdditional) ? $"{series} ({seriesAdditional})" : series;

                        // Извлекаем название команд
                        var radiantTeamNode = radiantCell?.SelectSingleNode(".//span[contains(@class, 'team-text-full')]");
                        var direTeamNode = direCell?.SelectSingleNode(".//span[contains(@class, 'team-text-full')]");

                        var match = new Match
                        {
                            League = leagueNode?.InnerText.Trim(),
                            MatchId = matchIdNode?.InnerText.Trim(),
                            Date = dateNode?.GetAttributeValue("datetime", string.Empty).Trim(),
                            Series = fullSeries,
                            RadiantTeam = radiantTeamNode?.InnerText.Trim(),
                            DireTeam = direTeamNode?.InnerText.Trim(),
                            Duration = row.SelectSingleNode(".//td[last()]")?.InnerText.Split('\n').FirstOrDefault()?.Trim()
                        };

                        // Парсим иконки героев для Radiant
                        var radiantHeroImages = radiantCell?.SelectNodes(".//img[contains(@class, 'img-icon')]");
                        if (radiantHeroImages != null)
                        {
                            foreach (var imgNode in radiantHeroImages)
                            {
                                var src = "https://www.dotabuff.com/" + imgNode.GetAttributeValue("src", "");
                                if (!string.IsNullOrEmpty(src))
                                {
                                    match.RadiantHeroes.Add(src);
                                }
                            }
                        }

                        // Парсим иконки героев для Dire
                        var direHeroImages = direCell?.SelectNodes(".//img[contains(@class, 'img-icon')]");
                        if (direHeroImages != null)
                        {
                            foreach (var imgNode in direHeroImages)
                            {
                                var src = "https://www.dotabuff.com/" + imgNode.GetAttributeValue("src", "");
                                if (!string.IsNullOrEmpty(src))
                                {
                                    match.DireHeroes.Add(src);
                                }
                            }
                        }

                        matches.Add(match);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"Ошибка при обработке строки матча: {ex.Message}");
                        _httpContextAccessor.HttpContext.Response.Redirect("https://localhost:7109/Matches?page=1");
                    }
                }

                _logger.LogInformation($"Спарсено {matches.Count} матчей на странице {page}.");

                _context.Matches.RemoveRange(_context.Matches);
                _context.Matches.AddRange(matches);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Матчи успешно сохранены в базе данных.");
            }
            else
            {
                _logger.LogWarning($"Матчи не найдены на странице {page}. Перенаправление на первую страницу.");
            }

        }




        // Метод для обновления и героев, и предметов
        public async Task ParseAndSaveAllAsync(string dateFilter = "all")
        {
            await ParseAndSaveHeroesAsync();
            await ParseAndSaveItemsAsync(dateFilter);
        }
    }
}
