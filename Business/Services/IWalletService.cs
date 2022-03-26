using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyrros.Interface
{
    public interface IWalletService
    {
        Task<Tuple<object, string>> UpdateWallet(long Id, long amount);
        Task<Tuple<object, string>> GetWalletDetails(long account);
    }
}
