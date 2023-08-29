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
                var user = JsonConvert.DeserializeObject<User>(data);
                
                App.User = user;

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

        public async Task<List<ServiceType>> GetServiceTypes()
        {
            var url = this.Url + $"GetServiceTypes";
            var res = await client.GetStringAsync(url);
            var result = JsonConvert.DeserializeObject<List<ServiceType>>(res);
            return result;
        }

        public async Task<List<Service>> getServices(long id)
        {
            string url = this.Url + $"getServices?id={id}";

            var res = await client.GetStringAsync(url);
            var result = JsonConvert.DeserializeObject<List<Service>>(res);

            return result;
        }
        public async Task<int> getPostByDate(long serviceId, DateTime date)
        {
            string url = this.Url + $"getPostByDate?serviceId={serviceId}&date={date}";

            var res = await client.GetStringAsync(url);
            var result = JsonConvert.DeserializeObject<int>(res);

            return result;
        }

        public async Task<bool> storeAddonService(AddonServiceDetail addonServiceDetail)
        {
            string url = this.Url + $"storeAddonService";

            var json = JsonConvert.SerializeObject(addonServiceDetail);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var res = await client.PostAsync(url, data);

            if (res.IsSuccessStatusCode)
            {
                var result = await res.Content.ReadAsStringAsync();
                var count = JsonConvert.DeserializeObject<int>(result);

                App.User.CartCount = count;

                return true;
            }

            return false;
        }

        public async Task<List<AddonServiceDetail>> getAddonServiceDetails(long id)
        {
            string url = this.Url + $"getAddonServiceDetails?id={id}";

            var res = await client.GetStringAsync(url);
            var result = JsonConvert.DeserializeObject<List<AddonServiceDetail>>(res);

            return result;
        }

        public async Task<bool> delAddonServiceDetail(long id)
        {
            string url = this.Url + $"delAddonServiceDetail?id={id}&addonServiceId={App.User.AddOnServiceId}";

            var res = await client.GetAsync(url);

            if (res.IsSuccessStatusCode)
            {
                var result = await res.Content.ReadAsStringAsync();
                var count = JsonConvert.DeserializeObject<int>(result);

                App.User.CartCount = count;

                return true;
            }

            return false;
        }

        public async Task<Coupon> checkCoupon(string code)
        {
            string url = this.Url + $"checkCoupon?code={code}";

            var res = await client.GetAsync(url);

            if (res.IsSuccessStatusCode)
            {
                var result = await res.Content.ReadAsStringAsync();
                var coupon = JsonConvert.DeserializeObject<Coupon>(result);
                return coupon;
            }

            return null;
        }

        public async Task<bool> proceedPayment(long addonSeriveId, long couponId)
        {
            string url = this.Url + $"proceedPayment?addonSeriveId={addonSeriveId}&couponId={couponId}";

            var res = await client.GetAsync(url);

            if (res.IsSuccessStatusCode)
            {
                var result = await res.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(result);

                App.User = user;

                return true;
            }

            return false;
        }
    }
}
}
