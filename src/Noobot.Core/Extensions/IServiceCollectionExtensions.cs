using Microsoft.Extensions.DependencyInjection;
using Noobot.Core.Configuration;
using Noobot.Core.MessagingPipeline.Middleware;
using Noobot.Core.MessagingPipeline.Middleware.StandardMiddleware;
using Noobot.Core.Plugins.StandardPlugins;

namespace Noobot.Core.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddNoobotCore(this IServiceCollection services, IConfigReader configReader)
        {
            services.AddSingleton(configReader);
            services.AddSingleton<INoobotCore, NoobotCore>();

            // Middleware
            services.AddTransient<IMiddleware, UnhandledMessageMiddleware>();
            if (configReader.AboutEnabled)
            {
                services.Decorate<IMiddleware, AboutMiddleware>();
            }
            if (configReader.StatsEnabled)
            {
                services.Decorate<IMiddleware, StatsMiddleware>();
            }
            if (configReader.HelpEnabled)
            {
                services.Decorate<IMiddleware, HelpMiddleware>();
            }
            services.Decorate<IMiddleware, BeginMessageMiddleware>();

            // Plugins
            services.AddSingleton<StatsPlugin>();

            return services;
        }
    }
}