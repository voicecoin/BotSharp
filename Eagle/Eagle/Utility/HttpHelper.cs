using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Utility
{
    public class HttpHelper
    {
        public static async Task<T> Rest<T>(string requestUri, object body)
        {
            T result = default(T);
            if(body == null)
            {
                body = new object();
            }

            String json = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUri);
                
                HttpResponseMessage response = await client.PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json"));
                string content = await response.Content.ReadAsStringAsync();
                
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = JsonConvert.DeserializeObject<T>(content);
                }
                else
                {
                    throw new Exception(content);
                }
            }

            return result;
        }
    }
}