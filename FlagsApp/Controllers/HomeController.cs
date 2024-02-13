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

        private async Task ReloadFilterData()
        {
            ViewBag.—olors = await colorApiService.GetAllColors();
            ViewBag.Lines = await linesApiService.GetAllLines();
            ViewBag.Selected—olorsId = new List<int>();
            ViewBag.SelectedLinesId = new List<int>();
        }

        [HttpGet]   
        public async Task<IActionResult> Index()
        {
            ViewBag.Flags = await flagsApiService.GetAllFlags();
            await ReloadFilterData();
            return View();
        }


        private async Task<List<Flag>> FilterFlags(List<int> selectedColorsId, List<int> selectedLinesId)
        {
            var allFlags = await flagsApiService.GetAllFlags();
            var flagsByColors = selectedColorsId.Count != 0 ? await flagColorApiService.GetAllFlagsByColorsId(selectedColorsId) : allFlags;
            var flagsByLines = selectedLinesId.Count != 0 ? await flagLinesApiService.GetAllFlagsByLinesId(selectedLinesId) : allFlags;

            return flagsByColors.Intersect(flagsByLines).ToList();
        }

        [HttpPost]
        public async Task<IActionResult> FilterForCountry(string countryName)
        {
            var allFlags = await flagsApiService.GetAllFlags();
            await ReloadFilterData();

            if (string.IsNullOrEmpty(countryName))
            {
                ViewBag.Flags = allFlags;
                return View("Index");
            }

            var filteredFlags = allFlags.Where(f => f.CountryName.Contains(countryName, StringComparison.CurrentCultureIgnoreCase)).ToList();
            ViewBag.Flags = filteredFlags;
         
            return View("Index");   
        }

        [HttpPost]
        public async Task<IActionResult> Index(List<int> selectedColorsId, List<int> selectedLinesId)
        {
            ViewBag.Flags = await FilterFlags(selectedColorsId, selectedLinesId);

            ViewBag.—olors = await colorApiService.GetAllColors();
            ViewBag.Lines = await linesApiService.GetAllLines();
            
            ViewBag.Selected—olorsId = selectedColorsId;
            ViewBag.SelectedLinesId = selectedLinesId;

            return View("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Flag(int id)
        {
            var selected—olors = await flagColorApiService.GetColorsByFlagId(id);
            var selectedColorsId = selected—olors.Select(Ò => Ò.Id).ToList();

            var selectedLines = await flagLinesApiService.GetLinesByFlagId(id);
            var selectedLinesId = selectedLines.Select(Ò => Ò.Id).ToList();

            var flag = await flagsApiService.GetFlag(id);

            ViewBag.Colors = await colorApiService.GetAllColors();
            ViewBag.LInes = await linesApiService.GetAllLines();
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
