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
    /// Логика взаимодействия для GroupChatPreview.xaml
    /// </summary>
    public partial class GroupChatPreview : UserControl
    {
        private GroupChat groupChat;
        public GroupChat GroupChat => groupChat;

        public GroupChatPreview(GroupChat groupChat)
        {
            InitializeComponent();

            this.groupChat = groupChat;
            GroupChatNameLabel.Content = groupChat.Name;
        }
    }
}
