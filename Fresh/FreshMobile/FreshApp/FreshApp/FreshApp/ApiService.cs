using System;
using System.Collections.Generic;
using System.Net.Http;
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
    }
}
