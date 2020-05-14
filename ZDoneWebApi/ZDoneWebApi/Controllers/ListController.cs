using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZDoneWebApi.BusinessLogic.Interfaces;
using ZDoneWebApi.Data.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZDoneWebApi.Controllers
{
    [EnableCors]
    [Route("api/lists")]
    [ApiController]
    public class ListController : ControllerBase
    {
        private readonly IListBl _listBl;

        public ListController(IListBl listBl)
        {
            _listBl = listBl;
        }

        // GET: api/Item
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var allItems = await _listBl.GetAllAsync();
            if (allItems == null)
                return NotFound();
            return Ok(allItems);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var list = await _listBl.ReadAsync(id);
            if (list == null)
                return NotFound();
            return Ok(list);
        }

        [HttpGet("{id}/items")]
        public async Task<ActionResult> GetItems(int id)
        {
            var list = await _listBl.GetItemsByListId(id);
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
            var result = await _listBl.UpdateAsync(list);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _listBl.DeleteAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }
    }
}