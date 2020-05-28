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
    /*
     * Le serveur est démarré par n'importe quel client, voire même par une personne externe. La méthode de fonctionnement était à l'origine faite pour fonctionner avec un serveur distant
     * à une IP fixe mais cela ne s'est pas fait pour des raisons de facilité de correction de l'application.
     * Le client demande au serveur un code, le serveur lui envoie. Quand le client se connecte à un channel de jeux avec un code, le serveur crée ce channel si il n'existe pas
     * Si le channel existe déjà et qu'il n'est pas complet, on ajoute le client au channel et les informations passent alors de client en client au travers de la classe channel.
     * **/
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
            //Bouton qui allume ou éteint le serveur
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
                //Envoyer un code d'accès à un channel
                if ((String)data == "giveMeCode")
                {
                    int chanel;
                    bool isValid = false;
                    Random random = new Random();
                    chanel = random.Next(1, 9999);
                    //Envoyer le code d'un channel qui n'est pas encore créé
                    while(isValid == false)
                    {
                        isValid = true;
                        for (int i = 0; i < PlayingChannels.Count; i++)
                        {
                            if (chanel == PlayingChannels[i].code)
                            {
                                isValid = false;
                            }
                        }
                    }
                    remoteClients[remoteClients.IndexOf(client)].Send(chanel.ToString());
                }
                
            }//Si tableau de byte, c'est un numéro de channel
            else if (data is byte[])
            {
                String stCode = Encoding.ASCII.GetString((byte[])data);
                //Récupère le channel
                for (int i = 0; i < PlayingChannels.Count; i++)
                {
                    //Si le code lu correspont à un channel ?
                    if (PlayingChannels[i].code == Int32.Parse(stCode))
                    {
                        //Si oui, on ajoute le joueur dans le channel existant
                        if (PlayingChannels[i].AddPlayer(remoteClients[remoteClients.IndexOf(client)]))
                        {
                            //On retire la callback de remoteclient qui est appelée quand on recoit une info parce qu'on va l'ajouter dans la classe channel,
                            //pour gérer les infos seulement entre les deux joueurs
                            remoteClients[remoteClients.IndexOf(client)].DataReceived -= RemoteClient_DataReceived;
                            monitorServerMessages.AddMessage(client.ClientSocket.RemoteEndPoint + " join the channel " + stCode );
                        }
                        return;
                    }
                }
                //Si on arrive ici, c'est qu'on est passé par le return et donc on a pas trouvé de channel existant correspondant au code envoyé par le client
                //On crée donc le channel voulu et on y ajoute le client voulu. Ensuite, on ajoute le channel créé à la liste de channels 
                Channel chan = new Channel(Int32.Parse(stCode));
                chan.AddPlayer(client);
                PlayingChannels.Add(chan);
                //Console management
                monitorServerMessages.AddMessage("Channel " + stCode + "create");
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

        private void TetrisServerApp_FormClosing(object sender, FormClosingEventArgs e)
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
