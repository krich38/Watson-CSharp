using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Database
    {
        private static SQLiteConnection m_Connection;

        public static SQLiteConnection GetConnection()
        {
            return m_Connection;
        }

        public static void ExecuteUpdate(string sql)
        {
            SQLiteCommand command = new SQLiteCommand(sql, m_Connection);
            command.ExecuteNonQuery();

        }

        public static SQLiteDataReader ExecuteQuery(String sql)
        {
            SQLiteCommand command = new SQLiteCommand(sql, m_Connection);
            return command.ExecuteReader();
        }
    }
}
