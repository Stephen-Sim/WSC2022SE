using FreshApi.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
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
                Date = x.ItemPrices.Any() ? x.ItemPrices.Max(y => y.Date).Date.ToString("yyyy/MM/dd") : "There is no availablity.",
                Color = x.ItemPrices.Any(y => y.Date >= DateTime.Now && y.Date <= DateTime.Now.AddDays(5)) ? Color.Black : Color.Red
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
                x.CancellationPolicyID,
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
            }).OrderBy(x => x.Date).ToList();

            return Ok(itemPrices);
        }

        [HttpGet]
        public object GetCancellationPolicies()
        {
            var policies = ent.CancellationPolicies.ToList().Select(x => new
            {
                x.ID,
                x.Name,
            });

            return Ok(policies);
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
            var ip = ent.ItemPrices.ToList().FirstOrDefault(x => x.ID == (long)ItemPrice.ID);
            ip.Price = (decimal)ItemPrice.Price;
            ip.CancellationPolicyID = (long)ItemPrice.CancellationPolicyID;
            ent.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public object AddItemPriceListing(AddItemPriceListingRequest addItemPrice)
        {
            try
            {
                var temp = ent.ItemPrices.ToList().Where(x => x.ItemID == addItemPrice.ItemId && x.Date.Date >= addItemPrice.StartDate.Date && x.Date.Date <= addItemPrice.EndDate.Date && x.BookingDetails.Count() == 0).ToList();
                ent.ItemPrices.RemoveRange(temp);

                for (var i = addItemPrice.StartDate; i <= addItemPrice.EndDate; i = i.AddDays(1))
                {
                    if (ent.ItemPrices.ToList().Any(x => x.Date.Date == i.Date && x.BookingDetails.Any()))
                    {
                        continue;
                    }

                    var date = ent.DimDates.ToList().FirstOrDefault(x => x.Date.Date == i.Date);

                    ItemPrice itemPrice = new ItemPrice { 
                        ItemID = addItemPrice.ItemId,
                        GUID = Guid.NewGuid(),
                        Date = i.Date
                    };

                    if (date.isHoliday)
                    {
                        itemPrice.Price = addItemPrice.HilodayPrice;
                        itemPrice.CancellationPolicyID = addItemPrice.HilodayPolicyId;
                    }
                    else if (date.DayOfWeek == 1 || date.DayOfWeek == 7)
                    {
                        itemPrice.Price = addItemPrice.WeekendPrice;
                        itemPrice.CancellationPolicyID = addItemPrice.WeekendPolicyId;
                    }
                    else
                    {
                        itemPrice.Price = addItemPrice.OtherdayPrice;
                        itemPrice.CancellationPolicyID = addItemPrice.OtherdayPolicyId;
                    }

                    ent.ItemPrices.Add(itemPrice);
                }

                ent.SaveChanges();

                return Ok();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return BadRequest();
            }
        }

        public class AddItemPriceListingRequest
        {
            public long ItemId { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public decimal WeekendPrice { get; set; }
            public long WeekendPolicyId { get; set; }
            public decimal HilodayPrice { get; set; }
            public long HilodayPolicyId { get; set; }
            public decimal OtherdayPrice { get; set; }
            public long OtherdayPolicyId { get; set; }
        }
    }
}
