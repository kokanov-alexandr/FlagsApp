using FlagsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlagsApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphicElementsApiController : ControllerBase
    {
        private readonly FlagsDBContext dbContext;

        public GraphicElementsApiController(FlagsDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await dbContext.GraphicElements.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var graphicElement = await dbContext.GraphicElements.FindAsync(id);

            if (graphicElement == null)
            {
                return NotFound();
            }
            return Ok(graphicElement);
        }

        [HttpPost]
        public async Task<IActionResult> Create(GraphicElement graphicElement)
        {
            await dbContext.GraphicElements.AddAsync(graphicElement);
            await dbContext.SaveChangesAsync();
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var graphicElement = await dbContext.GraphicElements.FindAsync(id);

            if (graphicElement == null)
            {
                return BadRequest();
            }

            dbContext.Remove(graphicElement);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            dbContext.RemoveRange(dbContext.GraphicElements);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
