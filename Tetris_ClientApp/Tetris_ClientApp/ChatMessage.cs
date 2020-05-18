using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_ClientApp
{
    [Serializable]
    class ChatMessage
    {
        public string strMessage = "";
        public ChatMessage(string strText)
        {
            strMessage = strText;
        }
    }
}
