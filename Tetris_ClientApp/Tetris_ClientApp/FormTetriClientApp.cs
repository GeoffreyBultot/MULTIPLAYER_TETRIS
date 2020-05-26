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
        TetrisGrid gridPlayerMe;
        TetrisGrid gridPlayerRival; 
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
                txtBoxChat.Text += "Welcome in the channel " + serverConnection.stCode.ToString();
                gridPlayerMe.FigureMovedDown += onFigureMovedDown;
                gridPlayerMe.ScoreChanged += onScoreChanged;
                gridPlayerMe.GameOver += onGameOver;
                gridPlayerMe.FigureSet += onFigureSet;
            }
            else //if(serverConnection.DialogResult == DialogResult.Cancel)
            {
                Load += (s, e) => Close();
            }
        }

        private void onFigureSet(Figure fg)
        {
            secondPieceGrid.setFigure(fg);
        }

        #region GRAPHICS
        void InitGraphics()
        {
            gridPlayerMe = new TetrisGrid(300, 600, 20, 10);
            gridPlayerRival = new TetrisGrid(300, 600, 20, 10);

            this.Height = gridPlayerMe.Height + 150;

            /** GRIDS LOCATION */
            gridPlayerMe.Location = new Point(50, this.Height - gridPlayerMe.Height - 50);
            gridPlayerRival.Location = new Point(gridPlayerMe.Location.X+ gridPlayerMe.Width+50, this.Height - gridPlayerRival.Height - 50);
            secondPieceGrid.Location = new Point(gridPlayerMe.Location.X, gridPlayerMe.Location.Y - secondPieceGrid.Height - 10);
            this.Controls.Add(gridPlayerMe);
            this.Controls.Add(gridPlayerRival);

            /** CHAT LOCATION */
            txtBoxChat.Location = new Point(gridPlayerRival.Location.X + gridPlayerRival.Width + 20, gridPlayerRival.Location.Y);
            btnSend.Location = new Point(txtBoxChat.Location.X + txtBoxChat.Width - btnSend.Width, txtBoxChat.Location.Y + txtBoxChat.Height + 5);
            textBox1.Location = new Point(txtBoxChat.Location.X , txtBoxChat.Location.Y + txtBoxChat.Height + 5);
            pictureBox1.Location = new Point(txtBoxChat.Location.X + (txtBoxChat.Width) / 2 - pictureBox1.Width/2, gridPlayerRival.Location.Y + gridPlayerRival.Height - pictureBox1.Height);
            btnReady.Location = new Point(txtBoxChat.Location.X + txtBoxChat.Width - btnReady.Width, txtBoxChat.Location.Y - btnReady.Height - 5);
            /** FONT LABELS SCORE */
            pfc.AddFontFile(Application.StartupPath+"\\..\\..\\Datas\\7_Segment.ttf");
            Font labelTetrisFont = new Font(pfc.Families[0], 26, FontStyle.Bold);
            
            lblScoreMe.Font = labelTetrisFont;
            lblScoreRival.Font = labelTetrisFont;
            lblRival.Font = labelTetrisFont;
            lblYOU.Font = labelTetrisFont;
            
            /** LABELS SCORE LOCATION */

            lblScoreMe.Location = new Point(gridPlayerMe.Location.X + (gridPlayerMe.Width) / 2 - lblScoreMe.Width / 2, gridPlayerMe.Location.Y - lblScoreMe.Height - 5);
            lblYOU.Location = new Point(gridPlayerMe.Location.X + (gridPlayerMe.Width) / 2 - lblYOU.Width / 2, lblScoreMe.Location.Y - lblYOU.Height - 5);
            lblScoreRival.Location = new Point(gridPlayerRival.Location.X + (gridPlayerRival.Width) / 2 - lblScoreRival.Width / 2, gridPlayerRival.Location.Y - lblScoreRival.Height - 5);
            lblRival.Location = new Point(gridPlayerRival.Location.X + (gridPlayerRival.Width) / 2 - lblRival.Width / 2, lblScoreMe.Location.Y - lblRival.Height - 5);


            this.Width = txtBoxChat.Location.X + txtBoxChat.Width + 50;
        }
        #endregion


        private void onScoreChanged(object sender, EventArgs e)
        {
            if (sender is TetrisGrid)
            {

                PrintHandler p = new PrintHandler(printScore);

                String strScore = "";
                if ((TetrisGrid)sender == gridPlayerMe)
                {
                    strScore = "score " + gridPlayerMe.score.ToString();
                    this.Invoke(p, lblScoreMe, strScore);
                }
                else if ((TetrisGrid)sender == gridPlayerRival)
                {
                    strScore = "score " + gridPlayerRival.score.ToString();
                    this.Invoke(p, lblScoreRival, strScore);
                }
            }
        }

        /**Appelé quand Me a perdu*/
        private void onGameOver(object sender, EventArgs e)
        {
            remoteServer.Send("gameOver");

            gridPlayerMe.asyncstopGrid();
            String strMessage = "";
            String strCaption = "GAME OVER";
            if (gridPlayerMe.score > gridPlayerRival.score)
            {
                strMessage = "YOU WIN \n";
            }
            else if (gridPlayerMe.score == gridPlayerRival.score)
            {
                strMessage = "YOU LOSE \n";
            }
            else
            {
                strMessage = "YOU LOSE \n";
            }
            strMessage += "YOU : " + gridPlayerMe.score.ToString() + " point(s)\n ";
            strMessage += "RIVAL : " + gridPlayerRival.score.ToString() + " point(s)";
            var result = MessageBox.Show(strMessage, strCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            btnReady.Enabled = true;
            btnReady.Text = "Ready";
        }

        /**Appelé quand l'adversaire a perdu*/
        private void StopGameGridMe()
        {
            gridPlayerMe.asyncstopGrid();
            String strMessage = "";
            String strCaption = "RIVAL GAME OVER";
            if (gridPlayerMe.score > gridPlayerRival.score)
            {
                strMessage = "YOU WIN \n";
            }
            else if (gridPlayerMe.score == gridPlayerRival.score)
            {
                strMessage = "YOU WIN \n";
            }
            else
            {
                strMessage = "YOU LOSE \n" + gridPlayerMe.score.ToString() + " point(s)";
            }
            strMessage += "YOU : " + gridPlayerMe.score.ToString() + " point(s)\n ";
            strMessage += "RIVAL : " + gridPlayerRival.score.ToString() + " point(s)";
            var result = MessageBox.Show(strMessage, strCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            btnReady.Enabled = true;
            btnReady.Text = "Ready";
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
                    TimerHandler p = new TimerHandler(startGameGridMe);
                    this.Invoke(p);
                }
                else if ((String)data == "gameOver")
                {
                    RivalGameOverHandler p = new RivalGameOverHandler(StopGameGridMe);
                    this.Invoke(p);
                }
            }
            else if(data is ChatMessage)
            {
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
                int rows = gridPlayerRival.numLines;
                int cols = gridPlayerRival.numCols;
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        gridPlayerRival.pictBox_Case[i, j].BackColor = info.tbColors[i,j];
                    }
                }
                gridPlayerRival.score = info.score;
                onScoreChanged(gridPlayerRival, EventArgs.Empty);
            }
        }

        private void printMessageChat(TextBox txtbox, string msgToPrint)
        {
            Console.WriteLine("printing");
            txtbox.Text += "Rival: " + msgToPrint +"\r\n";
        }


        private void startGameGridMe()
        {
            gridPlayerMe.start();
            btnReady.Enabled = false;
            btnReady.Text = "In game";
            PrintHandler p = new PrintHandler(printScore);
            lblScoreMe.Text = "score " + gridPlayerMe.score.ToString();
            lblScoreRival.Text = "score " + gridPlayerRival.score.ToString();
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
