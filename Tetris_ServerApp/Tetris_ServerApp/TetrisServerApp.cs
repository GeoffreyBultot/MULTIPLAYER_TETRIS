using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
namespace Tetris_ServerApp
{
    //remoteClients
    public partial class TetrisServerApp : Form
    {
        IPHostEntry localIP = Dns.Resolve(Dns.GetHostName());
        
        Server server;
        List<Client> remoteClients = new List<Client>();
        List<Channel> PlayingChannels = new List<Channel>();
        public TetrisServerApp()
        {
            InitializeComponent();

            txtBoxIP.Text = localIP.AddressList[0].ToString();//"192.168.0.6";//localIP.AddressList[0].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(server == null)
            {
                server = new Server(txtBoxIP.Text, 2600);
                server.ServerStarted += Server_ServerStarted;
                server.ServerStopped += Server_ServerStopped;
                server.ClientAccepted += Server_ClientAccepted;
            }
            if (server.Running == true)
            {
                server.Stop();
            }
            else
            {
                server = new Server(txtBoxIP.Text, 2600);
                server.ServerStarted += Server_ServerStarted;
                server.ServerStopped += Server_ServerStopped;
                server.ClientAccepted += Server_ClientAccepted;
                server.Start();
            }
        }
        private void Server_ClientAccepted(Client client)
        {
            Client remoteClient = client;
            remoteClient.DataReceived += RemoteClient_DataReceived;
            remoteClient.ClientDisconnected += RemoteClient_ClientDisconnected;
            remoteClients.Add(client);
            updateClientCount();
            addClientToListBox(client);
            monitorServerMessages.AddMessage("Client Connected -> " + client.ClientSocket.RemoteEndPoint);
        }

        private void addClientToListBox(Client client)
        {
            listBoxConnectedClients.Items.Add(client.ClientSocket.RemoteEndPoint + " -> " + DateTime.Now.ToLongTimeString());
        }
        private void removeClientFromListBox(Client client)
        {
            int clientToRemoveIndex = listBoxConnectedClients.FindString(client.ClientSocket.RemoteEndPoint.ToString());
            listBoxConnectedClients.Items.RemoveAt(clientToRemoveIndex);
        }

        private void updateClientCount()
        {
            //labelNumberConnectedClients.Text = remoteClients.Count.ToString();
        }

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
                
            }
            else if (data is byte[])
            {
                String stCode = Encoding.ASCII.GetString((byte[])data);
                
                for (int i = 0; i < PlayingChannels.Count; i++)
                {
                    if (PlayingChannels[i].code == Int32.Parse(stCode))
                    {
                        
                        if (PlayingChannels[i].AddPlayer(remoteClients[remoteClients.IndexOf(client)]))
                        {
                            remoteClients[remoteClients.IndexOf(client)].DataReceived -= RemoteClient_DataReceived;
                            monitorServerMessages.AddMessage(client.ClientSocket.RemoteEndPoint + " join the channel " + stCode );
                        }
                        return;
                    }
                }
                Channel chan = new Channel(Int32.Parse(stCode));
                chan.AddPlayer(client);
                PlayingChannels.Add(chan);
                monitorServerMessages.AddMessage(client.ClientSocket.RemoteEndPoint + " join the channel " + stCode);
            }
            else
            {
                /*int nextClientIndex = (remoteClients.IndexOf(client) + 1) % remoteClients.Count;
                remoteClients[nextClientIndex].Send(data);
                */
                //remoteClients[remoteClients.IndexOf(client)].Send(data);
                //TODO: envoyer sur le bon channel
            }
        }
        private void RemoteClient_ClientDisconnected(Client client, string message)
        {
            remoteClients.Remove(client);
            //PlayingChannels[]

            for (int i = 0; i < PlayingChannels.Count; i++)
            {
                for (int j = 0; j < PlayingChannels[i].remoteClients.Count; j++)
                //foreach (Client cl in PlayingChannels[i].remoteClients)
                {
                    if (PlayingChannels[i].remoteClients[j] == client)
                        PlayingChannels[i].removeClient(client);
                }
                
            }
            removeClientFromListBox(client);
            updateClientCount();
            monitorServerMessages.AddMessage("Client Disconnected -> " + client.ClientSocket.RemoteEndPoint);
        }

        private void Server_ServerStopped()
        {
            btnStartServer.BackgroundImage = new Bitmap(Application.StartupPath + "\\..\\..\\Datas\\IMG\\start_logo.png");
            //btnStartServer.Text = "Start Server";
            monitorServerMessages.AddMessage("===SERVER STOPPED===");
            labelServerStatus.Text = "Server Stopped";
        }

        private void Server_ServerStarted()
        {
            btnStartServer.BackgroundImage = new Bitmap(Application.StartupPath + "\\..\\..\\Datas\\IMG\\stop_logo.png");
            //btnStartServer.Text = "Stop Server";
            monitorServerMessages.AddMessage("Server listening on : " + server.listenSocket.LocalEndPoint);
            labelServerStatus.Text = "Server listening on : " + server.listenSocket.LocalEndPoint;
        }

        private void AroundTheWorldServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            /* Pour les explications voir la fonction AroundTheWorldClient_FormClosing de la classe AroundTheWorldClient 
             */
            foreach (Client client in remoteClients)
            {
                client.ClientDisconnected -= RemoteClient_ClientDisconnected;
                client.Disconnect();
            }
            server.ServerStopped -= Server_ServerStopped;
            server.Stop();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
