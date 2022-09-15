using CliParser;
using Purin.Cmd.Services;

namespace Purin.Cmd
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var startup = new ProgramStartupService();
            //var test = new string[] { "types", "C:\\Code\\Purin\\Demo\\test.txt" };

            //var test = new string[] { "run", "-s", "C:\\Code\\Purin\\Demo\\test.txt", "-a", "helloworld", "-t" };

            //var test = new string[] { "inspect", "-s", "C:\\Code\\Purin\\Demo\\test.txt", "-t" };
            var test = new string[] { "repl", "-t"};
            test.Resolve(startup);
        }
    }
}