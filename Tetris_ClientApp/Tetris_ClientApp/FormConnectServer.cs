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
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Tetris_ClientApp
{
    public partial class FormConnectServer : Form
    {

        IPHostEntry localIP = Dns.Resolve(Dns.GetHostName());
        private Socket clientSocket;
        IPEndPoint ep;
        public FormConnectServer()
        {
            InitializeComponent();
            txtBoxServerIP.Text = localIP.AddressList[0].ToString();
            clientSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine(localIP);
            ep = new IPEndPoint(IPAddress.Parse(localIP.AddressList[0].ToString()), 2600);
            clientSocket.BeginConnect(ep, clientConnectedCallback, null);
        }

        private void btnGetCode_Click(object sender, EventArgs e)
        {
            String strGetCode = "giveMeCode";
            ReceiveBuffer receiveBuffer = new ReceiveBuffer();
            Send(strGetCode);
            receiveData();
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

        private void receiveData()
        {
            if (clientSocket.Connected)
            {
                ReceiveBuffer receiveBuffer = new ReceiveBuffer();
                clientSocket.BeginReceive(receiveBuffer.tempBuffer, 0, ReceiveBuffer.BufferSize, SocketFlags.None, receiveCallback, receiveBuffer);
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
                    //onClientDisconnected(e.Message);
                }
            }
        }
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
                //onConnectionRefused(e.Message);
            }

            if (clientSocket.Connected)
            {
                //receiveData();
                //onClientConnected(this);
            }
        }

        private void dataSendCallback(IAsyncResult ar)
        {
            if (clientSocket.Connected)
            {
                clientSocket.EndSend(ar);
                Console.WriteLine("DatasSend");
                receiveData();
                //onDataSent(this);
            }
            else
            {
                //onClientDisconnected("unable to send data : client disconnected");
            }
        }

        private void receiveCallback(IAsyncResult ar)
        {
            int dataReceivedSize = 0;
            try 
            {
                dataReceivedSize = clientSocket.EndReceive(ar);
            }
            catch (Exception e)
            {
                if (!clientSocket.Connected)
                {
                    //onClientDisconnected(e.Message);
                }
            }
            ReceiveBuffer receiveBuffer = (ReceiveBuffer)ar.AsyncState;

            if (dataReceivedSize > 0)
            {
                receiveBuffer.Append(dataReceivedSize);
                if (clientSocket.Available > 0)
                    clientSocket.BeginReceive(receiveBuffer.tempBuffer, 0, ReceiveBuffer.BufferSize, SocketFlags.None, receiveCallback, receiveBuffer);
                else
                {
                    object data = receiveBuffer.Deserialize();
                    //onDataReceived(data);
                    receiveData();
                }
            }
        }
    }
}
