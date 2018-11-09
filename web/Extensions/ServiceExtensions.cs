using System.IO;
using Akka.Actor;
using Akka.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace btm.web.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddPaasActorSystem(this IServiceCollection services)
        {
            var cfg = ConfigurationFactory.ParseString(File.ReadAllText("Config.properties"));
            var actorSystem = ActorSystem.Create("paassystem", cfg);

            services.AddSingleton(actorSystem);

            return services;
        }
    }
}
