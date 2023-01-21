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
        private readonly WebApplicationFactory<Program> _webAppFactory;
        public ItemControllerTest()
        {
          _webAppFactory = new WebApplicationFactory<Program>();
        }

        [Fact]
        public async void GetAllItemsAsync()
        {
            using(var _httpClient = _webAppFactory.CreateDefaultClient())
            {

                var response = await _httpClient.GetFromJsonAsync<List<ItemDto>>("/api/items");

                Assert.NotNull(response);

                var itemDto = response?.FirstOrDefault();

                Assert.Equal(itemDto.Price, 5);
                Assert.Equal("Potion", itemDto.Name);
                Assert.Equal("3b7728a5-358c-493d-9d74-2b83ea32c802", itemDto.Id.ToString());
            }
        }

        [Fact]
        public async void GetItemByIdAsync()
        {
            using(var httpClient = _webAppFactory.CreateDefaultClient())
            {
                var response = await httpClient.GetFromJsonAsync<ItemDto>($"{baseApiUrl}/3b7728a5-358c-493d-9d74-2b83ea32c802");

                var itemDto = response ?? null;

                Assert.NotNull(itemDto);
                Assert.Equal(itemDto.Price, 5);
                Assert.Equal("Potion", itemDto.Name);
                Assert.Equal("3b7728a5-358c-493d-9d74-2b83ea32c802", itemDto.Id.ToString());
            }

        }
    }
}