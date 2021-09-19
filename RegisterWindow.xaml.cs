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
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (PwdBox.Password != PwdRepeatBox.Password)
            {
                ErrorMsgTextBlock.Text = "Passwords don't match";
                return;
            }

            try
            {
                if (!DBHandler.TryRegister(LoginTextBox.Text, PwdBox.Password))
                    ErrorMsgTextBlock.Text = "Учётная запись с таким логином уже существует!";
                else
                {
                    new ChatListWindow().Show();
                    this.Close();
                }
            }
            catch (Exception excp)
            {
                ErrorMsgTextBlock.Text = excp.Message;
            }

        }
    }
}
