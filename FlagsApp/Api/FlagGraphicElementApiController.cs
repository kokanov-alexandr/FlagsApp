using FlagsApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlagsApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlagGraphicElementApiController : ControllerBase
    {
        private readonly FlagsDBContext dbContext;
        public FlagGraphicElementApiController(FlagsDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await dbContext.FlagGraphicElements.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(FlagGraphicElement flagGraphicElement)
        {
            await dbContext.FlagGraphicElements.AddAsync(flagGraphicElement);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            dbContext.RemoveRange(dbContext.FlagGraphicElements);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{flagId}/{graphicElementId}")]
        public async Task<IActionResult> Delete(int flagId, int graphicElementId)
        {
            var flagGraphicElement = await dbContext.FlagGraphicElements.FindAsync(flagId, graphicElementId);

            if (flagGraphicElement == null)
            {
                return NotFound();
            }

            dbContext.FlagGraphicElements.Remove(flagGraphicElement);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("GraphicElementsByFlagId/{flagId}")]
        public async Task<IActionResult> GetGraphicElementsByFlagId(int flagId)
        {
            var graphicElementsForFlag = await dbContext.FlagGraphicElements
                .Where(fge => fge.FlagId == flagId)
                .Join(
                    dbContext.GraphicElements,
                    fge => fge.GraphicElementId,
                    ge => ge.Id,
                    (fge, ge) => ge
                )
                .ToListAsync();

            return Ok(graphicElementsForFlag);
        }

        [HttpGet("FlagsByGraphicElementId/{graphicElementId}")]
        public async Task<IActionResult> GetFlagsByLinesId(int graphicElementId)
        {
            var flagsForGraphicElement = await dbContext.FlagGraphicElements
               .Where(fl => fl.GraphicElementId == graphicElementId)
               .Join(
                   dbContext.Flags,
                   fge => fge.FlagId,
                   f => f.Id,
                   (fge, f) => f
               )
               .ToListAsync();

            return Ok(flagsForGraphicElement);
        }

        [HttpDelete("GraphicElementsByFlagId/{flagId}")]
        public async Task<IActionResult> DeleteGraphicElementsByFlagId(int flagId)
        {
            var graphicElements = dbContext.FlagGraphicElements.Where(fge => fge.FlagId == flagId).ToList();

            dbContext.RemoveRange(graphicElements);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}
