using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlanningPoker.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PlanningPoker.TerminalClient
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets("PPTC-secrets");

            var Configuration = builder.Build();

            var serviceProvider = new ServiceCollection()
                .AddOptions()
                .AddPlanningPokerClient(Configuration)
                .AddSingleton<PokerTerminal, PokerTerminal>()
                .AddSingleton<PlanningClient, PlanningClient>()
                .BuildServiceProvider();

            var planningClient = serviceProvider.GetService<PlanningClient>();

            var cancellationSource = new CancellationTokenSource();
            await planningClient.Start(cancellationSource);

            do
            {
                while (!Console.KeyAvailable)
                {
                    // Do something
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

            Console.WriteLine("Exiting program...");
            cancellationSource.Cancel();
            Thread.Sleep(1000);
            Console.WriteLine("Done...");
            Console.ReadLine();
        }
    }
}
