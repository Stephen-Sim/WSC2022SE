using FreshApp.Models;
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
                var data = await res.Content.ReadAsStringAsync();
                int user_id = JsonConvert.DeserializeObject<int>(data);
                Preferences.Set("isLoggedIn", true);
                Preferences.Set("user_id", user_id);
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

        public async Task<List<Item>> GetItems(long userId)
        {
            var url = this.Url + $"GetItems?userId={userId}";
            var res = await client.GetStringAsync(url);
            var result = JsonConvert.DeserializeObject<List<Item>>(res);
            return result;
        }

        public async Task<List<ItemPrice>> GetItemPrices(long ItemId)
        {
            var url = this.Url + $"GetItemPrices?ItemId={ItemId}";
            var res = await client.GetStringAsync(url);
            var result = JsonConvert.DeserializeObject<List<ItemPrice>>(res);
            return result;
        }

        public async Task<bool> DeleteItemPrice(long Id)
        {
            var url = this.Url + $"DeleteItemPrice?Id={Id}";
            var res = await client.GetAsync(url);

            if (res.IsSuccessStatusCode)
            {
                var data = await res.Content.ReadAsStringAsync();
                return true;
            }

            return false;
        }

        public async Task<List<CancellationPolicy>> GetCancellationPolicies()
        {
            var url = this.Url + $"GetCancellationPolicies";
            var res = await client.GetStringAsync(url);
            var result = JsonConvert.DeserializeObject<List<CancellationPolicy>>(res);
            return result;
        }

        public async Task<bool> EditItemPrice(object obj)
        {
            var url = this.Url + $"EditItemPrice";

            var json = JsonConvert.SerializeObject(obj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var res = await client.PostAsync(url, content);

            if (res.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> AddItemPriceListing(AddItemPriceListingRequest itemPrices)
        {
            try
            {
                var url = this.Url + $"AddItemPriceListing";

                var json = JsonConvert.SerializeObject(itemPrices);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var res = await client.PostAsync(url, content);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
