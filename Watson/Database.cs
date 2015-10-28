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

        public static UserAccess AuthenticateUser(String username, String password)
        {
            UserAccess access = UserAccess.ANYONE;
            try
            {
                SQLiteDataReader reader = ExecuteQuery("SELECT * FROM users WHERE username='" + username + "'");
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        if (reader.GetString(1).Equals(password))
                        {
                            return UserAccessAttr.GetByAccess(reader.GetInt32(2));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return access;
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

        public static string GetKeyValue(IRCServer server, string key)
        {

            SQLiteCommand cmd = new SQLiteCommand("select value from keyvalues where server = @ip and key = @key", GetConnection());
            cmd.Parameters.AddWithValue("@ip", server.IP);
            cmd.Parameters.AddWithValue("@key", key);
            SQLiteDataReader rs = cmd.ExecuteReader();
            if (rs.HasRows)
            {
                if (rs.Read())
                {
                    return rs["value"].ToString();
                }
            }

            return null;
        }

        public static void SetKeyValue(IRCServer server, string key, object value)
        {
            SQLiteCommand cmd = new SQLiteCommand("insert into keyvalues (server, key, value) values (@server, @key, @value)", GetConnection());
            cmd.Parameters.AddWithValue("@server", server.IP);
            cmd.Parameters.AddWithValue("@key", key);
            cmd.Parameters.AddWithValue("@value", value.ToString());
            cmd.ExecuteNonQuery();

        }
    }
}
