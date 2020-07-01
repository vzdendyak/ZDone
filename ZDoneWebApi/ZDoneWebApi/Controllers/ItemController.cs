using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZDoneWebApi.BusinessLogic.Interfaces;
using ZDoneWebApi.Data.DTOs;

namespace ZDoneWebApi.Controllers
{
    [Authorize]
    [EnableCors]
    [Route("api/items")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemBl _itemBl;
        private readonly IUserBl _userBl;
        private string userId;
        private int projectId;

        public ItemController(IItemBl itemBl, IUserBl userBl)
        {
            _itemBl = itemBl;
            _userBl = userBl;
        }

        private (string, int) GetUserId()
        {
            var userId = User != null ? User.Claims.Single(c => c.Type == "id").Value : null;
            var projectId = User != null ? User.Claims.Single(c => c.Type == "projectId").Value : null;
            if (projectId != null)
            {
                return (userId, int.Parse(projectId));
            }
            return (userId, 0);
        }

        // GET: api/Item
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            //Console.WriteLine(HttpContext.Request.Cookies["refresh"]);
            (userId, projectId) = GetUserId();
            var allItems = await _itemBl.GetAllByProject(projectId, userId);
            if (allItems == null)
                return NotFound();
            return Ok(allItems);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            (userId, projectId) = GetUserId();
            if (!(await _userBl.IsHaveAccesToItem(id, userId)))
            {
                return Forbid();
            }
            var item = await _itemBl.ReadAsync(id, userId, projectId);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpGet("date/{date}")]
        public async Task<ActionResult> GetByDate(string date)
        {
            var item = await _itemBl.GetDateItems(date);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpGet("unlisted")]
        public async Task<ActionResult> GetUnlisted()
        {
            var item = await _itemBl.GetUnlistedItems();
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpGet("completed")]
        public async Task<ActionResult> GetCompleted()
        {
            var item = await _itemBl.GetCompletedItems();
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpGet("deleted")]
        public async Task<ActionResult> GetDeleted()
        {
            var item = await _itemBl.GetDeletedItems();
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
            (userId, projectId) = GetUserId();
            var result = await _itemBl.CreateAsync(item, userId);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.ItemDto);
        }

        // PUT: api/Item/5
        [HttpPut]
        public async Task<ActionResult> Put(ItemDto item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _itemBl.UpdateAsync(item);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _itemBl.DeleteAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok();
        }
    }
}