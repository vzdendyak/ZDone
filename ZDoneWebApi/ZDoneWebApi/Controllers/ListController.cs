using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using ZDoneWebApi.BusinessLogic.Interfaces;
using ZDoneWebApi.Data.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZDoneWebApi.Controllers
{
    [EnableCors]
    [Route("api/lists")]
    [ApiController]
    [Authorize]
    public class ListController : ControllerBase
    {
        private readonly IListBl _listBl;
        private readonly IUserBl _userBl;
        private string userId;
        private int projectId;

        public ListController(IListBl listBl, IUserBl userBl)
        {
            _listBl = listBl;
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
            (userId, projectId) = GetUserId();
            if (!(await _userBl.IsHaveProjectPermission(userId, projectId)))
            {
                return Forbid();
            }
            var allItems = await _listBl.GetAllAsync();

            if (allItems == null)
                return NotFound();
            return Ok(allItems);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            (userId, projectId) = GetUserId();
            if (!(await _userBl.IsHaveAccessToList(id, userId)))
            {
                return Forbid();
            }
            var list = await _listBl.ReadAsync(id, userId);
            if (list == null)
                return NotFound();
            return Ok(list);
        }

        [HttpGet("{id}/items")]
        public async Task<ActionResult> GetItems(int id)
        {
            (userId, projectId) = GetUserId();
            if (!(await _userBl.IsHaveAccessToList(id, userId)))
            {
                return Forbid();
            }
            var list = await _listBl.GetItemsByListId(id);
            if (list == null)
                return NotFound();
            return Ok(list);
        }

        [HttpGet("{id}/items/done")]
        public async Task<ActionResult> GetDoneItems(int id)
        {
            (userId, projectId) = GetUserId();
            if (!(await _userBl.IsHaveAccessToList(id, userId)))
            {
                return Forbid();
            }
            var list = await _listBl.GetDoneItemsByListId(id);
            if (list == null)
                return NotFound();
            return Ok(list);
        }

        [HttpGet("{id}/items/undone")]
        public async Task<ActionResult> GetUnDoneItems(int id)
        {
            (userId, projectId) = GetUserId();
            if (!(await _userBl.IsHaveAccessToList(id, userId)))
            {
                return Forbid();
            }
            var list = await _listBl.GetUndoneItemsByListId(id);
            if (list == null)
                return NotFound();
            return Ok(list);
        }

        // POST: api/Item
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ListDto list)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            (userId, projectId) = GetUserId();
            if (!(await _userBl.IsHaveProjectPermission(userId, projectId)))
            {
                return Forbid();
            }
            var result = await _listBl.CreateAsync(list);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.id);
        }

        // PUT: api/Item/5
        [HttpPut]
        public async Task<ActionResult> Put(ListDto list)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            (userId, projectId) = GetUserId();
            if (!(await _userBl.IsHaveProjectPermission(userId, projectId)))
            {
                return Forbid();
            }
            var result = await _listBl.UpdateAsync(list);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            (userId, projectId) = GetUserId();
            if (!(await _userBl.IsHaveAccessToList(id, userId)))
            {
                return Forbid();
            }
            var result = await _listBl.DeleteAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }
    }
}