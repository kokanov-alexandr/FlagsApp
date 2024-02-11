using FlagsApp.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace FlagsApp.Controllers
{
    public class FlagsApiService
    {
        private readonly HttpClient httpClient;
        private const string baseApiUrl = "https://localhost:7156/api/flagsApi/";

        public FlagsApiService()
        {
            httpClient = new HttpClient();
        }

        public void SaveImage(Flag flag)
        {
            WebClient webClient = new();
            webClient.DownloadFile(flag.ImageSrc, "Images/" + flag.ImageSrc.Split("/").Last());
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

            foreach (var flag in flags)
            {
                if (!IsImageSave(flag))
                {
                    await UpdateImage(flag);
                    SaveImage(flag);
                    Console.WriteLine(flag.Id.ToString()  + " processed");
                }
            }
            return flags;
        }
    }
}