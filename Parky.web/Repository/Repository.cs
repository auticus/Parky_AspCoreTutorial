using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Parky.web.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected readonly IHttpClientFactory ClientFactory;

        public Repository(IHttpClientFactory clientFactory)
        {
            ClientFactory = clientFactory;
        }

        public async Task<T> GetAsync(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + id); //failure to append id results in an array coming back
            var client = ClientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(json);
            }

            //if not ok, can log from the client or do some other action that can alert
            //users that something was wrong
            return null;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = ClientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(json);
            }

            //if not ok, can log from the client or do some other action that can alert
            //users that something was wrong
            //in this case, just returning an empty list back.  Not a fan of returning nulls.
            return new List<T>();
        }

        public async Task<bool> CreateAsync(string url, T dto)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (dto == null) return false;
            request.Content = new StringContent(
                JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var client = ClientFactory.CreateClient();
            var response = await client.SendAsync(request);

            return response.StatusCode == HttpStatusCode.Created;
        }

        public async Task<bool> UpdateAsync(string url, T dto)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, url);
            if (dto == null) return false;
            request.Content = new StringContent(
                JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var client = ClientFactory.CreateClient();
            var response = await client.SendAsync(request);

            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<bool> DeleteAsync(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url + id);
            var client = ClientFactory.CreateClient();
            var response = await client.SendAsync(request);

            return response.StatusCode == HttpStatusCode.NoContent;
        }
    }
}
