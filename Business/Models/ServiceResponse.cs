using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyrros.Models
{
    public class ServiceResponse
    {
        public Boolean Success { get; set; }
        public String Message { get; set; }
        public Int32 DataCount { get; set; }
        public Object Data { get; set; }
        public Tuple<string, decimal> Tuple { get; set; }
    }
}
