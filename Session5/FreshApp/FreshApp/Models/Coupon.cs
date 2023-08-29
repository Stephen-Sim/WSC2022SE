using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshApp.Models
{
    public class Coupon
    {
        public long ID { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal MaximimDiscountAmount { get; set; }
    }
}
