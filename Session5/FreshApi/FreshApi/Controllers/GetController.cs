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
        public WSC2022SE_Session5Entities ent { get; set; }

        public GetController()
        {
            ent = new WSC2022SE_Session5Entities();
        }

        [HttpGet]
        public object Login(string username, string password)
        {
            var user = ent.Users.FirstOrDefault(x => x.Username == username && x.Password == password);

            if (user == null)
            {
                return BadRequest();
            }

            if (!user.AddonServices.Any(x => x.CouponID == null))
            {
                var addonService = new AddonService()
                {
                    GUID = Guid.NewGuid(),
                    UserID = user.ID,
                    CouponID = null
                };

                ent.AddonServices.Add(addonService);
                ent.SaveChanges();
            }

            return Ok(new { 
                ID = user.ID,
                Name = user.FullName,
                user.FamilyCount,
                CartID = user.AddonServices.FirstOrDefault(x => x.CouponID == null).ID,
                CartCount = user.AddonServices.FirstOrDefault(x => x.CouponID == null).AddonServiceDetails.Count
            });
        }

        [HttpGet]
        public object GetServiceTypes()
        {
            var serviceTypes = ent.ServiceTypes.ToList().Select(x => new { 
                x.ID,
                x.Name,
                x.Description,
                IconName = x.IconName.Substring(3).Replace("-", ""),
            });

            return serviceTypes;
        }
    }
}
