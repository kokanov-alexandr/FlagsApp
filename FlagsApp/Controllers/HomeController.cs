using FlagsApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FlagsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly FlagsApiService flagsApiService;

        private readonly ColorApiService colorApiService;
        private readonly FlagColorApiService flagColorApiService;

        private readonly LinesApiService linesApiService;
        private readonly FlagLinesApiService flagLinesApiService;

        private readonly GraphicElementApiService graphicElementApiService;
        private readonly FlagGraphicElementApiService flagGraphicElementApiService;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            flagsApiService = new FlagsApiService();
            
            colorApiService = new ColorApiService();
            flagColorApiService = new FlagColorApiService();
            
            linesApiService = new LinesApiService();
            flagLinesApiService = new FlagLinesApiService();
            
            graphicElementApiService = new GraphicElementApiService();
            flagGraphicElementApiService = new FlagGraphicElementApiService();
        }

        private async Task ReloadFilterData()
        {
            ViewBag.—olors = await colorApiService.GetAllColors();
            ViewBag.Lines = await linesApiService.GetAllLines();
            ViewBag.GraphicElements = await graphicElementApiService.GetAllGraphicElements();

            ViewBag.Selected—olorsId = new List<int>();
            ViewBag.SelectedLinesId = new List<int>();
            ViewBag.SelectedGraphicElementsId = new List<int>();
        }

        [HttpGet]   
        public async Task<IActionResult> Index()
        {
            ViewBag.Flags = await flagsApiService.GetAllFlags();
            await ReloadFilterData();
            return View();
        }


        private async Task<List<Flag>> FilterFlags(List<int> selectedColorsId, List<int> selectedLinesId, List<int> selectedGraphicElementsId)
        {
            var allFlags = await flagsApiService.GetAllFlags();
            var flagsByColors = selectedColorsId.Count != 0 ? await flagColorApiService.GetAllFlagsByColorsId(selectedColorsId) : allFlags;
            var flagsByLines = selectedLinesId.Count != 0 ? await flagLinesApiService.GetAllFlagsByLinesId(selectedLinesId) : allFlags;
            var flagsByGraphicElements = selectedGraphicElementsId.Count != 0 ? await flagGraphicElementApiService.GetAllFlagsByGraphicElementsId(selectedGraphicElementsId) : allFlags;
            
            return flagsByColors.Intersect(flagsByLines).Intersect(flagsByGraphicElements).ToList();
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
        public async Task<IActionResult> Index(List<int> selectedColorsId, List<int> selectedLinesId, List<int> selectedGraphicElementsId)
        {
            ViewBag.Flags = await FilterFlags(selectedColorsId, selectedLinesId, selectedGraphicElementsId);

            ViewBag.—olors = await colorApiService.GetAllColors();
            ViewBag.Lines = await linesApiService.GetAllLines();
            ViewBag.GraphicElements = await graphicElementApiService.GetAllGraphicElements();

            ViewBag.Selected—olorsId = selectedColorsId;
            ViewBag.SelectedLinesId = selectedLinesId;
            ViewBag.SelectedGraphicElementsId = selectedGraphicElementsId;

            return View("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Flag(int id)
        {
            var selected—olors = await flagColorApiService.GetColorsByFlagId(id);
            var selectedColorsId = selected—olors.Select(Ò => Ò.Id).ToList();

            var selectedLines = await flagLinesApiService.GetLinesByFlagId(id);
            var selectedLinesId = selectedLines.Select(l => l.Id).ToList();

            var selectedGraphicElements = await flagGraphicElementApiService.GetGraphicElementsByFlagId(id);
            var selectedGraphicElementsId = selectedGraphicElements.Select(ge => ge.Id).ToList();

            var flag = await flagsApiService.GetFlag(id);

            ViewBag.Colors = await colorApiService.GetAllColors();
            ViewBag.Lines = await linesApiService.GetAllLines();
            ViewBag.GraphicElements = await graphicElementApiService.GetAllGraphicElements();

            ViewBag.Selected—olorsId = selectedColorsId;
            ViewBag.SelectedLinesId = selectedLinesId;
            ViewBag.SelectedGraphicElementsId = selectedGraphicElementsId;

            ViewBag.Flag = flag;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Flag(int flagId, List<int> selectedColorsId, List<int> selectedLinesId, List<int> selectedGraphicElementsId)
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

            await flagGraphicElementApiService.DeleteGraphicElementsByFlagId(flagId);
            foreach (var i in selectedGraphicElementsId)
            {
                await flagGraphicElementApiService.Create(new FlagGraphicElement { FlagId = flagId, GraphicElementId = i });
            }

            return RedirectToAction("Flag", new {flagId});
        }



        private FlagsTest GetFlagsTest()
        {
            var questionsCount = 3;
            var flags = flagsApiService.GetAllFlags().Result;

            var random = new Random();
            var shuffledFlags = flags.OrderBy(x => random.Next()).ToList();

            var selectedFlags = shuffledFlags.Take(questionsCount).ToList();

            return new FlagsTest
            {
                Flags = selectedFlags,
                QuestionsCount = questionsCount,
                CorrectAnswersCount = 0,
                QuestionNumber = 1,
                Answers = new List<string>()
            };
        }
            

        private bool IsFlagsTestStarted()
        {
            string isFlagsTestStartedString = HttpContext.Session.GetString("IsFlagsTestStarted");
            return !string.IsNullOrEmpty(isFlagsTestStartedString) && 
                bool.TryParse(isFlagsTestStartedString, out bool result) ? result : false;

        }

        [HttpGet]
        public IActionResult Test()
        {
           
            if (!IsFlagsTestStarted())
            {
                string flagsTestString = JsonConvert.SerializeObject(GetFlagsTest());
                HttpContext.Session.SetString("FlagsTest", flagsTestString);
                HttpContext.Session.SetString("IsFlagsTestStarted", true.ToString());
            }

            string s = HttpContext.Session.GetString("FlagsTest");
            FlagsTest flagsTest = JsonConvert.DeserializeObject<FlagsTest>(s);

            if (flagsTest.QuestionNumber > flagsTest.QuestionsCount)
            {
                HttpContext.Session.Remove("FlagsTest");
                HttpContext.Session.Remove("IsFlagsTestStarted");
            }
            return View(flagsTest);
        }

        [HttpPost]
        public IActionResult Test(string answer)
        {
            string s = HttpContext.Session.GetString("FlagsTest");
            FlagsTest flagsTest = JsonConvert.DeserializeObject<FlagsTest>(s);


            if (flagsTest.Flags[flagsTest.QuestionNumber - 1].CountryName == answer)
            {
                flagsTest.CorrectAnswersCount++;
            }

            flagsTest.QuestionNumber++;
            flagsTest.Answers.Add(answer);
            string flagsTestString = JsonConvert.SerializeObject(flagsTest);
            HttpContext.Session.SetString("FlagsTest", flagsTestString);

            return RedirectToAction("Test");
        }
    }
}
