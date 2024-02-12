using FlagsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FlagsApp.Controllers
{
    public class ColorApiService : Controller
    {
        private readonly HttpClient httpClient;
        private const string baseApiUrl = "https://localhost:7156/api/colorsApi/";

        public ColorApiService()
        {
            httpClient = new HttpClient();
        }


        public async Task<List<Color>> GetAllColors()
        {
            var response = await httpClient.GetAsync(baseApiUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode}");
            }

            return JsonConvert.DeserializeObject<List<Color>>(await response.Content.ReadAsStringAsync()); ;
        }



        public async Task<Color> GetColor(int id)
        {
            var response = await httpClient.GetAsync(baseApiUrl + id.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode}");
            }

            return JsonConvert.DeserializeObject<Color>(await response.Content.ReadAsStringAsync());
        }
    }
}
