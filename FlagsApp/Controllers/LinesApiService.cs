using FlagsApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FlagsApp.Controllers
{
    public class LinesApiService : Controller
    {
        private readonly HttpClient httpClient;
        private const string baseApiUrl = "https://localhost:7156/api/linesApi/";

        public LinesApiService()
        {
            httpClient = new HttpClient();
        }


        public async Task<List<Lines>> GetAllLines()
        {
            var response = await httpClient.GetAsync(baseApiUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode}");
            }

            return JsonConvert.DeserializeObject<List<Lines>>(await response.Content.ReadAsStringAsync());
        }



        public async Task<Lines> GetLines(int id)
        {
            var response = await httpClient.GetAsync(baseApiUrl + id.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode}");
            }

            return JsonConvert.DeserializeObject<Lines>(await response.Content.ReadAsStringAsync());
        }
    }
}
