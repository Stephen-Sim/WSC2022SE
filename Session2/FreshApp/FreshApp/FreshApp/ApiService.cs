using FreshApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FreshApp
{
    public class ApiService
    {
        public string Url { get; set; } = "http://192.168.0.151:45455/api/get/";
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
                var result = await res.Content.ReadAsStringAsync();
                Application.Current.Properties["user_id"] = JsonConvert.DeserializeObject<long>(result);
                return true;
            }

            return false;
        }

        public async Task<List<Item>> GetListings(long userId)
        {
            var url = this.Url + $"GetListings?userId={userId}";
            var res = await client.GetStringAsync(url);
            return JsonConvert.DeserializeObject<List<Item>>(res);
        }

        public async Task<List<ItemPrice>> GetItemPrices(long itemId)
        {
            var url = this.Url + $"GetItemPrices?itemId={itemId}";
            var res = await client.GetStringAsync(url);
            return JsonConvert.DeserializeObject<List<ItemPrice>>(res);
        }

        public async Task<bool> DeleteItemPrice(long itemPriceId)
        {
            var url = this.Url + $"DeleteItemPrice?itemPriceId={itemPriceId}";
            var res = await client.GetAsync(url);

            if (res.IsSuccessStatusCode)
            {
                var result = await res.Content.ReadAsStringAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> EditItemPrice(object ip)
        {
            var url = this.Url + $"EditItemPrice";

            var json = JsonConvert.SerializeObject(ip);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var res = await client.PostAsync(url, data);

            if (res.IsSuccessStatusCode)
            {
                var result = await res.Content.ReadAsStringAsync();
                return true;
            }

            return false;
        }
    }
}
