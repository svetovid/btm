using System;
using System.IO;
using Akka.Actor;
using Akka.Configuration;

namespace testlighthouse
{
    class Program
    {
        static void Main(string[] args)
        {
            var root = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName;
            var cfg = ConfigurationFactory.ParseString(File.ReadAllText(Path.Combine(root, "Config.properties")));

            ActorSystem.Create("myactorsys", cfg).WhenTerminated.Wait();
        }
    }
}
