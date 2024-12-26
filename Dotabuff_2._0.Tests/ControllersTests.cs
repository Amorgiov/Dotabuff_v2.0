using Dotabuff_2._0.Common.Interfaces;
using Dotabuff_2._0.Controllers;
using Dotabuff_2._0.Data;
using Dotabuff_2._0.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Dotabuff_2._0.Tests
{
    public class ControllersTests
    {
        private readonly Mock<IDotabuffParsingService> _mockParsingService;
        private readonly Mock<HttpClient> _mockHttpClient;
        private readonly ApplicationDbContext _context;

        public ControllersTests()
        {
            _mockParsingService = new Mock<IDotabuffParsingService>();
            _mockHttpClient = new Mock<HttpClient>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "dotabuff_clone")
                .Options;
            _context = new ApplicationDbContext(options);
        }

        [Fact]
        public async Task HeroesController_Index_ReturnsViewResult_WithHeroes()
        {
            // Arrange
            var heroes = new List<Hero> { new Hero { Name = "Axe", ImageUrl = "url" } };
            _mockParsingService.Setup(service => service.GetHeroesAsync()).ReturnsAsync(heroes);
            var controller = new HeroesController(_mockParsingService.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Hero>>(viewResult.ViewData.Model);
            model.Should().HaveCount(1);
        }

        [Fact]
        public async Task ItemsController_Index_ReturnsViewResult_WithItems()
        {
            // Arrange
            var items = new List<Item> { new Item { Name = "Blink Dagger", ImageUrl = "url" } };
            _mockParsingService.Setup(service => service.GetItemsAsync(It.IsAny<string>())).ReturnsAsync(items);
            var controller = new ItemsController(_mockParsingService.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Item>>(viewResult.ViewData.Model);
            model.Should().HaveCount(1);
        }

        [Fact]
        public async Task AdminController_UpdateHeroes_CallsParsingServiceAndRedirects()
        {
            // Arrange
            var controller = new AdminController(_mockParsingService.Object);

            // Act
            var result = await controller.UpdateHeroes();

            // Assert
            _mockParsingService.Verify(service => service.ParseAndSaveHeroesAsync(), Times.Once);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            redirectResult.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task AdminController_UpdateItems_CallsParsingServiceAndRedirects()
        {
            // Arrange
            var controller = new AdminController(_mockParsingService.Object);

            // Act
            var result = await controller.UpdateItems();

            // Assert
            _mockParsingService.Verify(service => service.ParseAndSaveItemsAsync(It.IsAny<string>()), Times.Once);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            redirectResult.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task ItemsController_Index_ReturnsEmptyView_WhenExceptionOccurs()
        {
            // Arrange
            _mockParsingService
                .Setup(service => service.GetItemsAsync(It.IsAny<string>()))
                .ThrowsAsync(new System.Exception("Error"));
            var controller = new ItemsController(_mockParsingService.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Item>>(viewResult.ViewData.Model);
            model.Should().BeEmpty();
        }

        [Fact]
        public async Task MatchesController_Index_ReturnsFilteredMatches()
        {
            // Arrange
            var matches = new List<Models.Match>
            {
                new Models.Match
                {
                    League = "League1",
                    MatchId = "12345",
                    Date = "2023-01-01",
                    Series = "Best of 3",
                    RadiantTeam = "Team A",
                    DireTeam = "Team B",
                    Duration = "40:00"
                },
                new Models.Match
                {
                    League = "League2",
                    MatchId = "67890",
                    Date = "2023-01-02",
                    Series = "Best of 5",
                    RadiantTeam = "Team C",
                    DireTeam = "Team D",
                    Duration = "50:00"
                }
            };

            _context.Matches.AddRange(matches);
            await _context.SaveChangesAsync();

            var controller = new MatchesController(_mockParsingService.Object, _context);

            // Act
            var result = await controller.Index(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Models.Match>>(viewResult.ViewData.Model);
            model.Should().HaveCount(2);
        }


    }
}

