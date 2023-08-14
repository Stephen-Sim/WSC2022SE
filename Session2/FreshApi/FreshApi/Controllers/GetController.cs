using FreshApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FreshApi.Controllers
{
    public class GetController : ApiController
    {
        public WSC2022SE_Session2Entities ent { get; set; }

        public GetController()
        {
            ent = new WSC2022SE_Session2Entities();
        }

        [HttpGet]
        public object Login(string username, string password)
        {
            var user = ent.Users.FirstOrDefault(x => x.Username == username && x.Password == password);

            if (user == null)
            {
                return BadRequest();
            }

            return user.ID;
        }

        [HttpGet]
        public object GetItems(long userId)
        {
            var items = ent.Items.ToList().Where(x => x.UserID == userId).OrderByDescending(x => x.ItemPrices.Where(y => y.BookingDetails != null).Count()).Select(x => new
            {
                x.ID,
                x.Title,
                Date = x.ItemPrices.Any() ? x.ItemPrices.OrderByDescending(y => y.Date).First().Date.ToString("yyyy/MM/dd") : System.DateTime.Today.AddDays(5).ToString("yyyy/MM/dd"),
            });

            return Ok(items);
        }
    }
}
