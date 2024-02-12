using FlagsApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FlagsApp.Controllers
{
    public class FlagColorApiService : Controller
    {
        private readonly HttpClient httpClient;
        private const string baseApiUrl = "https://localhost:7156/api/flagColorApi/";

        public FlagColorApiService()
        {
            httpClient = new HttpClient();
        }

        public async Task<List<Color>> GetColorsByFlagId(int flagId)
        {
            var response = await httpClient.GetAsync(baseApiUrl + "ColorsByFlagId/" + flagId.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode}");
            }

            var colors = JsonConvert.DeserializeObject<List<Color>>(await response.Content.ReadAsStringAsync());
            return colors;
        }

        public async Task DeleteColorsByFlagId(int flagId)
        {
            var response = await httpClient.DeleteAsync(baseApiUrl + "ColorsByFlagId/" + flagId);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode}");
            }
        }

        public async Task Create(FlagColor flagColor)
        {
            var response = await httpClient.PostAsJsonAsync(baseApiUrl, flagColor);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode}");
            }
        }
    }
}
