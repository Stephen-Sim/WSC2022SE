using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshApp.Models
{
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
