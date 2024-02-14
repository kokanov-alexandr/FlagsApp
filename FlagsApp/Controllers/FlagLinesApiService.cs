using FlagsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FlagsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlagLinesApiService : ControllerBase
    {
        private readonly HttpClient httpClient;
        private const string baseApiUrl = "https://localhost:7156/api/flagLinesApi/";

        public FlagLinesApiService()
        {
            httpClient = new HttpClient();
        }

        public async Task<List<Lines>> GetLinesByFlagId(int flagId)
        {
            var response = await httpClient.GetAsync(baseApiUrl + "LinesByFlagId/" + flagId.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode}");
            }

            var lines = JsonConvert.DeserializeObject<List<Lines>>(await response.Content.ReadAsStringAsync());
            return lines;
        }


        public async Task<List<Flag>> GetAllFlagsByLinesId(List<int> selectedLinesId)
        {
            var allFlags = new List<Flag>();
            foreach (var i in selectedLinesId)
            {
                var response = await httpClient.GetAsync(baseApiUrl + "FlagsByLinesId/" + i.ToString());
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error: {response.StatusCode}");
                }

                var flags = JsonConvert.DeserializeObject<List<Flag>>(await response.Content.ReadAsStringAsync()) ?? [];
                allFlags.AddRange(flags);
            }
            return allFlags.Distinct().ToList();
        }

        public async Task DeleteLinesByFlagId(int flagId)
        {
            var response = await httpClient.DeleteAsync(baseApiUrl + "LinesByFlagId/" + flagId);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode}");
            }
        }

        public async Task Create(FlagLines flagLines)
        {
            var response = await httpClient.PostAsJsonAsync(baseApiUrl, flagLines);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode}");
            }
        }
    }
}
