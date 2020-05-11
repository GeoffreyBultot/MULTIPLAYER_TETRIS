using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_ServerApp
{
    public class Client
    {
        public delegate void ClientConnectedHandler(Client client);
        public delegate void DataSendHandler(Client client);
        public delegate void DataReceivedHandler(Client client, object data);
        public delegate void ClientDisconnectedHandler(Client client, string message);
        public delegate void ConnectionRefusedHandler(Client client, string message);

        public event DataSendHandler DataSent;
        public event DataReceivedHandler DataReceived;
        public event ClientConnectedHandler ClientConnected;
        public event ClientDisconnectedHandler ClientDisconnected;
        public event ConnectionRefusedHandler ConnectionRefused;

        public Socket ClientSocket { get { return clientSocket; } set { clientSocket = value; } }

        private Socket clientSocket;

        public bool ready = false;

        #region Constructors
     
        public Client()
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public Client(Socket clientSocket)
        {
            this.clientSocket = clientSocket;
            receiveData();
        }

        public Client(Client client)
        {
            clientSocket = client.ClientSocket;
            receiveData();
        }
        #endregion

        public void Connect(string address, int port)
        {
            if (!clientSocket.Connected)
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(address), port);
                clientSocket.BeginConnect(ep, clientConnectedCallback, null);
            }
        }
        public void Disconnect()
        {
            if (clientSocket.Connected)
            {
                clientSocket.Close();
            }
        }

        public void Send(object data)
        {
            if (clientSocket.Connected)
            {
                /* Les données à envoyer doivent être transformés en tableau de byte (serialize).
                 */
                byte[] dataBuffer = serialize(data);

                try
                {
                    clientSocket.BeginSend(dataBuffer, 0, dataBuffer.Length, SocketFlags.None, dataSendCallback, null);
                }
                catch (SocketException e)
                {
                    onClientDisconnected(e.Message);
                }
            }
        }

        private void receiveData()
        {
            if (clientSocket.Connected)
            {
                /* On crée un ReceiveBuffer qui contient un buffer temporaire dans lequel EndReceive écrira les données reçues et
                 * un système de reconstitution des données reçues. Il est important d'envoyer cet objet au callback pour pouvoir
                 * réceptionner la totalité des données en cas de dépassement du buffer temporaire (plus d'informations -> ReceiveBuffer.cs).
                 */
                ReceiveBuffer receiveBuffer = new ReceiveBuffer();
                clientSocket.BeginReceive(receiveBuffer.tempBuffer, 0, ReceiveBuffer.BufferSize, SocketFlags.None, receiveCallback, receiveBuffer);
            }
        }

        private byte[] serialize(object data)
        {
            /* L'objet à envoyer doit être sérialisé avant de pouvoir être envoyé (plus d'informations -> ReceiveBuffer.cs).
             * On utilise ici un BinaryFormatter qui nécessite l'utilisation d'un memoryStream dans lequel seront écrites
             * les données sérialisées.
             */
            BinaryFormatter bin = new BinaryFormatter();
            MemoryStream mem = new MemoryStream();
            bin.Serialize(mem, data);
            byte[] buffer = mem.GetBuffer();
            mem.Close();
            return buffer;
        }

        #region Callbacks

        private void clientConnectedCallback(IAsyncResult ar)
        {
            /* On tente de se connecter au serveur. Si la connexion est établie, on démarre la réception de 
             * données et on déclenche l'event ClientConnected. Si la connexion est refusée ou prends trop
             * de temps, la méthode EndAccept lève un exception. On déclenche alors l'event ConnectionRefused
             * en transmettant le message de l'exception (contenant à priori la raison du problème).
             */
            try
            {
                clientSocket.EndConnect(ar);
            }
            catch (SocketException e)
            {
                onConnectionRefused(e.Message);
            }

            if (clientSocket.Connected)
            {
                receiveData();
                onClientConnected(this);
            }
        }

        private void dataSendCallback(IAsyncResult ar)
        {
            if (clientSocket.Connected)
            {
                clientSocket.EndSend(ar);
                onDataSent(this);
            }
            else
            {
                onClientDisconnected("unable to send data : client disconnected");
            }
        }

        private void receiveCallback(IAsyncResult ar)
        {
            /* La méthode EndReceive écrit les données reçues dans le buffer passé en paramètre de la méthode
             * BeginReceive et renvoie le nombre de bytes écrits. EndReceive est en attente de nouvelles données
             * provenant du client distant. Si la connexion est interrompue, une exception est levée et on déclenche
             * l'event ClientDisconnected.
             * On récupère l'objet receiveBuffer afin de pouvoir reconstituer les données reçue en une ou plusieurs
             * fois (plus d'explications dans le fichier ReceiveBuffer.cs)
             */
            int dataReceivedSize = 0;
            try
            {
                dataReceivedSize = clientSocket.EndReceive(ar);
            }
            catch (Exception e)
            {
                if (!clientSocket.Connected)
                {
                    onClientDisconnected(e.Message);
                }
            }
            Console.WriteLine(ar.AsyncState.ToString());
            ReceiveBuffer receiveBuffer = (ReceiveBuffer)ar.AsyncState;

            if (dataReceivedSize > 0)
            {
                /* Si des données ont été reçues, on les accumule dans le memoryStream, et si après réception il y en a encore,
                 * on on recommence la réception asynchrone en mettant le receiveBuffer en stateObject afin que ce soit toujours
                 * le même qui s'accumule.
                 * Si toutes les données ont été reçues (Available = false), on désérialise le buffer et on déclenche l'event
                 * DataReceived en mettant les données désérialisées en paramètre.
                 */
                receiveBuffer.Append(dataReceivedSize);
                if (clientSocket.Available > 0)
                    clientSocket.BeginReceive(receiveBuffer.tempBuffer, 0, ReceiveBuffer.BufferSize, SocketFlags.None, receiveCallback, receiveBuffer);
                else
                {
                    object data = receiveBuffer.Deserialize();
                    if (data != null)
                    {
                        onDataReceived(data);
                        
                    }
                    receiveData();
                }
            }
        }
        #endregion

        #region Raising event methods

        /* Pour les explications, voir AsyncServer.cs
         */

        private void onClientDisconnected(string message)
        {
            if (ClientDisconnected != null)
            {
                if (ClientDisconnected.Target is System.Windows.Forms.Control)
                {
                    ((System.Windows.Forms.Control)ClientDisconnected.Target).Invoke(ClientDisconnected, this, message);
                }
                else
                {
                    ClientDisconnected(this, message);
                }
            }
        }

        private void onConnectionRefused(string message)
        {
            if (ConnectionRefused.Target is System.Windows.Forms.Control)
            {
                ((System.Windows.Forms.Control)ConnectionRefused.Target).Invoke(ConnectionRefused, this, message);
            }
            else
            {
                ConnectionRefused(this, message);
            }
        }

        private void onDataReceived(object data)
        {
            if (DataReceived != null)
            {
                if (DataReceived.Target is System.Windows.Forms.Control)
                {
                    ((System.Windows.Forms.Control)DataReceived.Target).Invoke(DataReceived, this, data);
                }
                else
                {
                    DataReceived(this, data);
                }
            }
        }

        private void onClientConnected(Client asyncClient)
        {
            if (ClientConnected != null)
            {
                if (ClientConnected.Target is System.Windows.Forms.Control)
                {
                    ((System.Windows.Forms.Control)ClientConnected.Target).Invoke(ClientConnected, this);
                }
                else
                {
                    ClientConnected(this);
                }
            }
        }

        private void onDataSent(Client asyncClient)
        {
            if (DataSent != null)
            {
                if (DataSent.Target is System.Windows.Forms.Control)
                {
                    ((System.Windows.Forms.Control)DataSent.Target).Invoke(DataSent, this);
                }
                else
                {
                    DataSent(this);
                }
            }
        }
        #endregion


    }
}
