using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoCRUD.Models
{
    public class AdjustPrice
    {
        public decimal NewPrice { get; set; }
        public string Reason { get; set; }
    }
}
