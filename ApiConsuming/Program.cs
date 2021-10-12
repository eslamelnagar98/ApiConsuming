using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ApiConsuming
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("http://localhost:15672/api/queues")
            };

            using (var client = new HttpClient())
            {
                //Set Basic Auth
                var user = "Islam";
                var password = "12345";
                var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{user}:{password}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64String);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var rabbitMqjsonObjects = JsonConvert.DeserializeObject<List<RabbitMqJson>>(content);
                    foreach (var rabbitMQJson in rabbitMqjsonObjects)
                    {
                        Console.WriteLine(rabbitMQJson.Name);
                    }
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
        }
    }
    public class RabbitMqJson
    {
        public string Name { get; set; }
    }
}
