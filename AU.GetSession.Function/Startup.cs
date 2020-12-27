using System.Linq;
using AU.GetSession.Domain.GetSession;
using AU.GetSession.Domain.Services.Repositories;
using AU.GetSession.External.Cosmos;
using AU.GetSession.Function;
using AU.GetSession.Function.Profiles;
using AU.GetSession.Services.External.Cosmos;
using AU.GetSession.Services.Repositories.Player;
using AutoMapper;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

[assembly: FunctionsStartup(typeof(Startup))]
namespace AU.GetSession.Function
{
    public class Startup : FunctionsStartup
    {
        private const string ConfigCosmosConnectionString = "CosmosDBConnection";
        private const string ConfigTableName = "Table";

        private IConfigurationRoot configRoot;

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            configRoot = builder.ConfigurationBuilder
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true) // Add our Local Settings.
                .AddEnvironmentVariables() // Add our Azure Settings (if hosted).
                .Build();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Replace our default IConfiguration for our application with our custom configuration root.
            builder.Services.Replace(ServiceDescriptor.Singleton(typeof(IConfiguration), configRoot));
            var config = (IConfiguration)builder.Services.First(d => d.ServiceType == typeof(IConfiguration)).ImplementationInstance;

            builder.Services.AddAutoMapper(typeof(ModelProfile));

            // Domain
            builder.Services.AddTransient<IGetSessionHandler, GetSessionHandler>();

            // Services
            builder.Services.AddTransient<IPlayerRepository, PlayerRepository>();

            // External
            builder.Services.AddTransient<ITableContext, TableContext>();
            builder.Services.AddSingleton<CloudTable>((s) =>
            {
                var storageAccount = CloudStorageAccount.Parse(config[ConfigCosmosConnectionString]);
                var cloudTableClinet = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

                return cloudTableClinet.GetTableReference(config[ConfigTableName]);
            });
        }
    }
}
