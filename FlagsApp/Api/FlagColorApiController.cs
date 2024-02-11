using FlagsApp.Models;
using GenHTTP.Modules.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlagsApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlagColorApiController : ControllerBase
    {
        private readonly FlagsDBContext dbContext;
        public FlagColorApiController(FlagsDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await dbContext.FlagColors.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(FlagColor flagColor)
        {
            await dbContext.FlagColors.AddAsync(flagColor);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            dbContext.RemoveRange(dbContext.FlagColors);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{flagId}/{colorId}")]
        public async Task<IActionResult> Delete(int flagId, int colorId)
        {
            var flagColor = await dbContext.FlagColors.FindAsync(flagId, colorId);

            if (flagColor == null)
            {
                return NotFound();
            }

            dbContext.FlagColors.Remove(flagColor);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("ColorsByFlagId/{flagId}")]
        public async Task<IActionResult> GetColorsByFlagId(int flagId)
        {
            var colorsForFlag = await dbContext.FlagColors
                .Where(fc => fc.FlagId == flagId)
                .Select(fc => fc.ColorId)
                .ToListAsync();

            return Ok(colorsForFlag);
        }

        [HttpGet("FlagsByColorId/{colorId}")]
        public async Task<IActionResult> GetFlagsByColorId(int colorId)
        {
            var colorsForFlag = await dbContext.FlagColors
                .Where(fc => fc.ColorId == colorId)
                .Select(fc => fc.FlagId)
                .ToListAsync();

            return Ok(colorsForFlag);
        }
    }
}
