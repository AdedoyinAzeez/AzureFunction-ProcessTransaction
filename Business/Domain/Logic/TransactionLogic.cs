using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pyrros.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyrros.Domain.Logic
{
    public class TransactionLogic : ITransactionLogic
    {
        private readonly ILogger<TransactionLogic> _logger;
        string error = "";
        private string connectionString;
        public TransactionLogic(ILogger<TransactionLogic> logger, IOptions<PyrrosOptions> options)
        {
            _logger = logger;
            this.connectionString = options.Value.ConnectionString;
        }

        public string TransactionInsert(Transaction transaction)
        {
            string ErrorMsg = "";
            int result = 0;
            Connect con = new Connect(connectionString);

            try
            {
                con.SetProcedure("Transaction_Insert");
                con.AddParam("@Amount", transaction.Amount);
                con.AddParam("@Direction", transaction.Direction);
                con.AddParam("@Account", transaction.Account);

                result = con.Execute();
                ErrorMsg = con.errmsg;
                _logger.LogInformation(ErrorMsg);
                return ErrorMsg;

            }
            catch (Exception ex)
            {
                error = ex.Message + ":" + ex.StackTrace;
                _logger.LogError(error);
                return ErrorMsg;
            }
            finally
            {
                con.Connection.Close();
            }
        }
    }
}
