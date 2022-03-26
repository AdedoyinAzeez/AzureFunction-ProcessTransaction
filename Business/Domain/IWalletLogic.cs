using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyrros.Domain
{
    public interface IWalletLogic
    {
        DataSet GetWalletDetails(long account);
        string UpdateBalance(long account, decimal amount);
    }
}
