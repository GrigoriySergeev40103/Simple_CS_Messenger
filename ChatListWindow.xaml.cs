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
    /// Логика взаимодействия для ChatListWindow.xaml
    /// </summary>
    public partial class ChatListWindow : Window, IUpdatable
    {
        public ChatListWindow()
        {
            InitializeComponent();

            DBHandler.Updatables.Add(this);

            foreach (var interlocutor in DBHandler.Interlocutors)
            {
                var privateChatPreview = new PrivateChatPreview(interlocutor);

                ChatListBox.Items.Add(privateChatPreview);

                privateChatPreview.MouseLeftButtonUp += PrivateMessagePreview_Click;
            }

            foreach (var groupChat in DBHandler.GroupChats)
            {
                var privateChatPreview = new GroupChatPreview(groupChat);

                ChatListBox.Items.Add(privateChatPreview);

                privateChatPreview.MouseLeftButtonUp += GroupChatPreview_Click;
            }
        }

        private void PrivateMessagePreview_Click(object sender, EventArgs eventArgs)
        {
            new PrivateMessagesWindow((sender as PrivateChatPreview).Interlocutor).Show();
            Close();
        }
        private void GroupChatPreview_Click(object sender, EventArgs eventArgs)
        {
            new GroupChatMessagesWindow((sender as GroupChatPreview).GroupChat).Show();
            Close();
        }

        public void Update()
        {
            Dispatcher.Invoke(() =>
            {
                ChatListBox.Items.Clear();

                foreach (var interlocutor in DBHandler.Interlocutors)
                {
                    var privateChatPreview = new PrivateChatPreview(interlocutor);

                    ChatListBox.Items.Add(privateChatPreview);

                    privateChatPreview.MouseLeftButtonUp += PrivateMessagePreview_Click;
                }

                foreach (var groupChat in DBHandler.GroupChats)
                {
                    var privateChatPreview = new GroupChatPreview(groupChat);

                    ChatListBox.Items.Add(privateChatPreview);

                    privateChatPreview.MouseLeftButtonUp += GroupChatPreview_Click;
                }
            });
        }
    }
}
