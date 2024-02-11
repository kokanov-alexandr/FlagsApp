using FlagsApp.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace FlagsApp.Controllers
{
    public class FlagsApiService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly HttpClient httpClient;
        private const string baseApiUrl = "https://localhost:7156/api/flagsApi/";

        public FlagsApiService()
        {
            httpClient = new HttpClient();
        }

        public void SaveImage(Flag flag)
        {
            WebClient webClient = new();
            var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var imagePath = Path.Combine(wwwrootPath, "Images", flag.ImageSrc.Split("/").Last());
            webClient.DownloadFile(flag.ImageSrc, imagePath);
        }

        public bool IsImageSave(Flag flag)
        {
            return flag.ImageName != null;
        }


        public async Task UpdateImage(Flag flag)
        {

            string jsonContent = JsonConvert.SerializeObject(new FlagDTO { ImageName = flag.ImageSrc.Split("/").Last() });
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var method = new HttpMethod("PATCH");

            var request = new HttpRequestMessage(method, baseApiUrl + flag.Id.ToString())
            {
                Content = content
            };

            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
     
        public async Task<List<Flag>> GetAllFlags()
        {
            var response = await httpClient.GetAsync(baseApiUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode}");
            }

            var flags = JsonConvert.DeserializeObject<List<Flag>>(await response.Content.ReadAsStringAsync());



            int flagsCount = flags.Count();
            for (int i = 0; i < flagsCount; i++)
            {
                if (!IsImageSave(flags[i]))
                {
                    await UpdateImage(flags[i]);
                    SaveImage(flags[i]);
                    await Console.Out.WriteLineAsync($"{i + 1} / {flagsCount} processed");;
                }
            }
            foreach (var flag in flags)
            {
               
            }
            return flags;
        }
    }
}