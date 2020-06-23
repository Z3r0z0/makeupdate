using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using UpdateLib;

namespace MakeUpdate
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(args[0], optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            UpdateConfig updateConfig = new UpdateConfig();
            configuration.GetSection(UpdateResurce.UpdateConfigSection).Bind(updateConfig);

            Console.WriteLine(updateConfig.MarshalerPath);
            Console.WriteLine(updateConfig.Schema);

            Update update = new Update(updateConfig, args.Skip(1).ToArray());

            update.UpdateMessage += Console.WriteLine;
            update.ExecuteUpdate();
            update.Dispose();

            Console.ReadKey();

        }
    }
}
