using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZDoneWebApi.BusinessLogic.Interfaces;
using ZDoneWebApi.Data.DTOs;

namespace ZDoneWebApi.Controllers
{
    [EnableCors]
    [Route("api/items")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemBl _itemBl;

        public ItemController(IItemBl itemBl)
        {
            _itemBl = itemBl;
        }

        // GET: api/Item
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var allItems = await _itemBl.GetAllAsync();
            if (allItems == null)
                return NotFound();
            return Ok(allItems);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var item = await _itemBl.ReadAsync(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        // POST: api/Item
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ItemDto item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _itemBl.CreateAsync(item);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.ItemDto);
        }

        // PUT: api/Item/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, ItemDto item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _itemBl.UpdateAsync(item);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _itemBl.DeleteAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }
    }
}