using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_ServerApp
{
    
    class Channel
    {
        public int code;
        private int nPlayer;
        private const int maxPlayer = 2;
        public Channel(int code)
        {
            
        }

        public bool AddPlayer(Client player)
        {
            if (nPlayer <= maxPlayer)
            {
                nPlayer++;
                return true;
            }
            else
            {
                return false;
            }
            
        }

        Client player1 = new Client();
    }
}
