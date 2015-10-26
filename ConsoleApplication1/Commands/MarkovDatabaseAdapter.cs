using System;
using System.Data.SQLite;
using System.Text;

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
            SQLiteDataReader rs = Database.ExecuteQuery("select seed_a, seed_b from mem.markov order by random() limit 1");
            if (rs.HasRows)
            {
                string found1 = null, found2 = null;
                if (rs.Read())
                {
                    found1 = rs["seed_a"].ToString();
                }
                if (rs.Read())
                {
                    found2 = rs["seed_b"].ToString();
                }
                return MarkovGenerate(found1, found2);
            }
            return null;
        }

        public static String MarkovFind(String seed1, String seed2)
        {
            SQLiteCommand cmd;
            if (seed2 == null)
            {
                cmd = new SQLiteCommand("select seed_a, seed_b from mem.markov where seed_a = @s1 or seed_b = @s2 COLLATE NOCASE order by random() limit 1", connection);
                cmd.Parameters.AddWithValue("@s1", seed1);
                cmd.Parameters.AddWithValue("@s2", seed2);
            }
            else
            {
                cmd = new SQLiteCommand("select seed_a, seed_b from mem.markov where seed_a in (@s1, @s2) or seed_b in (@s1, @s2) COLLATE NOCASE order by random() limit 1", connection);
                cmd.Parameters.AddWithValue("@s1", seed1);
                cmd.Parameters.AddWithValue("@s2", seed2);
            }
            SQLiteDataReader rs = cmd.ExecuteReader();
            if (rs.HasRows)
            {
                string found1 = null, found2 = null;
                if (rs.Read())
                {
                    found1 = rs["seed_a"].ToString();
                }
                if (rs.Read())
                {
                    found2 = rs["seed_b"].ToString();
                }

                return MarkovGenerate(found1, found2);
            }


            return null;
        }

        private static string MarkovGenerate(string seed1, string seed2)
        {
            StringBuilder result = new StringBuilder();
            if (seed1 == null)
            {
                seed1 = "\n";
            }
            if (seed2 == null)
            {
                seed2 = "\n";
            }
            if (!seed1.Equals("\n"))
            {
                result.Append(seed1);
                result.Append(' ');
            }
            if (!seed2.Equals("\n"))
            {
                result.Append(seed2);
            }
            int wordcount = CommandManager.RANDOM.Next(30) + 10;
            int type = CommandManager.RANDOM.Next(3);
            switch (type)
            {
                case 0:
                    MarkovFillBackwards(result, wordcount, seed1, seed2);
                    break;
                case 1:
                    MarkovFillForwards(result, wordcount, seed1, seed2);
                    break;
                default:
                    int num = MarkovFillBackwards(result, wordcount / 2, seed1, seed2);
                    MarkovFillForwards(result, wordcount - num, seed1, seed2);
                    break;
            }
            if (result.Length > 0)
            {
                return result.ToString().Trim();
            }
            else
            {
                return null;
            }
        }

        private static int MarkovFillForwards(StringBuilder result, int wordcount, String seed1, String seed2)
        {
            int count = 0;
            for (int i = 0; i < wordcount / 2; i++)
            {
                String seed3 = MarkovNextSeed(seed1, seed2);
                if (seed3 == null || seed3.Equals("\n"))
                {
                    break;
                }
                count++;
                if (result.Length > 0)
                {
                    result.Append(' ');
                }
                result.Append(seed3);
                seed1 = seed2;
                seed2 = seed3;
            }
            return count;
        }

        private static String MarkovNextSeed(String seed1, String seed2)
        {
            SQLiteCommand cmd = new SQLiteCommand("select seed_c from mem.markov where seed_a = @s1 and seed_b = @s2 order by random() limit 1", connection);
            cmd.Parameters.AddWithValue("@s1", seed1);
            cmd.Parameters.AddWithValue("@s2", seed2);
            SQLiteDataReader rs = cmd.ExecuteReader();
            if (rs.HasRows)
            {
                if (rs.Read())
                {
                    return rs["seed_c"].ToString();
                }
            }

            return null;
        }

        private static int MarkovFillBackwards(StringBuilder result, int wordcount, String seed1, String seed2)
        {
            int count = 0;
            for (int i = 0; i < wordcount; i++)
            {
                String seed0 = MarkovPreviousSeed(seed1, seed2);
                if (seed0 == null || seed0.Equals("\n"))
                {
                    break;
                }
                count++;
                if (result.Length > 0)
                {
                    result.Insert(0, ' ');
                }
                result.Insert(0, seed0);
                seed2 = seed1;
                seed1 = seed0;
            }
            return count;
        }

        private static string MarkovPreviousSeed(String seed2, String seed3)
        {



            SQLiteCommand cmd = new SQLiteCommand("select seed_a from mem.markov where seed_b = @s1 and seed_c = @s2 order by random() limit 1", connection);
            cmd.Parameters.AddWithValue("@s1", seed2);
            cmd.Parameters.AddWithValue("@s2", seed3);

            SQLiteDataReader rs = cmd.ExecuteReader();
            if (rs.HasRows)
            {
                if (rs.Read())
                {
                    return rs["seed_a"].ToString();
                }
            }

            return null;
        }

        public static void MarkovLearn(string input)
        {
            string seed1, seed2;
            seed1 = seed2 = "\n";
            string[] words = input.Split(' ');
            foreach (string seed3 in words)
            {
                MarkovInsert(seed1, seed2, seed3);
                seed1 = seed2;
                seed2 = seed3;
            }
        }

        private static void MarkovInsert(string seed1, string seed2, string seed3)
        {

            SQLiteCommand cmd = new SQLiteCommand("insert into mem.markov(seed_a, seed_b, seed_c) values(@s1, @s2, @s3)", connection);

            cmd.Parameters.AddWithValue("@s1", seed1);
            cmd.Parameters.AddWithValue("@s2", seed2);
            cmd.Parameters.AddWithValue("@s3", seed3);

            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();

            cmd = new SQLiteCommand("insert into markov (seed_a, seed_b, seed_c) values (@s1, @s2, @s3)", connection);
            cmd.Parameters.AddWithValue("@s1", seed1);
            cmd.Parameters.AddWithValue("@s2", seed2);
            cmd.Parameters.AddWithValue("@s3", seed3);

            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();

        }
    }
}
