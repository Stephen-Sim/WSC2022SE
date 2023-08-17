using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshApp.Models
{
    public class ItemPrice
    {
        public long ID { get; set; }
        public string Date { get; set; }
        public double Price { get; set; }
        public string CancellationPolicy { get; set; }
        public long CancellationPolicyID { get; set; }
        public string Status { get; set; }
    }
}
