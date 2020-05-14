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
    [Route("api/folders")]
    [ApiController]
    public class FolderController : ControllerBase
    {
        private readonly IFolderBl _folderBl;

        public FolderController(IFolderBl folderBl)
        {
            _folderBl = folderBl;
        }

        // GET: api/Item
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var allItems = await _folderBl.GetAllAsync();
            if (allItems == null)
                return NotFound();
            return Ok(allItems);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var folder = await _folderBl.ReadAsync(id);
            if (folder == null)
                return NotFound();
            return Ok(folder);
        }

        [HttpGet("{id}/lists")]
        public async Task<IActionResult> GetLists(int id)
        {
            var lists = await _folderBl.GetRelatedLists(id);
            if (lists == null)
                return NotFound();
            return Ok(lists);
        }

        // POST: api/Item
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FolderDto folder)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _folderBl.CreateAsync(folder);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.id);
        }

        // PUT: api/Item/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, FolderDto folder)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _folderBl.UpdateAsync(folder);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _folderBl.DeleteAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }
    }
}