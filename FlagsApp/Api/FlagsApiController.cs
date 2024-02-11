using FlagsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace FlagsApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlagsApiController : ControllerBase
    {
        private readonly FlagsDBContext dbContext;

        public FlagsApiController(FlagsDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            return Ok(await dbContext.Flags.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {   
            var flag = await dbContext.Flags.FindAsync(id);

            if (flag == null)
            {
                return NotFound();
            }
            return Ok(flag);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Flag flag)
        {
            await dbContext.Flags.AddAsync(flag);
            await dbContext.SaveChangesAsync();
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var flag = await dbContext.Flags.FindAsync(id);

            if (flag == null)
            {
                return BadRequest();
            }

            dbContext.Remove(flag);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            dbContext.RemoveRange(dbContext.Flags);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] FlagDTO flagDTO)
        {
            var flag = await dbContext.Flags.FindAsync(id);

            if (flag == null)
            {
                return BadRequest();
            }

            flag.CountryName = flagDTO.CountryName ?? flag.CountryName;
            flag.ImageName = flagDTO.ImageName ?? flag.ImageName;
            flag.ImageSrc = flagDTO.ImageSrc ?? flag.ImageSrc;
            
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
