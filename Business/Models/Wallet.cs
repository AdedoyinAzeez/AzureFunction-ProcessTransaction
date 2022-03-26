using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyrros
{
    public class Wallet
    {
        public long Id { get; set; }
        public string CurrencyCode { get; set; }
        public long CustomerID { get; set; }
        public decimal AvailableBalance { get; set; }
        
    }
}
