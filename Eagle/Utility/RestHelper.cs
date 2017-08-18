using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public static class RestHelper
    {
        public static async Task<T> Rest<T>(string requestUri, object body = null, string method = "POST")
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

                HttpResponseMessage response;
                if (method == "GET")
                {
                    response = await client.GetAsync(requestUri);
                }
                else
                {
                    response = await client.PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json"));
                }
                    
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

        public static T GetSync<T>(string requestUri)
        {
            T result = default(T);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUri);

                HttpResponseMessage response = Task.Run(() => client.GetAsync(requestUri)).Result;

                string content = Task.Run(() => response.Content.ReadAsStringAsync()).Result; 
                result = JsonConvert.DeserializeObject<T>(content);
            }

            return result;
        }

        public static string GetSync(string requestUri)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUri);

                HttpResponseMessage response = Task.Run(() => client.GetAsync(requestUri)).Result;

                return Task.Run(() => response.Content.ReadAsStringAsync()).Result;
            }
        }

        public static T PostSync<T>(string requestUri, object body = null)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUri);

                String json = JsonConvert.SerializeObject(body);
                HttpResponseMessage response = Task.Run(() => client.PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json"))).Result;

                String result = Task.Run(() => response.Content.ReadAsStringAsync()).Result;

                return JsonConvert.DeserializeObject<T>(result);
            }
        }

        /*public static Task<IRestResponse> Execute(this IRestClient restClient, RestRequest restRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            restClient.ExecuteAsync(restRequest, (restResponse, asyncHandle) =>
            {
                if (restResponse.ResponseStatus == ResponseStatus.Error)
                    tcs.SetException(restResponse.ErrorException);
                else
                    tcs.SetResult(restResponse);
            });
            return tcs.Task;
        }*/
    }
}