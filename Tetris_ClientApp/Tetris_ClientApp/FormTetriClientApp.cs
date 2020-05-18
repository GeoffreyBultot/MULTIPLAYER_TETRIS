using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris_ClientApp
{
    public partial class formTetrisClientApp : Form
    {
        Client remoteServer;
        delegate void PrintHandler(Label label, string msgToPrint);
        delegate void PrintMessageHandler(TextBox txtbox, string msgToPrint);
        delegate void RivalGameOverHandler();
        delegate void TimerHandler();
        PrivateFontCollection pfc = new PrivateFontCollection();
            
        public formTetrisClientApp()
        {
            InitializeComponent();
            

            FormConnectServer serverConnection = new FormConnectServer();

            InitGraphics();
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

                tetrisGrid2.FigureMovedDown += onFigureMovedDown;
                tetrisGrid2.ScoreChanged += onScoreChanged;
                tetrisGrid2.GameOver += onGameOver;
            }
            else //if(serverConnection.DialogResult == DialogResult.Cancel)
            {
                Load += (s, e) => Close();
            }
        }


        void InitGraphics()
        {
            Console.WriteLine(Application.StartupPath);
            pfc.AddFontFile(Application.StartupPath+"\\..\\..\\Datas\\7_Segment.ttf");
            Font labelTetrisFont = new Font(pfc.Families[0], 26, FontStyle.Bold);
            lblScoreMe.Font = labelTetrisFont;
            lblScoreRival.Font = labelTetrisFont;
            lblRival.Font = labelTetrisFont;
            lblYOU.Font = labelTetrisFont;

        }

        private void onScoreChanged(object sender, EventArgs e)
        {
            if (sender is TetrisGrid)
            {

                PrintHandler p = new PrintHandler(printScore);

                String strScore = "";
                if ((TetrisGrid)sender == tetrisGrid2)
                {
                    strScore = "score " + tetrisGrid2.score.ToString();
                    this.Invoke(p, lblScoreMe, strScore);
                }
                else if ((TetrisGrid)sender == tetrisGridd)
                {
                    strScore = "score " + tetrisGridd.score.ToString();
                    this.Invoke(p, lblScoreRival, strScore);
                }
            }
        }

        /**Appelé quand Me a perdu*/
        private void onGameOver(object sender, EventArgs e)
        {
            Console.WriteLine("GameOver");
            remoteServer.Send("gameOver");

            tetrisGrid2.asyncstopGrid();
            String strMessage = "";
            String strCaption = "GAME OVER";
            if (tetrisGrid2.score > tetrisGridd.score)
            {
                strMessage = "YOU WIN \n";
            }
            else if (tetrisGrid2.score == tetrisGridd.score)
            {
                strMessage = "YOU LOSE \n";
            }
            else
            {
                strMessage = "YOU LOSE \n" + tetrisGrid2.score.ToString() + " point(s)";
            }
            strMessage += "YOU : " + tetrisGrid2.score.ToString() + " point(s)\n ";
            strMessage += "RIVAL : " + tetrisGridd.score.ToString() + " point(s)";
            var result = MessageBox.Show(strMessage, strCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

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
                    TetrisClientInfo info = new TetrisClientInfo(tetrisGrid2);
                    remoteServer.Send(info);
                }
        }

        private void btnAbandonner_Click(object sender, EventArgs e)
        {
            remoteServer.ready = true;
            remoteServer.Send("ready");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (remoteServer.ClientSocket.Connected == true)
            {
                if (textBox1.Text != "")
                {
                    ChatMessage msg = new ChatMessage(textBox1.Text);
                    txtBoxChat.Text += "You: " + textBox1.Text + "\r\n";
                    textBox1.Text = "";
                    remoteServer.Send(msg);
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Down)
            {
                tetrisGrid2.drop();
            }
            if (keyData == Keys.Up)
            {
                tetrisGrid2.rotate();
            }
            if (keyData == Keys.Left)
            {
                tetrisGrid2.moveLeft();
            }
            if (keyData == Keys.Right)
            {
                tetrisGrid2.moveRight();
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
                    RivalGameOverHandler p = new RivalGameOverHandler(StopTimerGridMe);
                    this.Invoke(p);
                }
            }
            else if(data is ChatMessage)
            {
                Console.WriteLine("pddddrinting");
                if (((ChatMessage)data).strMessage != "")
                {
                    String strMessage = ((ChatMessage)data).strMessage;
                    PrintMessageHandler p = new PrintMessageHandler(printMessageChat);
                    this.Invoke(p, txtBoxChat, strMessage);
                }
            }
            else
            {
                TetrisClientInfo info = (TetrisClientInfo)data;
                int rows = tetrisGridd.numLines;
                int cols = tetrisGridd.numCols;
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        tetrisGridd.labelsBlock[i, j].BackColor = info.tbColors[i,j];
                    }
                }
                tetrisGridd.score = info.score;
                onScoreChanged(tetrisGridd, EventArgs.Empty);
            }
        }

        private void printMessageChat(TextBox txtbox, string msgToPrint)
        {
            Console.WriteLine("printing");
            txtbox.Text += "Rival: " + msgToPrint +"\r\n";
        }

        /**Appelé quand l'adversaire a perdu*/
        private void StopTimerGridMe()
        {
            tetrisGrid2.asyncstopGrid();
            String strMessage = "";
            String strCaption = "RIVAL GAME OVER";
            if (tetrisGrid2.score > tetrisGridd.score)
            {
                strMessage = "YOU WIN \n";
            }
            else if (tetrisGrid2.score == tetrisGridd.score)
            {
                strMessage = "YOU WIN \n";
            }
            else
            {
                strMessage = "YOU LOSE \n" + tetrisGrid2.score.ToString() + " point(s)";
            }
            strMessage += "YOU : " + tetrisGrid2.score.ToString() + " point(s)\n ";
            strMessage += "RIVAL : " + tetrisGridd.score.ToString() + " point(s)";
            var result = MessageBox.Show(strMessage, strCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation );

        }

        private void startTimerGridMe()
        {
            tetrisGrid2.start();
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
