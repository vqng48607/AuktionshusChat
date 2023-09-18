using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class Bid
    {
        public decimal Amount { get; set; }  // Beløb på budet
        public DateTime Timestamp { get; set; } // Tidsstempel for budet

        // Konstruktør til at oprette et bud
        public Bid(decimal amount)
        {
            Amount = amount;
            Timestamp = DateTime.Now;
        }
    }
}
