using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CSMessanger
{
    public class User : IUpdatable, IEquatable<User>
    {
        private readonly int id;
        private string name;
        private List<PrivateMessage> privateMessages = new List<PrivateMessage>();

        public string Name => name;
        public int Id => id;
        public List<PrivateMessage> PrivateMessages => privateMessages;

        public User(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public void Update()
        {
            privateMessages = new List<PrivateMessage>();
            GetAllPrivateMessages();
        }

        /// <summary>
        /// Загружает все личные сообщения, полученные пользователем от этого пользователя.
        /// </summary>
        private void GetAllPrivateMessages()
        {
            string sql = "SELECT mid, message_text, date_sent, sent_by_uid " +
                         "FROM private_messages " +
                         "JOIN user_data ON private_messages.sent_by_uid = user_data.id " +
                         $"WHERE(sent_to_uid = {DBHandler.ClientUserId} AND sent_by_uid = {id}) " +
                         $"OR(sent_to_uid = {id} AND sent_by_uid = {DBHandler.ClientUserId})";

            using (MySqlCommand sqlCommand = new MySqlCommand(sql, DBHandler.Connection))
            {
                using (MySqlDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        PrivateMessage privateMessage = new PrivateMessage
                        (
                            dataReader.GetInt32(0),
                            dataReader.GetString(1),
                            dataReader.GetDateTime(2),
                            dataReader.GetInt32(3)
                        );

                        privateMessages.Add(privateMessage);
                    }

                    dataReader.Close();
                }
            }
        }

        public bool Equals(User other)
        {
            if (id == other.id)
                return true;
            else
                return false;
        }
    }
}
