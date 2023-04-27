using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrisInfoAPI.Pages
{
    public class CardModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
    }

    public class MessageCardModel : PageModel
    {
        public CardModel Card { get; set; }

        public async void OnGet(int id)
        {
            Card = await GetJsonDataOne(id);
        }

        public static async Task<CardModel> GetJsonDataOne(int id)
        {
            CardModel card = null;

            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.krisinformation.se");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync($"/v3/news/{id}");
            if (!response.IsSuccessStatusCode)
            {

            }

            else if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                try
                {
                    var message = JsonConvert.DeserializeObject<CardModel>(responseBody);
                    if (message != null)
                    {
                        card = new CardModel
                        {
                            Id = message.Id,
                            Title = message.Title,
                            Description = message.Description,
                            Country = message.Country
                        };

                        // Use the card object as needed
                        Console.WriteLine($"Id: {card.Id}, Title: {card.Title}, Description: {card.Description}, Country: {card.Country}");
                    }
                    Console.WriteLine("Tryck valfri knapp för att fortsätta");
                    Console.ReadLine();
                }
                catch (JsonReaderException)
                {
                    Console.WriteLine("Prutt! Det funkade inte.");
                }
            }

            return card;
        }
    }
}
