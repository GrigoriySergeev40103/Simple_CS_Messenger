using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CSMessanger
{
    public class GroupChat : IEquatable<GroupChat>, IUpdatable
    {
        public readonly int id;
        private string name;
        private List<User> members;
        private List<GroupChatMessage> messages;
        public string Name => name;
        public List<GroupChatMessage> Messages => messages;

        public GroupChat(int groupChatId, string groupChatName)
        {
            id = groupChatId;
            name = groupChatName;
        }

        public bool Equals(GroupChat other)
        {
            if (id == other.id)
                return true;
            else
                return false;
        }

        public void Update()
        {
            members = new List<User>();
            messages = new List<GroupChatMessage>();
            GetGroupChatMembersBasicInfo();
            GetAllMessagesInGroupChat();
        }


        /// <summary>
        /// Загружает id и имена участников заданной группы.
        /// </summary>
        /// <param name="groupChat"></param>
        private void GetGroupChatMembersBasicInfo()
        {
            // Получение списка участников
            string sql = "SELECT user_data.id , login " +
                         "FROM group_chat_members " +
                         "JOIN user_data ON group_chat_members.uid = user_data.id " +
                         $"WHERE group_chat_members.gc_id = {id}";

            using (MySqlCommand sqlCommand = new MySqlCommand(sql, DBHandler.Connection))
            {
                using (MySqlDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        members.Add(DBHandler.TryAddUser(new User(dataReader.GetInt32(0), dataReader.GetString(1))));
                    }

                    dataReader.Close();
                }
            }
        }

        /// <summary>
        /// Загружает все сообщения в заданном групповом чате.
        /// </summary>
        /// <param name="groupChat"></param>
        private void GetAllMessagesInGroupChat()
        {
            string sql = "SELECT gcm_id, gcm_text, gcm_date_sent, gcm_sent_by_uid " +
                         "FROM messanger_data.group_chat_messages " +
                         $"WHERE gc_id = {id}";

            using (MySqlCommand sqlCommand = new MySqlCommand(sql, DBHandler.Connection))
            {
                using (MySqlDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        int index = 0;
                        for(int i = 0; i < members.Count; i++)
                        {
                            if (members[i].Id == dataReader.GetInt32(3))
                            {
                                index = i;
                                break;
                            }
                        }    
                        messages.Add(new GroupChatMessage
                        (
                            id,
                            dataReader.GetInt32(0),
                            dataReader.GetString(1),
                            dataReader.GetDateTime(2),
                            members[index]
                        ));
                    }

                    dataReader.Close();
                }
            }
        }

    }
}
