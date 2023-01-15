using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Contracts;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Common;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> _itemsRepository;
        private readonly IPublishEndpoint publishEndpoint;

        public ItemsController(IRepository<Item> itemsRepository, IPublishEndpoint publishEndpoint)
        {
            _itemsRepository = itemsRepository;
            this.publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            var items = (await _itemsRepository.GetAllAsync())
                        .Select(item => item.AsDto());
            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = await _itemsRepository.GetAsync(id);
            if (item is null) return NotFound("Opps!😪Item was not found!");
            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createItem)
        {
            var item = new Item
            {
                Name = createItem.Name,
                Description = createItem.Description,
                Price = createItem.Price,
                CreatedDate = DateTimeOffset.Now
            };

            await _itemsRepository.CreateAsync(item);

            await publishEndpoint.Publish(new CatalogItemCreated(item.Id, item.Name, item.Description));

            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = await _itemsRepository.GetAsync(id);

            if (existingItem is null) return NotFound("Oops!😪Item not found.");


            existingItem.Name = updateItemDto.Name;
            existingItem.Description = updateItemDto.Description;
            existingItem.Price = updateItemDto.Price;

            await _itemsRepository.UpdateAsync(existingItem);

            await publishEndpoint.Publish(new CatalogItemUpdated(existingItem.Id, existingItem.Name, existingItem.Description));

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var existingItem = await _itemsRepository.GetAsync(id);

            if (existingItem is null) return NotFound("Oops!😪Item not found.");

            await _itemsRepository.RemoveAsync(existingItem.Id);

            await publishEndpoint.Publish(new CatalogItemDeleted(existingItem.Id));

            return NoContent();
        }

    }
}