using FlagsApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlagsApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinesApiController : ControllerBase
    {
        private readonly FlagsDBContext dbContext;

        public LinesApiController(FlagsDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await dbContext.Lines.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var lines = await dbContext.Lines.FindAsync(id);

            if (lines == null)
            {
                return NotFound();
            }
            return Ok(lines);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Lines lines)
        {
            await dbContext.Lines.AddAsync(lines);
            await dbContext.SaveChangesAsync();
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var lines = await dbContext.Lines.FindAsync(id);

            if (lines == null)
            {
                return BadRequest();
            }

            dbContext.Remove(lines);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            dbContext.RemoveRange(dbContext.Lines);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

     
    }
}
