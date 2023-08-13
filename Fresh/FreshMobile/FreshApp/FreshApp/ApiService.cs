using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshApp
{
    public class ApiService
    {
        public string Url { get; set; } = "http://192.168.0.151:45457/api/get/";
        public HttpClient client { get; set; }

        public ApiService()
        {
            client = new HttpClient();
        }

        public async Task<bool> Login(string username, string password)
        {
            var url = this.Url + $"Login?username={username}&password={password}";
            var res = await client.GetAsync(url);

            if (res.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<object> GetObject(long Id)
        {
            var url = this.Url + $"GetObject?Id={Id}";
            var res = await client.GetAsync(url);

            if (res.IsSuccessStatusCode)
            {
                var data = await res.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<object>(data);
                return obj;
            }

            return null;
        }

        public async Task<bool> StoreObject(object obj)
        {
            var url = this.Url + $"StoreObject";

            var json = JsonConvert.SerializeObject(obj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var res = await client.PostAsync(url, content);

            if (res.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}
