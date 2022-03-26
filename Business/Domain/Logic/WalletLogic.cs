using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pyrros.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyrros.Domain.Logic
{
    public class WalletLogic : IWalletLogic
    {
        private readonly ILogger<WalletLogic> _logger;
        string error = "";

        private string connectionString;
        public WalletLogic(ILogger<WalletLogic> _logger, IOptions<PyrrosOptions> options)
        {
            this._logger = _logger;
            this.connectionString = options.Value.ConnectionString;
        }

        public DataSet GetWalletDetails(long account)
        {
            Connect con = new Connect(connectionString);

            try
            {
                con.SetProcedure("GetWalletDetails");
                con.AddParam("@Id", account);
                DataSet ds = con.Select();
                return ds;
            }
            catch (Exception ex)
            {
                error = ex.Message + ":" + ex.StackTrace;
                _logger.LogError(error);
                return null;
            }
            finally
            {
                con.Connection.Close();
            }
        }

        public string UpdateBalance(long account, decimal amount)
        {
            string ErrorMsg = "";
            int result = 0;
            string? error = "";

            Connect con = new Connect(connectionString);

            try
            {

                con.SetProcedure("Wallet_Update");
                con.AddParam("@AvailableBalance", amount);
                con.AddParam("@Id", account);

                result = con.Execute();
                ErrorMsg = con.errmsg;
                //logger.Info(ErrorMsg);
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
