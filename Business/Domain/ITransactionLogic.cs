using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyrros.Domain
{
    public interface ITransactionLogic
    {
        string TransactionInsert(Transaction transaction);
    }
}
