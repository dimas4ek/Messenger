using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;

namespace Database
{
    public class DatabaseManager
    {
        private static string connectionString;

        private const string Host = "containers-us-west-206.railway.app:7008";
        private const string Username = "postgres";
        private const string Password = "TUw5RQK6gaU3k2JiFfA8";
        private const string Database = "railway";

        public DatabaseManager()
        {
            connectionString = $"Host={Host};Username={Username};Password={Password};Database={Database}";
        }

        public static void Main()
        {
            new DatabaseManager();
            
            using (var con = new NpgsqlConnection(connectionString))
            {
                con.Open();

                const string sql = "SELECT version()";

                var cmd = new NpgsqlCommand(sql, con);

                var version = cmd.ExecuteScalar()?.ToString();
                Console.WriteLine($"PostgreSQL version: {version}");

                con.Close();
            }
        }

        public static void RegisterUser(string username, string password)
        {
            using (var con = new NpgsqlConnection(connectionString))
            {
                con.Open();

                var cmd = new NpgsqlCommand();
                cmd.Connection = con;

                cmd.CommandText = $"INSERT INTO users (username, password) VALUES ('{username}', '{password}')";
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        public static bool CheckIfUserExists(string username)
        {
            using (var con = new NpgsqlConnection(connectionString))
            {
                con.Open();

                var cmd = new NpgsqlCommand();
                cmd.Connection = con;

                cmd.CommandText = $"SELECT COUNT(*) FROM users WHERE username = '{username}'";
                var count = Convert.ToInt32(cmd.ExecuteScalar());

                con.Close();

                return count != 0;
            }
        }

        public static bool LoginUser(string username, string password)
        {
            using (var con = new NpgsqlConnection(connectionString))
            {
                con.Open();

                var cmd = new NpgsqlCommand();
                cmd.Connection = con;

                cmd.CommandText = $"SELECT COUNT(*) FROM users WHERE username = '{username}' AND password = '{password}'";
                var count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count != 0)
                {
                    User.username = username;
                    
                    con.Close();

                    return true;
                }

                con.Close();

                return false;
            }
        }

        public static List<int> LoadDialog(string me, string companion)
        {
            using (var con = new NpgsqlConnection(connectionString))
            {
                con.Open();

                var cmd = new NpgsqlCommand();
                cmd.Connection = con;

                cmd.CommandText = $"SELECT id FROM users WHERE username = '{me}'";
                var meId = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.CommandText = $"SELECT id FROM users WHERE username = '{companion}'";
                var companionId = Convert.ToInt32(cmd.ExecuteScalar());
                
                cmd.CommandText = $"SELECT id FROM dialogs WHERE (user1 = '{meId}' AND user2 = '{companionId}') OR (user1 = '{companionId}' AND user2 = '{meId}')";
                if (cmd.ExecuteScalar() == null)
                {
                    cmd.CommandText = $"INSERT INTO dialogs (user1, user2) VALUES ('{meId}', '{companionId}')";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $"SELECT id FROM dialogs WHERE (user1 = '{meId}' AND user2 = '{companionId}') OR (user1 = '{companionId}' AND user2 = '{meId}')";
                }

                var dialogId = Convert.ToInt32(cmd.ExecuteScalar());

                cmd.CommandText = $"SELECT * FROM dialog_messages WHERE dialog_id = {dialogId}";

                var reader = cmd.ExecuteReader();
                
                var messages = new List<int>();
                
                while (reader.Read())
                {
                    var messageId = reader.GetInt32(0);
                    messages.Add(messageId);
                }
                reader.Close();

                con.Close();

                return messages;
            }
        }

        public static void SaveMessage(string me, string companion, string message)
        {
            using (var con = new NpgsqlConnection(connectionString))
            {
                con.Open();

                var cmd = new NpgsqlCommand();
                cmd.Connection = con;

                cmd.CommandText = $"SELECT id FROM users WHERE username = '{me}'";
                var meId = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.CommandText = $"SELECT id FROM users WHERE username = '{companion}'";
                var companionId = Convert.ToInt32(cmd.ExecuteScalar());

                cmd.CommandText = $"SELECT id FROM dialogs WHERE (user1 = '{meId}' AND user2 = '{companionId}') OR (user1 = '{companionId}' AND user2 = '{meId}')";

                var dialogId = Convert.ToInt32(cmd.ExecuteScalar());

                cmd.CommandText = $"INSERT INTO dialog_messages (dialog_id, message, sender) VALUES ('{dialogId}', '{message}', '{meId}')";
                cmd.ExecuteNonQuery();
                
                con.Close();
            }
        }

        public static bool FindFriend(string friendName)
        {
            using (var con = new NpgsqlConnection(connectionString))
            {
                con.Open();

                var cmd = new NpgsqlCommand();
                cmd.Connection = con;

                cmd.CommandText = $"SELECT COUNT(*) FROM users WHERE username = '{friendName}'";
                var count = Convert.ToInt32(cmd.ExecuteScalar());

                con.Close();

                return count != 0;
            }
        }

        public static void AddFriend(string friendName)
        {
            using (var con = new NpgsqlConnection(connectionString))
            {
                con.Open();

                var cmd = new NpgsqlCommand();
                cmd.Connection = con;

                cmd.CommandText = $"SELECT id FROM users WHERE username = '{User.username}'";
                var meId = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.CommandText = $"SELECT id FROM users WHERE username = '{friendName}'";
                var friendId = Convert.ToInt32(cmd.ExecuteScalar());

                cmd.CommandText = $"UPDATE friend_list SET friends_id = array_append(friends_id, '{friendId}') WHERE user_id = '{meId}'";
                cmd.ExecuteNonQuery();

                if (cmd.ExecuteNonQuery() == 0)
                {
                    cmd.CommandText = $"INSERT INTO friend_list (user_id) VALUES ('{meId}')";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $"UPDATE friend_list SET friends_id = array_append(friends_id, '{friendId}') WHERE user_id = '{meId}'";
                    cmd.ExecuteNonQuery();
                }

                cmd.CommandText = $"UPDATE friend_list SET friends_id = array_append(friends_id, '{meId}') WHERE user_id = '{friendId}'";
                cmd.ExecuteNonQuery();

                if (cmd.ExecuteNonQuery() == 0)
                {
                    cmd.CommandText = $"INSERT INTO friend_list (user_id) VALUES ('{friendId}')";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $"UPDATE friend_list SET friends_id = array_append(friends_id, '{meId}') WHERE user_id = '{friendId}'";
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
        }

        public static List<string> GetFriendList()
        {
            using (var con = new NpgsqlConnection(connectionString))
            {
                con.Open();

                var cmd = new NpgsqlCommand();
                cmd.Connection = con;

                cmd.CommandText = $"SELECT id FROM users WHERE username = '{User.username}'";
                var meId = Convert.ToInt32(cmd.ExecuteScalar());

                cmd.CommandText = $"SELECT friends_id FROM friend_list WHERE user_id = '{meId}'";
                var reader = cmd.ExecuteReader();

                var friends = new List<string>();
                var friendsId = Array.Empty<int>();

                while (reader.Read())
                {
                    friendsId = (int[])reader.GetValue(0);
                }
                reader.Close();

                foreach (var id in friendsId)
                {
                    cmd.CommandText = $"SELECT username FROM users WHERE id = '{id}'";
                    var username = cmd.ExecuteScalar()?.ToString();
                    friends.Add(username);
                    cmd.Cancel();
                    cmd.CommandText = "";
                }

                con.Close();

                return friends;
            }
        }

        public static bool CheckSender(int messageId)
        {
            using (var con = new NpgsqlConnection(connectionString))
            {
                con.Open();

                var cmd = new NpgsqlCommand();
                cmd.Connection = con;

                cmd.CommandText = $"SELECT sender FROM dialog_messages WHERE id = '{messageId}'";
                var senderId = Convert.ToInt32(cmd.ExecuteScalar());

                cmd.CommandText = $"SELECT username FROM users WHERE id = '{senderId}'";
                var senderName = cmd.ExecuteScalar()?.ToString();

                con.Close();

                return senderName == User.username;
            }
        }

        public static string GetMessage(int messageId)
        {
            using (var con = new NpgsqlConnection(connectionString))
            {
                con.Open();

                var cmd = new NpgsqlCommand();
                cmd.Connection = con;

                cmd.CommandText = $"SELECT message FROM dialog_messages WHERE id = '{messageId}'";
                var message = cmd.ExecuteScalar()?.ToString();

                con.Close();

                return message;
            }
        }

        public static List<string> FriendsFromSearch(string text)
        {
            using (var con = new NpgsqlConnection(connectionString))
            {
                con.Open();

                var cmd = new NpgsqlCommand();
                cmd.Connection = con;

                cmd.CommandText = $"SELECT username FROM users WHERE username LIKE '{text}%'";
                var reader = cmd.ExecuteReader();

                var friends = new List<string>();

                while (reader.Read())
                {
                    friends.Add(reader.GetValue(0).ToString());
                }
                reader.Close();

                con.Close();

                return friends;
            }
        }

        public static bool AlreadyFriends(string username, string friendName)
        {
            using (var con = new NpgsqlConnection(connectionString))
            {
                con.Open();

                var cmd = new NpgsqlCommand();
                cmd.Connection = con;

                cmd.CommandText = $"SELECT id FROM users WHERE username = '{username}'";
                var userId = Convert.ToInt32(cmd.ExecuteScalar());

                cmd.CommandText = $"SELECT friends_id FROM friend_list WHERE user_id = '{userId}'";
                var reader = cmd.ExecuteReader();

                var friendsId = Array.Empty<int>();

                while (reader.Read())
                {
                    friendsId = (int[])reader.GetValue(0);
                }
                reader.Close();

                cmd.CommandText = $"SELECT id FROM users WHERE username = '{friendName}'";
                var friendId = Convert.ToInt32(cmd.ExecuteScalar());

                if (friendsId.Any(id => id == friendId))
                {
                    con.Close();
                    return true;
                }

                con.Close();

                return false;
            }
        }
    }
}
