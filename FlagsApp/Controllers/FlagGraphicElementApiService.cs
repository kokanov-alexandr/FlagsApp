using FlagsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FlagsApp.Controllers
{
    public class FlagGraphicElementApiService : Controller
    {
        private readonly HttpClient httpClient;
        private const string baseApiUrl = "https://localhost:7156/api/flagGraphicElementApi/";

        public FlagGraphicElementApiService()
        {
            httpClient = new HttpClient();
        }

        public async Task<List<GraphicElement>> GetGraphicElementsByFlagId(int flagId)
        {
            var response = await httpClient.GetAsync(baseApiUrl + "GraphicElementsByFlagId/" + flagId.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode}");
            }

            var graphicElements = JsonConvert.DeserializeObject<List<GraphicElement>>(await response.Content.ReadAsStringAsync());
            return graphicElements;
        }


        public async Task<List<Flag>> GetAllFlagsByGraphicElementsId(List<int> selectedGraphicElementsId)
        {
            var allFlags = new List<Flag>();
            foreach (var i in selectedGraphicElementsId)
            {
                var response = await httpClient.GetAsync(baseApiUrl + "FlagsByGraphicElementId/" + i.ToString());
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error: {response.StatusCode}");
                }

                var flags = JsonConvert.DeserializeObject<List<Flag>>(await response.Content.ReadAsStringAsync()) ?? [];
                allFlags.AddRange(flags);
            }
            return allFlags.Distinct().ToList();
        }

        public async Task DeleteGraphicElementsByFlagId(int flagId)
        {
            var response = await httpClient.DeleteAsync(baseApiUrl + "GraphicElementsByFlagId/" + flagId);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode}");
            }
        }

        public async Task Create(FlagGraphicElement flagGraphicElement)
        {
            var response = await httpClient.PostAsJsonAsync(baseApiUrl, flagGraphicElement);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode}");
            }
        }
    }
}
