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
        private Socket clientSocket;
        IPEndPoint ep;
        delegate void PrintHandler(string msgToPrint);
        
        public FormConnectServer()
        {
            InitializeComponent();

            remoteServer = new Client();

            remoteServer.ClientConnected += RemoteServer_ClientConnected;
            remoteServer.DataReceived += RemoteServer_DataReceived;
            remoteServer.ClientDisconnected += RemoteServer_ClientDisconnected;
            remoteServer.ConnectionRefused += RemoteServer_ConnectionRefused;

            txtBoxServerIP.Text = localIP.AddressList[0].ToString();
            clientSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            //Console.WriteLine(localIP.AddressList[0].ToString());

            //ep = new IPEndPoint(IPAddress.Parse(localIP.AddressList[0].ToString()), 2600);

            remoteServer.Connect(localIP.AddressList[0].ToString(), 2600);
        }

        private void btnGetCode_Click(object sender, EventArgs e)
        {
            String strGetCode = "giveMeCode";
            
            //byte[] buffer = serialize(Encoding.ASCII.GetBytes(strGetCode));
            
            //ReceiveBuffer receiveBuffer = new ReceiveBuffer();
            remoteServer.Send(strGetCode);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            remoteServer.ClientConnected -= RemoteServer_ClientConnected;
            remoteServer.DataReceived -= RemoteServer_DataReceived;
            remoteServer.ClientDisconnected -= RemoteServer_ClientDisconnected;
            remoteServer.ConnectionRefused -= RemoteServer_ConnectionRefused;
            stCode = textBox1.Text;
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            remoteServer.Disconnect();
            this.Close();
        }

        private void RemoteServer_ConnectionRefused(Client client, string message)
        {
            /* Cette fonction sera exécutée si on essaie de se connecter au serveur alors qu'il est éteint.
             * Dans ce cas on affiche le message dans un MessageBox et on ferme la fenêtre.
             * L'objet MessageBox permet d'avoir une fenêtre de dialogue standard utilisées dans toutes
             * les applications windows pour informer l'utilisateur de l'application. On évite ainsi de devoir
             * créer une nouvelle WindowForm pour ce type de tâches très courantes.
             */

            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            this.Close();
        }

        private void RemoteServer_DataReceived(Client client, object data)
        {
            /* Lorsequ' on recoit des données du serveur, ces données contiennent les informations d'une forme (MovingShapeInfo).
             * On crée une nouvelle forme à partir de ses informations, et on veille à ce que sa position en X soit à 0 (à gauche).
             */
            //Console.WriteLine("c arrive");
            if (data is String)
            {
                String test = (String)data;
                Console.WriteLine(test);
                PrintHandler p = new PrintHandler(printClientInfo);
                this.Invoke(p, test);
            }
            else
            {
                //Console.WriteLine("other");

            }
        }
        
            

        private void printClientInfo(string msgToPrint)
        {
            lblCodeReceived.Text = "Code: " + msgToPrint;
            textBox1.Text = msgToPrint;
            //throw new NotImplementedException();
        }
    private void RemoteServer_ClientConnected(Client client)
        {
            //labelClientStatus.Text = "You are connected to " + client.ClientSocket.RemoteEndPoint;
        }

        private void RemoteServer_ClientDisconnected(Client client, string message)
        {
            MessageBox.Show("You have been disconnected ! Window will now close.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            this.Close();
        }

        
    }
}
