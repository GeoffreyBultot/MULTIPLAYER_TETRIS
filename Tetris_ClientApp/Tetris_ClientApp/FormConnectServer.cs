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
        public Client remoteServer;
        public String stCode;
        
        IPHostEntry localIP = Dns.Resolve(Dns.GetHostName());
        
        delegate void PrintHandler(string msgToPrint);

        public FormConnectServer()
        {
            InitializeComponent();
            
            remoteServer = new Client(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp));

            remoteServer.ClientConnected += RemoteServer_ClientConnected;
            remoteServer.DataReceived += RemoteServer_DataReceived;
            remoteServer.ClientDisconnected += RemoteServer_ClientDisconnected;
            remoteServer.ConnectionRefused += RemoteServer_ConnectionRefused;
            txtBoxServerIP.Text = localIP.AddressList[0].ToString();//"192.168.0.6";//localIP.AddressList[0].ToString();
        }

        private void RemoteServer_ClientConnected(Client client)
        {
            Console.WriteLine("connected");
            btnConnect.Text = "Disconnect";
        }

        private void RemoteServer_ClientDisconnected(Client client, string message)
        {
            
            //MessageBox.Show("You have been disconnected ! Window will now close.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            btnConnect.Text = "Connect";
            //this.Close();
        }

        private void btnGetCode_Click(object sender, EventArgs e)
        {
            String strGetCode = "giveMeCode";
            
            remoteServer.Send(strGetCode);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            
            if (remoteServer.ClientSocket.Connected)
            {
                Console.WriteLine("go disc");
                remoteServer.Disconnect();
            }
            else
            {
                Console.WriteLine("go conn");
                remoteServer.ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); ;
                remoteServer.Connect(localIP.AddressList[0].ToString(), 2600);//"192.168.0.6", 2600);
            }
        }

        private void btnGoGame_Click(object sender, EventArgs e)
        {
            bool isNumeric = int.TryParse(txtBoxCode.Text, out int n);

            if (remoteServer.ClientSocket.Connected && isNumeric)
            {
                remoteServer.ClientConnected -= RemoteServer_ClientConnected;
                remoteServer.DataReceived -= RemoteServer_DataReceived;
                remoteServer.ClientDisconnected -= RemoteServer_ClientDisconnected;
                remoteServer.ConnectionRefused -= RemoteServer_ConnectionRefused;
                stCode = txtBoxCode.Text;
                DialogResult = DialogResult.OK;
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            remoteServer.Disconnect();
            this.Close();
        }

        private void RemoteServer_ConnectionRefused(Client client, string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.Close();
        }

        private void RemoteServer_DataReceived(Client client, object data)
        {
            if (data is String)
            {
                String test = (String)data;
                Console.WriteLine(test);
                PrintHandler p = new PrintHandler(printClientInfo);
                this.Invoke(p, test);
            }
        }
        
        private void printClientInfo(string msgToPrint)
        {
            lblCodeReceived.Text = "Code: " + msgToPrint;
            txtBoxCode.Text = msgToPrint;
            //throw new NotImplementedException();
        }
    }
}
