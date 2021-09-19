using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMessanger
{
    public class GroupChatMessage
    {
        private readonly int id;
        private readonly int gcId;
        private string text;
        private DateTime dateSent;
        private User sentBy;

        public string Text => text;
        public User SentBy => sentBy;

        public GroupChatMessage(int groupChatMessageId, int groupChatId, string messageText, DateTime dateSent, User sentBy)
        {
            id = groupChatMessageId;
            gcId = groupChatId;
            text = messageText;
            this.dateSent = dateSent;
            this.sentBy = sentBy;
        }
    }
}
