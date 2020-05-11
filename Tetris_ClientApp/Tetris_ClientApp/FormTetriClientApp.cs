using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris_ClientApp
{
    public partial class formTetrisClientApp : Form
    {
        Client remoteServer;
        TetrisGrid gridPlayerMe;
        TetrisGrid gridPlayerRival;
        delegate void PrintHandler(Label label, string msgToPrint);
        delegate void TimerHandler();
        public formTetrisClientApp()
        {
            InitializeComponent();

            FormConnectServer serverConnection = new FormConnectServer();


            gridPlayerMe = new TetrisGrid(300, 600, 20, 10);
            gridPlayerRival = new TetrisGrid(300, 600, 20, 10);

            gridPlayerMe.Location = new Point(100, this.Height - gridPlayerMe.Height - 100);
            gridPlayerRival.Location = new Point(600, this.Height - gridPlayerRival.Height - 100);

            this.Controls.Add(gridPlayerMe);
            this.Controls.Add(gridPlayerRival);

            serverConnection.ShowDialog();
            if (serverConnection.DialogResult == DialogResult.OK)
            {
                remoteServer = serverConnection.remoteServer;
                remoteServer.ClientConnected += RemoteServer_ClientConnected;
                remoteServer.DataReceived += RemoteServer_DataReceived;
                remoteServer.ClientDisconnected += RemoteServer_ClientDisconnected;
                remoteServer.ConnectionRefused += RemoteServer_ConnectionRefused;

                byte[] codeChannel = Encoding.ASCII.GetBytes(serverConnection.stCode);
                remoteServer.Send(codeChannel);

                gridPlayerMe.FigureMovedDown += onFigureMovedDown;
                gridPlayerMe.ScoreChanged += onScoreChanged;
                gridPlayerMe.GameOver += onGameOver;
            }
            else if(serverConnection.DialogResult == DialogResult.Cancel)
            {
                Console.WriteLine("ouioui");
                //TODO : fermer le programme
            }
        }

        private void onScoreChanged(object sender, EventArgs e)
        {
            if (sender is TetrisGrid)
            {

                PrintHandler p = new PrintHandler(printScore);

                String strScore = "";
                if ((TetrisGrid)sender == gridPlayerMe)
                {
                    strScore = "score : " + gridPlayerMe.score.ToString();
                    this.Invoke(p, lblScoreMe, strScore);
                }
                else if ((TetrisGrid)sender == gridPlayerRival)
                {
                    strScore = "score : " + gridPlayerRival.score.ToString();
                    this.Invoke(p, lblScoreRival, strScore);
                }
            }
        }

        private void onGameOver(object sender, EventArgs e)
        {
            Console.WriteLine("GameOver");
            remoteServer.Send("gameOver");
        }

        private void printScore(Label label, string msgToPrint)
        {
            label.Text = msgToPrint;
        }

        private void onFigureMovedDown(object sender, EventArgs e)
        {
            if (remoteServer != null)
                if (remoteServer.ClientSocket.Connected)
                {
                    TetrisClientInfo info = new TetrisClientInfo(gridPlayerMe);
                    remoteServer.Send(info);
                }
        }

        private void btnAbandonner_Click(object sender, EventArgs e)
        {
            //gridPlayerMe.start();
            remoteServer.ready = true;
            remoteServer.Send("ready");
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Down)
            {
                gridPlayerMe.drop();
            }
            if (keyData == Keys.Up)
            {
                gridPlayerMe.rotate();
            }
            if (keyData == Keys.Left)
            {
                gridPlayerMe.moveLeft();
            }
            if (keyData == Keys.Right)
            {
                gridPlayerMe.moveRight();
            }
            return base.ProcessCmdKey(ref msg, keyData);
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
                if ((String)data == "start")
                {
                    TimerHandler p = new TimerHandler(startTimerGridMe);
                    this.Invoke(p);
                }
                else if ((String)data == "gameOver")
                {
                    Console.WriteLine("Adversaire game over");
                    TimerHandler p = new TimerHandler(StopTimerGridMe);
                    this.Invoke(p);
                }
            }
            else
            {
                TetrisClientInfo info = (TetrisClientInfo)data;
                 
                int rows = gridPlayerRival.numLines;
                int cols = gridPlayerRival.numCols;

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        gridPlayerRival.labelsBlock[i, j].BackColor = info.tbColors[i,j];
                    }
                }

                gridPlayerRival.score = info.score;
                onScoreChanged(gridPlayerRival, EventArgs.Empty);



            }
        }

        private void StopTimerGridMe()
        {
            gridPlayerMe.stop();
        }

        private void startTimerGridMe()
        {
            gridPlayerMe.start();
        }

        private void RemoteServer_ClientConnected(Client client)
        {
        }
        private void RemoteServer_ClientDisconnected(Client client, string message)
        {
            MessageBox.Show("You have been disconnected ! Window will now close.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            this.Close();
        }
    }
}
