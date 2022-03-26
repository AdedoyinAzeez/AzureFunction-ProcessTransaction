using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pyrros.Domain;
using Pyrros.Domain.Logic;
using Pyrros.Interface;
using Pyrros.Models;
using Pyrros.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(Pyrros.Startup))]
namespace Pyrros
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddHttpClient();

            builder.Services.AddOptions<PyrrosOptions>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("PyrrosOptions").Bind(settings);
            });

            builder.Services.AddSingleton<ITransactionService, TransactionRepository>();
            builder.Services.AddSingleton<IWalletService, WalletRepository>();
            builder.Services.AddSingleton<IWalletLogic, WalletLogic>();
            builder.Services.AddSingleton<ITransactionLogic, TransactionLogic>();

            builder.Services.AddLogging();

        }
    }
}
