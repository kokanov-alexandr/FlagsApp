using FlagsApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FlagsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private FlagsApiService flagsApiService;
        private FlagColorApiService flagColorApiService;
        private ColorApiService colorApiService;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            flagsApiService = new FlagsApiService();
            flagColorApiService = new FlagColorApiService();
            colorApiService = new ColorApiService();
        }
        [HttpGet]   
        public async Task<IActionResult> Index()
        {
            var flags = await flagsApiService.GetAllFlags();
            var colors = await colorApiService.GetAllColors();

            ViewBag.Flags = flags;
            ViewBag.—olors = colors;
            ViewBag.Selected—olorsId = new List<int>();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(List<int> selectedColorsId)
        {
            var flags = await flagColorApiService.GetAllFlagsByColorsId(selectedColorsId);
            ViewBag.Flags = flags;
            ViewBag.—olors = await colorApiService.GetAllColors();
            ViewBag.Selected—olorsId = selectedColorsId;

            return View("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Flag(int id)
        {
            var colors = await colorApiService.GetAllColors();
            var selected—olors = await flagColorApiService.GetColorsByFlagId(id);
            var selectedColorsId = selected—olors.Select(c => c.Id).ToList();

            var flag = await flagsApiService.GetFlag(id);
            ViewBag.Colors = colors;
            ViewBag.Selected—olorsId = selectedColorsId;
            ViewBag.Flag = flag;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Flag(int flagId, List<int> selectedColorIds)
        {
            await flagColorApiService.DeleteColorsByFlagId(flagId);
            foreach (var i in selectedColorIds)
            {
                await flagColorApiService.Create(new FlagColor { FlagId = flagId, ColorId = i });
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
