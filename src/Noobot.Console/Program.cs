using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Noobot.Core;
using Noobot.Core.Configuration;
using Noobot.Core.Extensions;

namespace Noobot.Console
{
    public class Program
    {
        private static INoobotCore _noobotCore;
        private static readonly ManualResetEvent _quitEvent = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            System.Console.WriteLine("Starting Noobot...");
            AppDomain.CurrentDomain.ProcessExit += ProcessExitHandler; // closing the window doesn't hit this in Windows
            System.Console.CancelKeyPress += ConsoleOnCancelKeyPress;

            RunNoobot()
                .GetAwaiter()
                .GetResult();

            _quitEvent.WaitOne();
        }
        
        private static async Task RunNoobot()
        {
            var configReader = JsonConfigReader.DefaultLocation();

            var services = new ServiceCollection()
                .AddLogging(logging =>
                {
                    logging.AddConsole();
                })
                .AddNoobotCore(configReader);
            var serviceProvider = services.BuildServiceProvider();

            _noobotCore = serviceProvider.GetRequiredService<INoobotCore>();

            await _noobotCore.Connect();
        }

        private static void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs consoleCancelEventArgs)
        {
            _quitEvent.Set();
            consoleCancelEventArgs.Cancel = true;
        }

        // not hit
        private static void ProcessExitHandler(object sender, EventArgs e)
        {
            System.Console.WriteLine("Disconnecting...");
            _noobotCore?.Disconnect();
        }
    }
}
