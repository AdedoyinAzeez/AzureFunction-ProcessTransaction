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
    public class WalletRepository : IWalletService
    {
        ServiceResponse res = new ServiceResponse();
        private readonly IWalletLogic walletLogic;
        private readonly ILogger<WalletRepository> _logger;
        string message = "";

        public WalletRepository()
        {

        }

        public WalletRepository(IWalletLogic walletLogic, ILogger<WalletRepository> logger)
        {
            this.walletLogic = walletLogic;
            this._logger = logger;
        }
        public async Task<Tuple<object, string>> GetWalletDetails(long account)
        {
            try
            {
                DataSet details = walletLogic.GetWalletDetails(account);
                if (details.Tables.Count > 0)
                {
                    DataTable dt = details.Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        Wallet wallet = new Wallet
                        {
                            Id = (long)dt.Rows[0]["Id"],
                            AvailableBalance = (decimal)dt.Rows[0]["ActualBalance"],
                            CurrencyCode = (string)dt.Rows[0]["CurrencyCode"],
                            CustomerID = (long)dt.Rows[0]["CustomerID"]
                        };

                        message = "Wallet details fetched successfully";
                        _logger.LogInformation(message);
                        res.Success = true;
                        res.Data = wallet;
                        res.Message = message;
                        return new Tuple<object, string>(res, "success");
                    }
                    else
                    {
                        message = "No wallet found";
                        _logger.LogInformation(message);
                        res.Success = true;
                        res.Data = null;
                        res.Message = message;
                        return new Tuple<object, string>(res, "success");
                    }

                }
                else
                {
                    message = "Wallet Details cannot be found";
                    _logger.LogError(message);
                    res.Success = false;
                    res.Data = null;
                    res.Message = message;
                    return new Tuple<object, string>(res, "error");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message + ":" + ex.StackTrace);
            }
            return new Tuple<object, string>(res, "error");
        }

        public async Task<Tuple<object, string>> UpdateWallet(long account, long amount)
        {
            try
            {
                DataSet details = walletLogic.GetWalletDetails(account);
                if (details.Tables.Count > 0)
                {
                    string updated = walletLogic.UpdateBalance(account, amount);

                    message = "Wallet updated successfully";
                    _logger.LogInformation(message);
                    res.Success = true;
                    res.Data = null;
                    res.Message = message;
                    return new Tuple<object, string>(res, "success");
                }
                else
                {
                    message = "Wallet does not exist";
                    _logger.LogError(message);
                    res.Success = false;
                    res.Data = null;
                    res.Message = message;
                    return new Tuple<object, string>(res, "error");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message + ":" + ex.StackTrace);
            }
            return new Tuple<object, string>(res, "error");
        }
    }
}
