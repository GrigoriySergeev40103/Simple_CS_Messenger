using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CSMessanger
{
    /// <summary>
    /// Логика взаимодействия для GroupChatMessagesWindow.xaml
    /// </summary>
    public partial class GroupChatMessagesWindow : Window, IUpdatable
    {
        GroupChat groupChat;
        public GroupChatMessagesWindow(GroupChat groupChat)
        {
            InitializeComponent();

            this.groupChat = groupChat;

            GroupChatNameLabel.Content = groupChat.Name;

            BackButton.Click += BackButton_MouseLeftButtonUp;
            SendButton.Click += SendButton_MouseLeftButtonUp;

            if(groupChat.Messages != null)
            {

                foreach (var message in groupChat.Messages)
                {
                    MessagesListBox.Items.Add(new GroupChatMessageUserControl(message.Text, message.SentBy.Name));
                }
            }
        }

        public void Update()
        {
            MessagesListBox.Items.Clear();

            groupChat.Update();

            if (groupChat.Messages != null)
            {

                foreach (var message in groupChat.Messages)
                {
                    MessagesListBox.Items.Add(new GroupChatMessageUserControl(message.Text, message.SentBy.Name));
                }
            }
        }

        private void BackButton_MouseLeftButtonUp(object sender, EventArgs e)
        {
            new ChatListWindow().Show();
            Close();
        }

        private void SendButton_MouseLeftButtonUp(object sender, EventArgs e)
        {
            DBHandler.SendGroupChatMessage(MessageTextBox.Text, groupChat.id);
            Update();
        }
    }
}
