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
        public int Identifier { get; set; }
        public string Headline { get; set; }
        public string BodyText { get; set; }
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
                            Identifier = message.Identifier,
                            Headline = message.Headline,
                            BodyText = message.BodyText,
                        };

                        // Use the card object as needed
                        Console.WriteLine($"Id: {card.Identifier}, Title: {card.Headline}, Description: {card.BodyText}");
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
