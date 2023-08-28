using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshApp.Models
{
    public class User
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public int FamilyCount { get; set; }
        public long CartID { get; set; }
        public int CartCount { get; set; }
    }
}
