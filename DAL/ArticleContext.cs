using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;


namespace News.Models
{
    public class ArticleContext : DbContext
    {
        public string ConnectionString { get; set; }

        public ArticleContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        private List<String> GetGenres(int articleId)
        {
            List<String> genres = new List<String>();

            using (MySqlConnection con = GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand("getGenres", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@articleId", articleId);
                cmd.Parameters["@articleId"].Direction = ParameterDirection.Input;

                con.Open(); //open db connection

                MySqlDataReader rdr = cmd.ExecuteReader();

                //while loop
                while (rdr.Read())
                {
                    genres.Add(rdr["genre"].ToString());
                }

                con.Close();
            }

            return genres;
        }


        public List<Article> GetAllArticles(int websiteId = 3)
        {
            DateTime ArticleDateRange = DateTime.Now.AddDays(-10);

            List<Article> articles = new List<Article>();
            using (MySqlConnection con = GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand("getArticles", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@websiteId", websiteId);
                cmd.Parameters.AddWithValue("@ArticleDateRange", ArticleDateRange);
                cmd.Parameters["@websiteId"].Direction = ParameterDirection.Input;
                cmd.Parameters["@ArticleDateRange"].Direction = ParameterDirection.Input;
                con.Open(); //open db connection

                MySqlDataReader rdr = cmd.ExecuteReader();

                //while loop
                while (rdr.Read())
                {
                    Article w = new Article();
                    w.ID = Convert.ToInt32(rdr["articleid"]);
                    w.Title = rdr["title"].ToString();
                    w.WebsiteUrl = rdr["websiteUrl"].ToString();
                    w.PublisherUrl = rdr["publisherUrl"].ToString();
                    w.Date = rdr["date"].ToString();
                    w.WebsiteName = rdr["websiteName"].ToString();
                    w.Genres = GetGenres(w.ID);
                    w.Description = rdr["description"].ToString();

                    articles.Add(w);
                }

                con.Close();
            }

            return articles;
        }

        public List<Article> GetArticles(List<int> articleIds)
        {
            List<Article> articles = new List<Article>();
            using (MySqlConnection con = GetConnection())
            {
                var result = String.Join(", ", articleIds.ToArray());
                string sql =
                    $"SELECT a.articleId, title, a.url as websiteUrl, websiteName FROM article a INNER JOIN website w ON a.websiteid=w.websiteid WHERE a.articleid in ({result})";
                MySqlCommand cmd = new MySqlCommand(sql, con);

                con.Open(); //open db connection

                MySqlDataReader rdr = cmd.ExecuteReader();

                //while loop
                while (rdr.Read())
                {
                    Article w = new Article();
                    w.ID = Convert.ToInt32(rdr["articleid"]);
                    w.Title = rdr["title"].ToString();
                    w.WebsiteUrl = rdr["websiteUrl"].ToString();
                    w.WebsiteName = rdr["websiteName"].ToString();

                    articles.Add(w);
                }

                con.Close();
            }

            return articles;
        }

        public Boolean CheckConnection()
        {
            MySqlConnection con = GetConnection();
            try
            {
                con.Open();
                con.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}