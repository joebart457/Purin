using CliParser;
using Purin.Cmd.Services;

namespace Purin.Cmd
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var startup = new ProgramStartupService();
            //var test = new string[] { "inspect", "C:\\Code\\Purin\\Demo\\test.txt" };

            var test = new string[] { "run", "-s", "C:\\Code\\Purin\\Demo\\test.txt", "-a", "helloworld", "-t" };
            test.Resolve(startup);
        }
    }
}