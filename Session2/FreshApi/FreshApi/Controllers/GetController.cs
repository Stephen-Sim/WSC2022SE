using FreshApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
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

        [HttpGet]
        public object GetItemPrices(long ItemId)
        {
            var itemPrices = ent.ItemPrices.ToList().Where(x => x.ItemID == ItemId).Select(x => new { 
                x.ID,
                Date = x.Date.ToString("yyyy/MM/dd"),
                CancellationPolicy = x.CancellationPolicy.Name,
                x.Price,
                Status = new Func<string>(() =>
                {
                    if (x.BookingDetails.Any())
                    {
                        return "booked";
                    }

                    if (ent.DimDates.Any(y => y.Date == x.Date && y.isHoliday == true))
                    {
                        return "holiday";
                    }

                    return "";
                })()
            }).ToList();

            return Ok(itemPrices);
        }

        [HttpGet]
        public object DeleteItemPrice(long Id)
        {
            ent.ItemPrices.Remove(ent.ItemPrices.FirstOrDefault(x => x.ID == Id));
            ent.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public object EditItemPrice(dynamic ItemPrice)
        {
            var ip = ent.ItemPrices.ToList().FirstOrDefault(x => x.ID == ItemPrice.ID);
            ip.Price = ItemPrice.Price;
            ip.CancellationPolicyID = ItemPrice.CancellationPolicyID;
            ent.SaveChanges();

            return Ok();
        }
    }
}
