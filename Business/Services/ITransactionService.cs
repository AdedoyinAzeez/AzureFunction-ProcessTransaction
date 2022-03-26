using Pyrros.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyrros.Interface
{
    public interface ITransactionService
    {
        Task<ServiceResponse> InsertTransaction(Transaction transaction);
    }
}
