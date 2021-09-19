using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMessanger
{
    public class PrivateMessage
    {
        private int mId;
        private string text;
        private DateTime dateSent;
        private int senderId;

        public string Text => text;
        public int SenderId => senderId;

        public PrivateMessage(int messageId, string text, DateTime dateSent, int sentById)
        {
            mId = messageId;
            this.text = text;
            this.dateSent = dateSent;
            senderId = sentById;
        }
    }
}
