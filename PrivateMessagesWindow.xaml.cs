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
    /// Логика взаимодействия для PrivateMessagesWindow.xaml
    /// </summary>
    public partial class PrivateMessagesWindow : Window, IUpdatable
    {
        private readonly User interlocutor;

        public PrivateMessagesWindow(User interlocutor)
        {
            InitializeComponent();

            this.interlocutor = interlocutor;

            interlocutor.Update();

            foreach (var message in interlocutor.PrivateMessages)
            {
                PrivateMessageUserControl messageUserControl;
                if (message.SenderId == DBHandler.ClientUserId)
                    messageUserControl = new PrivateMessageUserControl(message.Text, DBHandler.Login);
                else
                    messageUserControl = new PrivateMessageUserControl(message.Text, interlocutor.Name);
                PrivateMessagesListBox.Items.Add(messageUserControl);
            }
        }

        public void Update()
        {
            interlocutor.Update();
            PrivateMessagesListBox.Items.Clear();
            foreach (var message in interlocutor.PrivateMessages)
            {
                PrivateMessageUserControl messageUserControl;
                if (message.SenderId == DBHandler.ClientUserId)
                    messageUserControl = new PrivateMessageUserControl(message.Text, DBHandler.Login);
                else
                    messageUserControl = new PrivateMessageUserControl(message.Text, interlocutor.Name);
                PrivateMessagesListBox.Items.Add(messageUserControl);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new ChatListWindow().Show();
            Close();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            DBHandler.SendPrivateMessage(MessageTextBox.Text, interlocutor.Id);
            Update();
        }

    }
}
