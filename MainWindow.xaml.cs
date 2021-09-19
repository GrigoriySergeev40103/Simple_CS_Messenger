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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CSMessanger
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                DBHandler.Connect();
            }
            catch (Exception e)
            {
                ErrorMsgTextBlock.Text = e.Message;
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Open register window
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            // Send login info to server to check if exists
            try
            {
                if (DBHandler.TryLogIn(LoginTextBox.Text, PwdBox.Password))
                {
                    DBHandler.GetAllInterlocutors();
                    DBHandler.GetAllGroupChatsBasicInfo();
                    // If exists show chat list
                    ChatListWindow chatListWindow = new ChatListWindow();
                    chatListWindow.Show();
                    this.Close();
                }
            }
            catch (Exception exception)
            {
                ErrorMsgTextBlock.Text = exception.Message;
                return;
            }

        }
    }
}
