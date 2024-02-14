using FlagsApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FlagsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphicElementApiService : ControllerBase
    {
        private readonly HttpClient httpClient;
        private const string baseApiUrl = "https://localhost:7156/api/graphicElementsApi/";

        public GraphicElementApiService()
        {
            httpClient = new HttpClient();
        }


        public async Task<List<GraphicElement>> GetAllGraphicElements()
        {
            var response = await httpClient.GetAsync(baseApiUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode}");
            }

            return JsonConvert.DeserializeObject<List<GraphicElement>>(await response.Content.ReadAsStringAsync());
        }



        public async Task<GraphicElement> GetGraphicElement(int id)
        {
            var response = await httpClient.GetAsync(baseApiUrl + id.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode}");
            }

            return JsonConvert.DeserializeObject<GraphicElement>(await response.Content.ReadAsStringAsync());
        }
    }
}
