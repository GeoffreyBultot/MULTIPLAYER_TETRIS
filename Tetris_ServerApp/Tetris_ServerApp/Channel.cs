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
        public List<Client> remoteClients = new List<Client>();
        private bool inGame = false;
        public Channel()
        { 
        }
        public Channel(int code)
        {
            this.code = code;
        }

        public bool AddPlayer(Client player)
        {
            if (nPlayer <= maxPlayer)
            {
                remoteClients.Add(player);
                remoteClients[remoteClients.IndexOf(player)].DataReceived += RemoteClient_DataReceived;
                nPlayer++;
                return true;
            }
            else
            {
                return false;
            }
            
        }
        public bool start ()
        {
            if (this.inGame)
            {
                return false;
            }
            else
            { if (!(remoteClients.Count >= 1))
                {
                    return false;
                }
                else
                {
                    foreach (Client cl in remoteClients)
                    {
                        if (!(cl.ready))
                        {
                            return false;
                        }
                    }

                    foreach (Client cl in remoteClients)
                    {
                        cl.Send("start");
                        Console.WriteLine("JENVOIE START");
                    }
                    this.inGame = true;
                    return true;
                } 
            }
        }
        public void removeClient(Client player)
        {
            remoteClients.Remove(player);
        }

        #region Raising event methods

        private void RemoteClient_DataReceived(Client client, object data)
        {
            if (data is String)
            {
                Console.WriteLine(data);
                if ((String)data == "giveMeCode")
                {
                    int chanel;
                    Random random = new Random();
                    chanel = random.Next(1, 9999);

                    remoteClients[remoteClients.IndexOf(client)].Send(chanel.ToString());
                }
                else if ((String)data == "ready")
                {
                    remoteClients[remoteClients.IndexOf(client)].ready = true;
                    foreach (Client cl in this.remoteClients)
                    {
                        if (cl == client)
                        {
                            //Start if all client are ready
                            this.start();
                        }
                    }
                }
                else if((String)data == "gameOver")
                {
                    this.inGame = false;
                    int nextClientIndex = (remoteClients.IndexOf(client) + 1) % remoteClients.Count;
                    remoteClients[nextClientIndex].Send(data);
                    foreach(Client cl in remoteClients)
                    {
                        cl.ready = false;
                    }
                    

                }
            }
            else if (data is byte[])
            {
            }
            else
            {
                if (remoteClients.Count > 1)
                {
                    int nextClientIndex = (remoteClients.IndexOf(client) + 1) % remoteClients.Count;
                    remoteClients[nextClientIndex].Send(data);
                }
            }
            //monitorServerMessages.AddMessage("data sent from " + client.ClientSocket.RemoteEndPoint + " to " + remoteClients[nextClientIndex].ClientSocket.RemoteEndPoint);
        }
        #endregion

    }
}