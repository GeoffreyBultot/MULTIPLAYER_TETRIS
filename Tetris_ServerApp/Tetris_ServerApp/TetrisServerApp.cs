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
        //Socket listener;
        //Socket remoteClient;
        //IPEndPoint ep;
        
        Server server;
        List<Client> remoteClients = new List<Client>();
        List<Channel> PlayingChannels = new List<Channel>();
        public TetrisServerApp()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = localIP.AddressList[0].ToString();
            server = new Server(localIP.AddressList[0].ToString(), 2600);
            server.ServerStarted += Server_ServerStarted;
            server.ServerStopped += Server_ServerStopped;
            server.ClientAccepted += Server_ClientAccepted;
            server.Start();
        }
        private void Server_ClientAccepted(Client client)
        {
            /* Lorse qu'un client se connecte au serveur, on s'abonne au events et on garde une référence dans la liste remoteClients.
             * Ensuite on met à jour le nombre de clients connectés, on l'affiche dans la listBoxConnectedClients (addClientToListBox)
             * et on affiche un message dans la zone de monitoring.
             */
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
            /* En plus de l'adresse IP et le numéro de port, on affiche également l'heure de connexion.
             */
            listBoxConnectedClients.Items.Add(client.ClientSocket.RemoteEndPoint + " -> " + DateTime.Now.ToLongTimeString());
        }
        private void removeClientFromListBox(Client client)
        {
            /* La listBoxConnectedClients ne contient pas à proprement parlé les références vers les clients connectés,
             * mais uniquement une liste de string qui représente les clients. Pour supprimer un client de cette liste,
             * il faut trouver l'indice du string correspondant au client à supprimer. On retourve cet indice en cherchant
             * le string qui commence par l'adresse ip et le numéro de port du client à supprimer. 
             */
            int clientToRemoveIndex = listBoxConnectedClients.FindString(client.ClientSocket.RemoteEndPoint.ToString());
            listBoxConnectedClients.Items.RemoveAt(clientToRemoveIndex);
        }

        private void updateClientCount()
        {
            //labelNumberConnectedClients.Text = remoteClients.Count.ToString();
        }

        private void RemoteClient_DataReceived(Client client, object data)
        {
            /* Lorse que le serveur reçoit de données, celui doit les transmettre au prochain client. L'ordre des clients dans la liste
             * remoteClients correspond à l'ordre par lesquelles doivent passer chaque forme. On pourrait donc tout simplement envoyer
             * les données au client ayant l'indice du client d'ou provient les données (client source) + 1. Ceci pose problème lorse 
             * que le client source est le dernier de la liste. Dans ce cas il faut transmettre les données au premier client de la liste
             * (la forme vient de faire le tour du monde...), sinon une erreur "index out of bounds" est levée.
             * Une manière élégante de calculer cet indice consiste à prendre comme indice le reste de la division entière de l'indice du 
             * client source +1 par le nombre de clients connectés.
             */
            //int nextClientIndex = (remoteClients.IndexOf(client) + 1) % remoteClients.Count;
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
                    foreach(Channel ch in PlayingChannels)
                    //for (int i = 0; i < PlayingChannels.Count; i++)
                    {
                        foreach (Client cl in ch.remoteClients)
                        {
                            if (cl == client)
                            {
                                //Start if all client are ready
                                ch.start();
                            }
                        }
                    }
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

                            monitorServerMessages.AddMessage(client.ClientSocket.RemoteEndPoint + "Join the channel " + stCode );
                        }
                        return;
                    }
                }
                Channel chan = new Channel(Int32.Parse(stCode));
                chan.AddPlayer(client);
                PlayingChannels.Add(chan);
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
            btnStartServer.Text = "Start Server";
            monitorServerMessages.AddMessage("===SERVER STOPPED===");
            labelServerStatus.Text = "Server Stopped";
        }

        private void Server_ServerStarted()
        {
            btnStartServer.Text = "Stop Server";
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
    }
}
