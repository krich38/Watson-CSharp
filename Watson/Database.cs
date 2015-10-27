using System;
using System.Data.SQLite;

namespace Watson
{
    class Database
    {
        private static SQLiteConnection m_Connection;

        public static bool EstablishConnection()
        {
            try
            {
                m_Connection = new SQLiteConnection("Data Source=watson.db;Version=3;");
                m_Connection.Open();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Source);
                return false;
            }
        }

        public static SQLiteConnection GetConnection()
        {
            return m_Connection;
        }

        public static void ExecuteUpdate(string sql)
        {
            SQLiteCommand command = new SQLiteCommand(sql, m_Connection);
            command.ExecuteNonQuery();
        }

        public static SQLiteDataReader ExecuteQuery(string sql)
        {
            SQLiteCommand command = new SQLiteCommand(sql, m_Connection);
            return command.ExecuteReader();
        }
    }
}
