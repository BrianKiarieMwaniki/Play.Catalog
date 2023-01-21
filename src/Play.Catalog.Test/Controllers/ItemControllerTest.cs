using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Play.Catalog.Service;
using Play.Catalog.Service.Dtos;
using Xunit;

namespace Play.Catalog.Test.Controllers
{

    public class ItemControllerTest
    {
        private const string baseApiUrl = "/api/items";
        private readonly HttpClient _httpClient;
        public ItemControllerTest()
        {
            var webAppFactory = new WebApplicationFactory<Program>();

            _httpClient = webAppFactory.CreateDefaultClient();
        }

        [Fact]
        public async void GetAllItemsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<ItemDto>>("/api/items");

            Assert.NotNull(response);

            var itemDto = response?.FirstOrDefault();

            Assert.Equal(itemDto.Price, 5);
            Assert.Equal("Potion", itemDto.Name);
            Assert.Equal("3b7728a5-358c-493d-9d74-2b83ea32c802", itemDto.Id.ToString());
        }

        [Fact]
        public async void GetItemByIdAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ItemDto>($"{baseApiUrl}/3b7728a5-358c-493d-9d74-2b83ea32c802");

            var itemDto = response ?? null;

            Assert.NotNull(itemDto);
            Assert.Equal(itemDto.Price, 5);
            Assert.Equal("Potion", itemDto.Name);
            Assert.Equal("3b7728a5-358c-493d-9d74-2b83ea32c802", itemDto.Id.ToString());

        }
    }
}