using System.IO;
using Akka.Actor;
using Akka.Configuration;
using Serilog;

namespace btm.lighthouse
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

            var root = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName;
            var cfg = ConfigurationFactory.ParseString(File.ReadAllText(Path.Combine(root, "Config.properties")));

            ActorSystem.Create("paassystem", cfg).WhenTerminated.Wait();
        }
    }
}
