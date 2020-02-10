using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using News.Models;

namespace News.DAL
{
    public class GenreContext
    {
        public string ConnectionString { get; set; }

        public GenreContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection getConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<Genre> ListGenres(int websiteid)

        {
            List<Genre> GenreList = new List<Genre>();

            using (MySqlConnection conn = getConnection())
            {
                string sql = $"SELECT  websiteid, g.genre as genre, g.genreid as genreid FROM rssfeed r INNER JOIN genre g ON r.genreid = g.genreid WHERE r.websiteid = {websiteid}";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())

                {
                    Genre g = new Genre();
                    g.genreID = Convert.ToInt32(reader["genreid"]);
                    g.genre = reader["genre"].ToString();
                    GenreList.Add(g);
                }

                conn.Close();
            }
            return GenreList;
        }
    }
}
