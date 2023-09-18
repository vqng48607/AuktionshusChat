using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class AuktionsItem
    {
        public string Name { get; set; }           // Navn på varen
        public decimal MinimumPrice { get; set; }  // Minimumspris for varen
        public decimal SalePrice { get; set; }     // Salgspris for varen (0.00 indtil solgt)
        public int MaxAuctionDuration { get; set; } // Maksimal varighed af auktionen i sekunder
        public DateTime StartTime { get; set; }    // Starttidspunkt for auktionen

        // Konstruktør til at oprette en auktionsvare
        public AuktionsItem(string name, decimal minimumPrice, int maxAuctionDuration)
        {
            Name = name;
            MinimumPrice = minimumPrice;
            SalePrice = 0.00m; // Initialiseres til 0.00 indtil solgt
            MaxAuctionDuration = maxAuctionDuration;
            StartTime = DateTime.Now;
        }
    }
}
