using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_ClientApp
{
    /*
     * Contient juste un string, permet de différencier les infos à la réception
     */
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
