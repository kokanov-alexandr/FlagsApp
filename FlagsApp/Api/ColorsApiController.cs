using FlagsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlagsApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorsApiController : ControllerBase
    {
        private readonly FlagsDBContext dbContext;

        public ColorsApiController(FlagsDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await dbContext.Colors.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var color = await dbContext.Colors.FindAsync(id);

            if (color == null)
            {
                return NotFound();
            }
            return Ok(color);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Color color)
        {
            await dbContext.Colors.AddAsync(color);
            await dbContext.SaveChangesAsync();
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var color = await dbContext.Colors.FindAsync(id);

            if (color == null)
            {
                return BadRequest();
            }

            dbContext.Remove(color);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            dbContext.RemoveRange(dbContext.Colors);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Color newColor)
        {
            var color = await dbContext.Colors.FindAsync(id);

            if (color == null)
            {
                return BadRequest();
            }

            color.Name = newColor.Name ?? color.Name;
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
