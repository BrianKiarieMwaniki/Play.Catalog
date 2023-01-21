using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Play.Catalog.Service;
using Play.Catalog.Service.Entities;
using Play.Common;
using Play.Common.MongoDB;
using Xunit;

namespace Play.Catalog.Test.Data
{
    public class ItemsRepositoryTest
    {
        private readonly WebApplicationFactory<Program> app;

        public ItemsRepositoryTest()
        {
            app = new WebApplicationFactory<Program>()
                        .WithWebHostBuilder(builder =>{
                            builder.ConfigureServices(services =>{
                                services.AddMongo().AddMongoRepository<Item>("items");
                            });
                        });
        }

        [Fact]
        public async void GetAllItems()
        {
            using(var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var itemsRepository = services.GetRequiredService<IRepository<Item>>();

                var items = await itemsRepository.GetAllAsync();

                Assert.NotEmpty(items);
            }
        }
    }
}