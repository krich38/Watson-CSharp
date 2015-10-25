using System.Threading;

namespace ConsoleApplication1
{
    class Program
    {
        private static volatile Program INSTANCE;


        static void Main(string[] args)
        {
            Program program = new Program();
            INSTANCE = program;

            IRCServer server = new IRCServer("irc.moparisthebest.com", 6697, new string[] { "#bottest" }, true);
            program.Connect(server);
        }

        private void Connect(IRCServer server)
        {
            ConnectionWorker worker = new ConnectionWorker(server);
            Thread t = new Thread(worker.Process);
            t.Start();
        }

        public bool IsRunning()
        {
            return true;
        }

        public static Program GetInstance()
        {

            return INSTANCE;

        }

    }
}
