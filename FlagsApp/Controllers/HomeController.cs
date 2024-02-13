using FlagsApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FlagsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FlagsApiService flagsApiService;
        private readonly FlagColorApiService flagColorApiService;
        private readonly ColorApiService colorApiService;
        private readonly LinesApiService linesApiService;
        private readonly FlagLinesApiService flagLinesApiService;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            flagsApiService = new FlagsApiService();
            flagColorApiService = new FlagColorApiService();
            colorApiService = new ColorApiService();
            linesApiService = new LinesApiService();
            flagLinesApiService = new FlagLinesApiService();
        }

        [HttpGet]   
        public async Task<IActionResult> Index()
        {
            var flags = await flagsApiService.GetAllFlags();
            var colors = await colorApiService.GetAllColors();
            var lines = await linesApiService.GetAllLines();

            ViewBag.Flags = flags;
            ViewBag.—olors = colors;
            ViewBag.LInes = lines;
            ViewBag.Selected—olorsId = new List<int>();
            ViewBag.SelectedLinesId = new List<int>();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(List<int> selectedColorsId, List<int> selectedLinesId)
        {
            var flags = await flagColorApiService.GetAllFlagsByColorsId(selectedColorsId);
            ViewBag.Flags = flags;
            ViewBag.—olors = await colorApiService.GetAllColors();
            ViewBag.LInes = await linesApiService.GetAllLines();
            ViewBag.Selected—olorsId = selectedColorsId;
            ViewBag.SelectedLinesId = selectedLinesId;

            return View("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Flag(int id)
        {
            var colors = await colorApiService.GetAllColors();
            var lines = await linesApiService.GetAllLines();

            var selected—olors = await flagColorApiService.GetColorsByFlagId(id);
            var selectedColorsId = selected—olors.Select(l => l.Id).ToList();

            var selectedLines = await flagLinesApiService.GetLinesByFlagId(id);
            var selectedLinesId = selectedLines.Select(l => l.Id).ToList();

            var flag = await flagsApiService.GetFlag(id);

            ViewBag.Colors = colors;
            ViewBag.LInes = lines;
            ViewBag.Selected—olorsId = selectedColorsId;
            ViewBag.SelectedLinesId = selectedLinesId;
            
            ViewBag.Flag = flag;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Flag(int flagId, List<int> selectedColorsId, List<int> selectedLinesId)
        {
            await flagColorApiService.DeleteColorsByFlagId(flagId);
            foreach (var i in selectedColorsId)
            {
                await flagColorApiService.Create(new FlagColor { FlagId = flagId, ColorId = i });
            }

            await flagLinesApiService.DeleteLinesByFlagId(flagId);
            foreach (var i in selectedLinesId)
            {
                await flagLinesApiService.Create(new FlagLines { FlagId = flagId, LinesId = i });
            }

            return RedirectToAction("Flag", new {flagId});
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
