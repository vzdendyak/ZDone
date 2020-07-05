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
    [EnableCors]
    [Route("api/folders")]
    [ApiController]
    [Authorize]
    public class FolderController : ControllerBase
    {
        private readonly IFolderBl _folderBl;
        private readonly IUserBl _userBl;
        private string userId;
        private int projectId;

        public FolderController(IFolderBl folderBl, IUserBl userBl)
        {
            _folderBl = folderBl;
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
        public async Task<IActionResult> Get()
        {
            (userId, projectId) = GetUserId();
            if (!(await _userBl.IsHaveProjectPermission(userId, projectId)))
            {
                return Forbid();
            }
            var allItems = await _folderBl.GetByProjectIdAsync(projectId, userId);
            if (allItems == null)
                return NotFound();
            return Ok(allItems);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            (userId, projectId) = GetUserId();
            if (!(await _userBl.isHaveAccessToFolder(id, userId)))
            {
                return Forbid();
            }
            var folder = await _folderBl.ReadAsync(id);
            if (folder == null)
                return NotFound();
            return Ok(folder);
        }

        [HttpGet("{id}/lists")]
        public async Task<IActionResult> GetLists(int id)
        {
            (userId, projectId) = GetUserId();
            if (!(await _userBl.isHaveAccessToFolder(id, userId)))
            {
                return Forbid();
            }
            var lists = await _folderBl.GetRelatedLists(id, userId);
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
        [HttpPut]
        public async Task<IActionResult> Put(FolderDto folder)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            (userId, projectId) = GetUserId();
            var result = await _folderBl.UpdateAsync(folder, userId);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            (userId, projectId) = GetUserId();

            var result = await _folderBl.DeleteAsync(id, userId);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok();
        }
    }
}