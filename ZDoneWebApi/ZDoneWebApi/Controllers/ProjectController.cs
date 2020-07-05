using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZDoneWebApi.BusinessLogic.Interfaces;

namespace ZDoneWebApi.Controllers
{
    [Authorize]
    [EnableCors]
    [Route("api/projects")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectBl _projectBl;
        private readonly IUserBl _userBl;
        private string userId;
        private int projectId;

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

        public ProjectController(IProjectBl projectBl, IUserBl userBl)
        {
            _projectBl = projectBl;
            _userBl = userBl;
        }

        [HttpGet("items")]
        public async Task<ActionResult> Get()
        {
            //Console.WriteLine(HttpContext.Request.Cookies["refresh"]);
            (userId, projectId) = GetUserId();
            var allItems = await _projectBl.GetAllItems(userId);
            if (allItems == null)
                return NotFound();
            return Ok(allItems);
        }

        [HttpGet("items/{date}")]
        public async Task<ActionResult> GetItemsByDate(string date)
        {
            //Console.WriteLine(HttpContext.Request.Cookies["refresh"]);
            (userId, projectId) = GetUserId();
            var allItems = await _projectBl.GetDatedItems(date, userId);
            if (allItems == null)
                return NotFound();
            return Ok(allItems);
        }

        [HttpGet("items/completed")]
        public async Task<ActionResult> GetCompleted()
        {
            (userId, projectId) = GetUserId();
            var item = await _projectBl.GetCompletedItems(userId);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpGet("items/deleted")]
        public async Task<ActionResult> GetDeleted()
        {
            (userId, projectId) = GetUserId();
            var item = await _projectBl.GetDeletedItems(userId);
            if (item == null)
                return NotFound();
            return Ok(item);
        }
    }
}