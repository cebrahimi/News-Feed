using MySql.Data.MySqlClient;
using System;
using Microsoft.EntityFrameworkCore;

namespace News.DAL
{
    public class ResetContext
    {
        public string ConnectionString { get; set; }

        public ResetContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection getConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public int CheckKey(string key)
        {
            int userId = -1;

            using (MySqlConnection conn = getConnection())
            {
                string sql = $"SELECT * from user_password_reset where resetKey='{key}'";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    userId = Convert.ToInt32(reader["userId"]);
                }

                conn.Close();
            }
            return userId;
        }

        public void SetKey(int userId, string key)
        {
            using (MySqlConnection conn = getConnection())
            {
                string sql = $"INSERT INTO user_password_reset VALUES('{userId}', '{key}')";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();

                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        public void removeKey(int userId)
        {
            using (MySqlConnection conn = getConnection())
            {
                string sql = $"DELETE FROM user_password_reset WHERE userId = '{userId}'";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();

                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }
    }
}
