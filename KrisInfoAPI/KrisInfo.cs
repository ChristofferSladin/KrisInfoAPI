using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace KrisInfoAPI
{
    public class KrisInfo
    {
        public static async Task GetJsonDataAll()
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
                    if (messages != null)
                    {
                        Console.WriteLine("Samtliga Varningar");
                        Console.WriteLine("******************");
                        foreach (var message in messages)
                        {
                            Console.WriteLine($"Id: {message.Identifier}");
                            Console.WriteLine($"Message: {message.PushMessage}");
                            Console.WriteLine($"Published: {message.Published}");
                            Console.WriteLine($"Country: {message.Area[0].Description}");
                            Console.WriteLine("========================================");
                        }
                    }

                    Console.WriteLine("Tryck valfri knapp för att fortsätta");
                    Console.ReadLine();
                }
                catch (JsonReaderException)
                {
                    Console.WriteLine("Prutt! Det funkade inte.");
                }
            }
        }
    }
}
