using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Commands
{
    class MarkovDatabaseAdapter
    {
        private static SQLiteConnection connection;

        public static bool Setup()
        {

            connection = Database.GetConnection();

            Database.ExecuteUpdate("create table if not exists markov (seed_a TEXT, seed_b TEXT, seed_c TEXT, unique(seed_a, seed_b, seed_c) on conflict ignore)");
            Database.ExecuteUpdate("create table if not exists markov (seed_a TEXT, seed_b TEXT, seed_c TEXT, unique(seed_a, seed_b, seed_c) on conflict ignore)");
            Database.ExecuteUpdate("ATTACH DATABASE ':memory:' AS mem");
            Database.ExecuteUpdate("create table if not exists mem.markov (seed_a TEXT, seed_b TEXT, seed_c TEXT, unique(seed_a, seed_b, seed_c) on conflict ignore)");
            Database.ExecuteUpdate("insert into mem.markov (seed_a, seed_b, seed_c) select seed_a, seed_b, seed_c from markov");



            return true;
        }

        public static string MarkovGenerate()
        {
            return "";
        }

        public static void MarkovLearn(string text)
        {

        }
    }
}
