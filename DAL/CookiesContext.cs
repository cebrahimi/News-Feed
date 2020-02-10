using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;


namespace News.DAL
{
    public class CookiesContext
    {
        public string ConnectionString { get; set; }

        public CookiesContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        private MySqlConnection getConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public int GetSession(string sessionId)
        {
            int userId;
            using (MySqlConnection conn = getConnection())
            {
                string sql = $"SELECT userid FROM session WHERE sessionid = '{sessionId}'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();
                try
                {
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    rdr.Read();
                    userId = Convert.ToInt32(rdr["userid"]);
                }
                catch
                {
                    userId = -1;
                }
                conn.Close();
            }
            return userId;
        }
        public void DeleteSession(string sessionId)
        {
            using (MySqlConnection conn = getConnection())
            {
                string sql = $"DELETE FROM session WHERE sessionid = '{sessionId}'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        
        public string AddSession(int userid, string sessionid)
        {
            using (MySqlConnection conn = getConnection())
            {
                string returnVal;
                string format = "yyyy-MM-dd HH:mm:ss";
                string date = DateTime.Now.ToString(format);
                string sql = $"REPLACE INTO session(userid, sessionid, date) VALUES({userid},'{sessionid}','{date}')";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();

                try
                {
                    cmd.ExecuteNonQuery();
                    returnVal = "Success";
                }
                catch
                {
                    returnVal = "There is an existing session";
                }
                conn.Close();
                return returnVal;
            }
        }
    }
}
