using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private static readonly List<ItemDto> items = new()
        {
            new ItemDto(Guid.NewGuid(), "Potion", "Restores a small amount of HP", 5, DateTimeOffset.Now),
            new ItemDto(Guid.NewGuid(), "Antidote", "Cures poison", 7, DateTimeOffset.Now),
            new ItemDto(Guid.NewGuid(), "Bronze Sword", "Deals a small amount of damage", 20, DateTimeOffset.Now)
        };

        [HttpGet]
        public IEnumerable<ItemDto> Get()
        {
            return items;
        }

        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetById(Guid id)
        {
            var item = items.Where(i => i.Id == id).SingleOrDefault();
            if (item is null) return NotFound("Opps!😪Item was not found!");
            return item;
        }

        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto createItem)
        {
            var item = new ItemDto(Guid.NewGuid(), createItem.Name, createItem.Description, createItem.Price, DateTimeOffset.Now);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = items.Where(i => i.Id == id).SingleOrDefault();

            if (existingItem is null) return NotFound();

            var updateItem = existingItem with
            {
                Name = updateItemDto.Name,
                Description = updateItemDto.Description,
                Price = updateItemDto.Price
            };

            var index = items.FindIndex(existingItem => existingItem.Id == id);
            items[index] = updateItem;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteItem(Guid id)
        {
            var index = items.FindIndex(i => i.Id == id);

            if (index < 0) return NotFound("Opps!😪Item not found");

            items.RemoveAt(index);

            return NoContent();
        }

    }
}