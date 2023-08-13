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
    }
}
