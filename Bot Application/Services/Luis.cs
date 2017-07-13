using Bot_Application.Serialization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Application.Services
{
    public class Luis
    {
        public static async Task<Utterance> GetResponse(string message)
        {
            using (var client = new HttpClient())
            {
                const string authKey = "cb7515b8a93749b7bb11f550c899345b";

                 var url = $"https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/eed7d7f3-dd95-4cf3-beea-d3c9731bf25e?subscription-key={authKey}&timezoneOffset=0&verbose=true&q={message}";
				client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode) return null;
                var result = await response.Content.ReadAsStringAsync();

                var js = new DataContractJsonSerializer(typeof(Utterance));
                var ms = new MemoryStream(Encoding.ASCII.GetBytes(result));
                var list = (Utterance)js.ReadObject(ms);

                return list;
            }
        }
    }
}