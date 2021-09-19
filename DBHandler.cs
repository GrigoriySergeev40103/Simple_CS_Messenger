using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using MySql.Data.MySqlClient;

namespace CSMessanger
{
    public static class DBHandler
    {
        static string connectionString =
            "server=127.0.0.1;uid=messanger_app;pwd=5804NfvltnFJ64Q;database=messanger_data";
        private static MySqlConnection connection;
        public static MySqlConnection Connection => connection;
        private static DispatcherTimer updateTimer = new DispatcherTimer();
        
        
        private static int clientUserId;
        private static string clientUserName;
        private static List<IUpdatable> updatables = new List<IUpdatable>();
        private static List<GroupChat> groupChats = new List<GroupChat>();
        private static List<User> users = new List<User>();

        public static List<GroupChat> GroupChats => groupChats;
        public static List<User> Interlocutors => users.FindAll(u => u.PrivateMessages.Count != 0);
        public static string Login => clientUserName;
        public static int ClientUserId => clientUserId;

        public static List<IUpdatable> Updatables => updatables;

        public static void Connect()
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();

            updateTimer.Interval = new TimeSpan(50000000);
            updateTimer.Tick += UpdateData;
            updateTimer.Start();
        }

        /// <summary>
        /// Пытается войти за пользователя с заданным логином и паролем.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns> True - если получилось. False - если нет пользователя с заданным логином и паролем. </returns>
        public static bool TryLogIn(string login, string password)
        {
            string sql = $"SELECT id FROM user_data WHERE login = '{login}' AND password = '{password}'";
            int? userId;

            using (MySqlCommand sqlCommand = new MySqlCommand(sql, connection))
            {
                userId = sqlCommand.ExecuteScalar() as int?;
                if (userId != null)
                {
                    clientUserId = userId.Value;
                    clientUserName = login;
                    return true;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// Пытается зарегистрировать пользователя.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns> True - если удалось зарегистрироваться. False - если учётная запись с таким логином уже существует.</returns>
        public static bool TryRegister(string login, string password)
        {
            string sql = $"SELECT EXISTS(SELECT * FROM user_data WHERE login = '{login}')";

            using (MySqlCommand sqlCommand = new MySqlCommand(sql, connection))
            {
                if (Convert.ToInt32(sqlCommand.ExecuteScalar()) == 1)
                {
                    return false;
                }
                else
                {
                    sql = "INSERT INTO user_data(login, password) " +
                        $"VALUES('{login}', '{password}')";
                    sqlCommand.CommandText = sql;
                    sqlCommand.ExecuteNonQuery();

                    // Getting the uid for the registered user
                    sql = $"SELECT uid FROM user_data WHERE login = '{login}'";
                    sqlCommand.CommandText = sql;

                    clientUserId = (int)sqlCommand.ExecuteScalar();
                    clientUserName = login;

                    return true;
                }
            }
        }

        public static void GetAllInterlocutors()
        {
            string sql = "SELECT DISTINCT(sent_by_uid), user_data.login FROM messanger_data.private_messages " +
                         "JOIN user_data ON user_data.id = sent_by_uid " +
                         $"WHERE sent_to_uid = {clientUserId}";


            using (MySqlCommand sqlCommand = new MySqlCommand(sql, connection))
            {
                using (MySqlDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        User interlocutor = new User(dataReader.GetInt32(0), dataReader.GetString(1));

                        updatables.Add(interlocutor);
                        TryAddUser(interlocutor);
                    }

                    dataReader.Close();
                }
            }
        }

        /// <summary>
        /// Загружает из базы данных id и название групповых чатов, в которых состоит пользователь.
        /// </summary>
        public static void GetAllGroupChatsBasicInfo()
        {
            string sql = $"SELECT group_chat_members.gc_id, name " +
                          "FROM group_chat_members " +
                          "JOIN group_chat_data ON group_chat_members.gc_id = group_chat_data.id " +
                         $"WHERE group_chat_members.uid = {clientUserId}";

            using (MySqlCommand sqlCommand = new MySqlCommand(sql, connection))
            {
                using (MySqlDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        GroupChat groupChat = new GroupChat(dataReader.GetInt32(0), dataReader.GetString(1));

                        updatables.Add(groupChat);
                        groupChats.Add(groupChat);
                    }

                    dataReader.Close();
                }
            }
        }


        public static User TryAddUser(User user)
        {
            if (users.Contains(user))
                return users.Find(u => u.Id == user.Id);
            else
            {
                users.Add(user);
                return user;
            }
        }

        /// <summary>
        /// Отправляет сообщение пользователю с заданным id.
        /// </summary>
        /// <param name="message"> Текст сообщения. </param>
        /// <param name="receiver"></param>
        public static void SendPrivateMessage(string message, int receiver)
        {
            string sql = "INSERT INTO private_messages(sent_to_uid, sent_by_uid, message_text, date_sent) " +
                         "VALUE( " +
                         $"{receiver}, " +
                         $"{clientUserId}, " +
                         $"'{message}', " +
                         "NOW() " +
                         ")";

            using (MySqlCommand sqlCommand = new MySqlCommand(sql, connection))
            {
                sqlCommand.ExecuteNonQuery();
            }
        }

        public static void SendGroupChatMessage(string message, int receiverGroupChatId)
        {
            string sql = "INSERT INTO group_chat_messages(gc_id, gcm_text, gcm_date_sent, gcm_sent_by_uid) " +
                         "VALUE( " +
                         $"{receiverGroupChatId}, " +
                         $"'{message}', " +
                         "NOW(), " +
                         $"{clientUserId} " +
                         "); ";

            using (MySqlCommand sqlCommand = new MySqlCommand(sql, connection))
            {
                sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Временной интервал сверивания данных с базой данных, заданный в секундах.
        /// </summary>
        static int timeIntervalInSecs = 1;

        /// <summary>
        /// Последнее время, когда приложение сверялось с базой данных
        /// </summary>
        static DateTime lastTimeUpdated;

        /// <summary>
        /// Сверяет данные с базой данных через заданный интервал времени
        /// </summary>
        private static void UpdateData(object sender, EventArgs eventArgs)
        {
            foreach (var updatable in updatables)
            {
                updatable.Update();
            }

            lastTimeUpdated = DateTime.Now;
        }
    }
}

