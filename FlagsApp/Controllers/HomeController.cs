using FlagsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FlagsApp.Controllers
{
    public class HomeController : Controller
    {


        private readonly ILogger<HomeController> _logger;
        private FlagsApiService flagsApiService;

        public HomeController(ILogger<HomeController> logger, FlagsDBContext dbContext)
        {
            _logger = logger;
            flagsApiService = new FlagsApiService();
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var flags = await flagsApiService.GetAllFlags();
            return View(flags);      
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
