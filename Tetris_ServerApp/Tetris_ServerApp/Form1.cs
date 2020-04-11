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
    public partial class Form1 : Form
    {
        IPHostEntry localIP = Dns.Resolve(Dns.GetHostName());
        Socket listener;
        IPEndPoint ep;
        
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
                Socket clientSocket = listener.EndAccept(ar);
                Console.WriteLine("CLIENT ACCEPTE");
                

                //AsyncClient client = new AsyncClient(clientSocket);
                //onClientAccepted(client);
                
                listener.BeginAccept(acceptClientCallback, null);
                byte[] buffer = new byte[256];
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, receiveCallback, buffer);

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

            int dataReceivedSize = 0;
            Console.WriteLine("Data received");
            try
            {
                dataReceivedSize = listener.EndReceive(ar);
            }
            catch (Exception e)
            {
                if (!listener.Connected)
                {
                    //onClientDisconnected(e.Message);
                }
            }
            byte[] receivedData = (byte[])ar.AsyncState;
            Console.WriteLine(ar.AsyncState.ToString());
            ReceiveBuffer receiveBuffer = (ReceiveBuffer)ar.AsyncState ;

            if (dataReceivedSize > 0)
            {
                receiveBuffer.Append(dataReceivedSize);
                if (listener.Available > 0)
                    listener.BeginReceive(receiveBuffer.tempBuffer, 0, ReceiveBuffer.BufferSize, SocketFlags.None, receiveCallback, receiveBuffer);
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
