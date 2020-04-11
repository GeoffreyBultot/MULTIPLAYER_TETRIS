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
    public partial class Form1 : Form
    {
        IPHostEntry localIP = Dns.Resolve(Dns.GetHostName());
        Socket listener;
        Socket remoteClient;
        IPEndPoint ep;
        List<Client> remoteClients = new List<Client>();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = localIP.AddressList[0].ToString();
            ep = new IPEndPoint(IPAddress.Parse(localIP.AddressList[0].ToString()), 2600);
            listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(ep);
            listener.Listen(0);
            acceptClients();
            Console.WriteLine("ServerStart");
            //onServerStarted();
        }

        public void Stop()
        {
            listener.Close();
        }

        private void acceptClients()
        {
            listener.BeginAccept(acceptClientCallback, null);
        }
        private void receiveData()
        {
            if (listener.Connected)
            {
                
                ReceiveBuffer receiveBuffer = new ReceiveBuffer();
                listener.BeginReceive(receiveBuffer.tempBuffer, 0, ReceiveBuffer.BufferSize, SocketFlags.None, receiveCallback, receiveBuffer);
            }
        }
        private void acceptClientCallback(IAsyncResult ar)
        {
            try
            {
                remoteClient = listener.EndAccept(ar);
                Console.WriteLine("CLIENT ACCEPTE");                
                listener.BeginAccept(acceptClientCallback, null);
                byte[] buffer = new byte[256];
                remoteClient.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, receiveCallback, buffer);

                //receiveData();
            }
            catch (Exception e)
            {
                /* Lorse que le serveur se coupe (listener.close() ou autres soucis...) la méthode EndAccept se
                 * termine prématurément et une exeption est levée. On considère alors que le serveur est éteint
                 * et on déclenche l'event correspondant via la méthode onServerStopped.
                 */
                //onServerStopped();
            }
        }

        private void dataSendCallback(IAsyncResult ar)
        {
            if (listener.Connected)
            {
                listener.EndSend(ar);
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
            int bufferSize = remoteClient.EndReceive(ar);
            String strRep = "";
            String strCode = "333";
            byte[] receivedData = (byte[])ar.AsyncState;
            strRep = Encoding.ASCII.GetString(receivedData, 0, bufferSize);
            Console.WriteLine(strRep);

            byte[] Code = Encoding.ASCII.GetBytes(strCode);

            if ( strRep == "giveMeCode")
                remoteClient.BeginSend(Code, 0, Code.Length, SocketFlags.None, transmitClientCallback, Code);
            
        }

        private void transmitClientCallback(IAsyncResult ar)
        {
            //throw new NotImplementedException();
        }
    }
}
