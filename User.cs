using Discord;
using Discord.WebSocket;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DiscordTeamsBot
{
    public class User
    {
        private const String SERVER = "localhost";
        private const String DATABASE = "test_db";
        private const String UID = "root";
        private const String PASSWORD = "root";
        private static MySqlConnection dbConn;

        public int Id { get; private set; }

        public ulong userId { get; private set; }

        public int Currency { get; private set; }

        private User(int id, ulong uId, int curr)
        {
            Id = id;
            userId = uId;
            Currency = curr;
        }

        public static void InitializeDB()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();

            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = SERVER;
            builder.UserID = UID;
            builder.Password = PASSWORD;
            builder.Database = DATABASE;

            String ConnectionString = builder.ConnectionString;

            builder = null;

            dbConn = new MySqlConnection(ConnectionString);

            Console.WriteLine($"Database initialized! ({sw.ElapsedMilliseconds.ToString()}ms)");

            sw.Stop();
        }

        public static List<User> GetUsers()
        {
            List<User> users = new List<User>();

            String query = "SELECT * FROM test_db.discordtest";

            MySqlCommand cmd = new MySqlCommand(query, dbConn);

            dbConn.Open();

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = (int)reader["id"];
                ulong userID = (ulong)reader["userId"];
                int currency = (int)reader["currency"];

                User u = new User(id, userID, currency);

                users.Add(u);
            }

            dbConn.Close();

            return users;
        }

        public static void GetBalance(ulong userId, SocketUserMessage msg)
        {
            String query = string.Format("SELECT * FROM discordtest WHERE userId='{0}'", userId);

            MySqlCommand cmd = new MySqlCommand(query, dbConn);

            dbConn.Open();

            MySqlDataReader reader = cmd.ExecuteReader();

            reader.Read();

            int currency = (int)reader["currency"];

            dbConn.Close();

            EmbedBuilder eb = new EmbedBuilder();
            EmbedFooterBuilder efb = new EmbedFooterBuilder();

            eb.AddField("Currency", $"**{currency}**");

            eb.Color = Color.DarkOrange;

            eb.WithCurrentTimestamp();

            efb.IconUrl = msg.Author.GetAvatarUrl();

            eb.WithFooter(efb);

            var embed = eb.Build();

            msg.Channel.SendMessageAsync(embed: embed);
        }

        public static User Insert(ulong uId, int currency)
        {
            String query = string.Format("INSERT INTO discordtest(userId, currency) VALUES ('{0}', '{1}')", uId, currency);

            MySqlCommand cmd = new MySqlCommand(query, dbConn);

            dbConn.Open();

            cmd.ExecuteNonQuery();
            int id = (int)cmd.LastInsertedId;

            User user = new User(id, uId, currency);

            dbConn.Close();

            return user;
        }

        public static void Update(ulong uId, int currency)
        {
            String query = string.Format("UPDATE discordtest SET userId='{0}', currency='{1}' WHERE userId='{0}'", uId, currency);

            MySqlCommand cmd = new MySqlCommand(query, dbConn);

            dbConn.Open();

            cmd.ExecuteNonQuery();

            dbConn.Close();
        }

        public void Delete(int uId)
        {
            String query = string.Format("DELETE FROM discordtest WHERE userId={0}", uId);

            MySqlCommand cmd = new MySqlCommand(query, dbConn);

            dbConn.Open();

            cmd.ExecuteNonQuery();

            dbConn.Close();
        }
    }
}
