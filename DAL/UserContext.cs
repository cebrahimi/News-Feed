using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using News.Models;


namespace News.DAL
{
    public class UserContext
    {
        public string ConnectionString { get; set; }
        public UserContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        private MySqlConnection getConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<int> GetFavourites(int userId)
        {
            List<int> favourites = new List<int>();
            using (MySqlConnection conn = getConnection())
            {
                string sql = $"Select * from favorites WHERE userid='{userId}'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {    
                    favourites.Add(Convert.ToInt32(rdr["articleid"]));
                }                
                conn.Close();
            }
            return favourites;
        }

        public bool AddFavourite(int userId, int articleId)
        {
            using (MySqlConnection conn = getConnection())
            {
                string sql =
                    $"INSERT INTO favorites(userid, articleid) VALUES('{userId}', '{articleId}')";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    return false;
                }

                conn.Close();

                return true;
            }
        }

        public bool RemoveFavourite(int userId, int articleId)
        {
            using (MySqlConnection conn = getConnection())
            {
                string sql =
                    $"DELETE FROM favorites WHERE userid = '{userId}' AND articleid = '{articleId}'";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    return false;
                }

                conn.Close();

                return true;
            }
        }
        
        public User GetUser(int userId)
        {
            User user = new User();
            using (MySqlConnection conn = getConnection())
            {
                string sql = $"Select * from user WHERE userid='{userId}'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {                    
                    user.UserId = Convert.ToInt32(rdr["userid"]);
                    user.Username = rdr["username"].ToString();
                    user.Email = rdr["email"].ToString();
                }                
                conn.Close();
            }
            return user;
        }

        public int GetUserId(string email)
        {
            var userId = -1;
            using (MySqlConnection conn = getConnection())
            {
                string sql = $"Select userid from user WHERE email='{email}'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {                    
                    userId = Convert.ToInt32(rdr["userid"]);
                }
                conn.Close();
            }

            return userId;
        }
    }
}
