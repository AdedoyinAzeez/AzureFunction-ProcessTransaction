using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Pyrros.Domain;
using Pyrros.Interface;
using Pyrros.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyrros.Repository
{
    public class TransactionRepository : ITransactionService
    {
        ServiceResponse res = new ServiceResponse();
        private readonly ITransactionLogic transactionLogic;
        private readonly IWalletLogic walletLogic;
        private readonly ILogger<TransactionRepository> _logger;
        string message = "";

        public TransactionRepository()
        {

        }
        public TransactionRepository(ITransactionLogic transactionLogic, IWalletLogic walletLogic, ILogger<TransactionRepository> logger)
        {
            this.transactionLogic = transactionLogic;
            this.walletLogic = walletLogic;
            this._logger = logger;
        }
        public async Task<ServiceResponse> InsertTransaction(Transaction transaction)
        {
            try
            {
                var sufficient = walletLogic.GetWalletDetails(transaction.Account);
                decimal AvailableBalance = sufficient.Tables[0].Rows[0].Field<decimal>("AvailableBalance");
                string debited = string.Empty;

                if(transaction.Direction == "Debit")
                {
                    if (AvailableBalance <= 0 || (AvailableBalance - transaction.Amount < 0))
                    {
                        message = "You do not have sufficient amount to perform this transaction. Please fund your wallet and try again";
                        _logger.LogInformation(message);
                        res.Success = false;
                        res.Data = null;
                        res.Message = message;
                        return res;
                    }

                    string description = $"#{transaction.Amount} would be deducted from acct {transaction.Account} with  a present balance of {AvailableBalance - transaction.Amount}";
                    debited = walletLogic.UpdateBalance(transaction.Account, AvailableBalance-transaction.Amount);
                    
                }else if (transaction.Direction == "Credit")
                {
                    string description = $"#{transaction.Amount} would be added to acct {transaction.Account} with a new balance of {AvailableBalance + transaction.Amount}";
                    debited = walletLogic.UpdateBalance(transaction.Account, AvailableBalance + transaction.Amount);
                }

                if (!string.IsNullOrEmpty(debited))
                {
                    message = "Could not update the balance";
                    _logger.LogInformation(message);
                    res.Success = false;
                    res.Data = null;
                    res.Message = message;
                    return res;
                }

                var inserted = transactionLogic.TransactionInsert(transaction);

                if (!string.IsNullOrEmpty(inserted))
                {
                    message = "Could not insert transaction into Transaction table";
                    _logger.LogInformation(message);
                    res.Success = false;
                    res.Data = null;
                    res.Message = message;
                    return res;
                }
                else
                {
                    message = "Transaction inserted successfully";
                    _logger.LogInformation(message);
                    res.Success = true;
                    res.Data = null;
                    res.Message = message;
                    return res;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message + ":" + ex.StackTrace);
            }
            return res;
        }

        
    }
}
