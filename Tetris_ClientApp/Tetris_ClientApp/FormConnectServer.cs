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
        delegate void PrintHandler(string msgToPrint);
        public FormConnectServer()
        {
            InitializeComponent();
            txtBoxServerIP.Text = localIP.AddressList[0].ToString();
            clientSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine(localIP.AddressList[0].ToString());

            ep = new IPEndPoint(IPAddress.Parse(localIP.AddressList[0].ToString()), 2600);
            clientSocket.BeginConnect(ep, clientConnectedCallback, null);
        }

        private void btnGetCode_Click(object sender, EventArgs e)
        {
            String strGetCode = "giveMeCode";
            byte[] buffer = Encoding.ASCII.GetBytes(strGetCode);
            //ReceiveBuffer receiveBuffer = new ReceiveBuffer();
            Send(buffer);
            receiveData();
        }
        byte[] receiveBuffer = new byte[4096];
        private void receiveData()
        {
            if (clientSocket.Connected)
            {
                
                clientSocket.BeginReceive(receiveBuffer, 0, 4096, SocketFlags.None, receiveCallback, receiveBuffer);
            }
        }

        public void Send(object data)
        {
            if (clientSocket.Connected)
            {
                /* Les données à envoyer doivent être transformés en tableau de byte (serialize).
                 */

                byte[] dataBuffer = (byte[])data;

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
            int bufferSize = clientSocket.EndReceive(ar);
            String strRep = "";
            byte[] receivedData = (byte[])ar.AsyncState;
            strRep = Encoding.ASCII.GetString(receivedData, 0, bufferSize);
            Console.WriteLine(strRep);
            PrintHandler p = new PrintHandler(printClientInfo);
            this.Invoke(p, strRep);

        }

        private void printClientInfo(string msgToPrint)
        {
            lblCodeReceived.Text = "Code:"+msgToPrint;
            textBox1.Text = msgToPrint;
            //throw new NotImplementedException();
        }
    }
}
