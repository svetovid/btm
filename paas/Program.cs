using System.Diagnostics;
using System.IO;
using System.Linq;
using Akka.Actor;
using Akka.Configuration;
using btm.paas.Actors;
using Serilog;

namespace btm.paas
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 0;
            if (args.Any())
            {
                int.TryParse(args[0], out port);
            }

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                //.WriteTo.MSSqlServer("Server=.;Database=akka_logging;Integrated Security=SSPI;", "Logs")
                //.MinimumLevel.Information()
                .CreateLogger();

            var root = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName;
            var cfg = ConfigurationFactory.ParseString(File.ReadAllText(Path.Combine(root, "Config.properties")));
            var finalConfig = port > 0
                ? ConfigurationFactory.ParseString($"akka.remote.dot-netty.tcp.port = {port}").WithFallback(cfg)
                : cfg;

            Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));

            var _paasActorSystem = ActorSystem.Create("paassystem", finalConfig);
            _paasActorSystem.ActorOf(Props.Create(()=> new PaasActor()), "paas");
            _paasActorSystem.WhenTerminated.Wait();
        }
    }
}
