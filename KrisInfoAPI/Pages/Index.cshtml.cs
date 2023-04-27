using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace KrisInfoAPI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public List<KrisInfoVM> KrisLista { get; set; }


        public async Task GetJsonDataAll()
        {
            // Max siffra = 73! Vet inte varför!
            var days = 30;
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.krisinformation.se");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync($"/v3/news?days={days}");
            if (response.IsSuccessStatusCode)
            {
                // Gör om responsen till en sträng
                var responseBody = await response.Content.ReadAsStringAsync();
                try
                {
                    // Gör om strängen till vår egen skapade datatyp - KrisInfoResponse
                    var messages = JsonConvert.DeserializeObject<List<KrisInfoVM>>(responseBody);

                    KrisLista = messages;

                }
                catch (JsonReaderException)
                {
                }
            }
        }

        public void OnGet()
        {
            GetJsonDataAll();
        }
    }
}