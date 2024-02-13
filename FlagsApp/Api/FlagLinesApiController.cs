using FlagsApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlagsApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlagLinesApiController : ControllerBase
    {
        private readonly FlagsDBContext dbContext;
        public FlagLinesApiController(FlagsDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await dbContext.FlagLines.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(FlagLines flagLines)
        {
            await dbContext.FlagLines.AddAsync(flagLines);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            dbContext.RemoveRange(dbContext.FlagLines);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{flagId}/{linesId}")]
        public async Task<IActionResult> Delete(int flagId, int linesId)
        {
            var flagLines = await dbContext.FlagLines.FindAsync(flagId, linesId);

            if (flagLines == null)
            {
                return NotFound();
            }

            dbContext.FlagLines.Remove(flagLines);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("LinesByFlagId/{flagId}")]
        public async Task<IActionResult> GetLinesByFlagId(int flagId)
        {
            var linesForFlag = await dbContext.FlagLines
                .Where(fl => fl.FlagId == flagId)
                .Join(
                    dbContext.Lines,
                    fl => fl.LinesId,
                    l => l.Id,
                    (fl, l) => l
                )
                .ToListAsync();

            return Ok(linesForFlag);
        }

        [HttpGet("FlagsByLinesId/{linesId}")]
        public async Task<IActionResult> GetFlagsByLinesId(int linesId)
        {
            var flagsForLines = await dbContext.FlagLines
               .Where(fl => fl.LinesId== linesId)
               .Join(
                   dbContext.Flags,
                   fl => fl.FlagId,
                   f => f.Id,
                   (fl, f) => f
               )
               .ToListAsync();

            return Ok(flagsForLines);
        }

        [HttpDelete("LinesByFlagId/{flagId}")]
        public async Task<IActionResult> DeleteLInesByFlagId(int flagId)
        {
            var lines = dbContext.FlagLines.Where(fl => fl.FlagId == flagId).ToList();

            dbContext.RemoveRange(lines);
            await dbContext.SaveChangesAsync();
            return Ok();
        }


    }
}
