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
    /// Логика взаимодействия для GroupChatMessageUserControl.xaml
    /// </summary>
    public partial class GroupChatMessageUserControl : UserControl
    {
        public GroupChatMessageUserControl(string text, string name)
        {
            InitializeComponent();

            MessageTextLabel.Content = text;
            SenderUsernameLabel.Content = name;
        }
    }
}
