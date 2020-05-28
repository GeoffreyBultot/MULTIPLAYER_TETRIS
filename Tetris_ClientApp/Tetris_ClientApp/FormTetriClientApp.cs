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
    /**
     * Quand le client ouvre l'application, on démarre cette fenêtre et avant de l'afficher, on uvre une autre (dialog) pour qu'il puisse se connecter au serveur (voir FormConnectServer.cs). 
     * Il est à noter que le TetrisGrid hérite de la classe panel, ce qui permet de le placer directement dans le designer. Cependant, comme il contient un tableau à deux dimensions, 
     * Visual Studio génère des erreurs en disant qu'il ne peut gérer des tableaux que 'une seule dimension.
     * J'ajoute donc le tetrisGrid
     */
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

            //Création des tetrisgrid
            gridPlayerMe = new TetrisGrid(300, 600, 20, 10);
            gridPlayerRival = new TetrisGrid(300, 600, 20, 10);
            //this.btnReady.KeyDown += new System.Windows.Forms.
            resizeGraphics();


            serverConnection.ShowDialog();
            //Si le client clique sur OK
            if (serverConnection.DialogResult == DialogResult.OK)
            {
                //Le client remoteServer est tout d'abord créé dans la fenêtre FormConnectServer. On le passe à cette classe pour qu'il puisse être exploité ici.
                remoteServer = serverConnection.remoteServer;
                //Ajout des callback d'événement de communication
                remoteServer.DataReceived += RemoteServer_DataReceived;
                remoteServer.ClientDisconnected += RemoteServer_ClientDisconnected;
                remoteServer.ConnectionRefused += RemoteServer_ConnectionRefused;
                //Récupération du channel (la validité de cette info est gérée dans FormConnectServer)
                byte[] codeChannel = Encoding.ASCII.GetBytes(serverConnection.stCode);
                //Envoi du channel au serveur. Si un channel portant le même numéro est existant, le client le rejoindra et dans le cas contraire, le serveur créera le channel et y placera le client
                remoteServer.Send(codeChannel);
                //Affiche le numéro du channel pour que le client puisse le donner à la personne avec qui il veut jouer
                txtBoxChat.Text += "Welcome in the channel " + serverConnection.stCode.ToString();
                //Callaback quand la figure bouge, on l'utilise pour envoyer les infos de la grid
                gridPlayerMe.FigureMovedDown += onFigureMovedDown;
                //Quand le score change dans la classe tetrisgrid, pour l'afficher dans les label
                gridPlayerMe.ScoreChanged += onScoreChanged;
                //Callback quand le joueur a perdu
                gridPlayerMe.GameOver += onGameOver;
                //Callback quand une nouvelle pièce est générée. Elle permet de changer la pièce dans le pannel contenant la "next piece"
                gridPlayerMe.FigureSet += onFigureSet;
            }
            //Si le client annule la connexion
            else //if(serverConnection.DialogResult == DialogResult.Cancel)
            {
                //Fermeture de l'appli si le client annule la connexion
                Load += (s, e) => Close();
            }
        }

        

        #region RESIZEGRAPHICS
        void resizeGraphics()
        {
            //Comme les tetrisGrid sont ajoutés en ligne de code, je réaligne les autres éléments aux tetrisgrid pour un meilleur rendu esthétique 
            //taille de la fenêtre dépendant des tetrisgrid
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

            //Longueur de la fenêtre
            this.Width = txtBoxChat.Location.X + txtBoxChat.Width + 50;
        }
        #endregion

        #region TETRISGRISCALLBACK
        //Quand le score change
        private void onScoreChanged(object sender, EventArgs e)
        {
            if (sender is TetrisGrid)
            {

                PrintHandler p = new PrintHandler(printScore);
                //Le sender permet de savoir dans quel label mettre le score
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
            if (sender is TetrisGrid)
            {

                remoteServer.Send("gameOver");

                gridPlayerMe.stopGrid();
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
        }

        
        private void onFigureMovedDown(object sender, EventArgs e)
        {
            if (sender is TetrisGrid)
            {
                if (remoteServer != null)
                    if (remoteServer.ClientSocket.Connected)
                    {
                        TetrisClientInfo info = new TetrisClientInfo(gridPlayerMe);
                        remoteServer.Send(info);
                    }
            }
        }
        //affiche la prochaine figure dans le control TetrisSecondPieceGrid
        private void onFigureSet(object sender, Figure nextPiece)
        {
            if (sender is TetrisGrid)
            {
                secondPieceGrid.setFigure(nextPiece);
            }
        }
        #endregion


        /**Appelé par INVOKE grâce au RivalGameOverHandler quand l'adversaire a perdu*/
        private void StopGameGridMe()
        {
            //Permet de stopper la grid
            gridPlayerMe.stopGrid();
            //Affiche win ou lose dans une messagebox
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

        //Appelé par un printhandler pour print le score dans le bon label
        private void printScore(Label label, string msgToPrint)
        {
            label.Text = msgToPrint;
        }

        //Envoie au serveur que le joueur est ready
        private void btnReady_Click(object sender, EventArgs e)
        {
            remoteServer.ready = true;
            remoteServer.Send("ready");
        }

        //Envoie de messages dans le chat
        private void btnSend_Click(object sender, EventArgs e)
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
        //gestion des contrôles clavier pour bouger les pièces
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Console.WriteLine("toto");
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


        //Ferme le programme si problème de connexion
        private void RemoteServer_ConnectionRefused(Client client, string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.Close();
        }

        //Quand on reçoit des données du serveur
        private void RemoteServer_DataReceived(Client client, object data)
        {
            //Si c'est du texte, à ce niveau, c'est soir le serveur qui dit qu'on peut start la game ou un que l'adversaire a perdu
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
            else if(data is ChatMessage)//Si c'est un chat message, on l'affiche dans le chat
            {
                if (((ChatMessage)data).strMessage != "")
                {
                    String strMessage = ((ChatMessage)data).strMessage;
                    PrintMessageHandler p = new PrintMessageHandler(printMessageChat);
                    this.Invoke(p, txtBoxChat, strMessage);
                }
            }
            else if (data is TetrisClientInfo) //Si c'est un client info, on rafraichit la grid de l'adversaire. On utilise un TetriClientInfo parce qu'on ne peut pas sérialiser les données graphiques comme les picrutebox
            {
                TetrisClientInfo info = (TetrisClientInfo)data;
                int rows = gridPlayerRival.numLines;
                int cols = gridPlayerRival.numCols;
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {//Copie des couleurs de la grid de l'adversaire
                        gridPlayerRival.pictBox_Case[i, j].BackColor = info.tbColors[i,j];
                    }
                }
                //Ragrichissement du score
                gridPlayerRival.score = info.score;
                onScoreChanged(gridPlayerRival, EventArgs.Empty);
            }
        }

        //Vient d'un invoke d'un PrintMessageHandler pour ajouter les messages du rival quand ils arrivent du réseau
        private void printMessageChat(TextBox txtbox, string msgToPrint)
        {
            Console.WriteLine("printing");
            txtbox.Text += "Rival: " + msgToPrint +"\r\n";
        }

        //Appelé avec un invoke d'un TimerHandler pour démarrer le serveur. En effet par exemple, ne pas utiliser d'invoke provoque des bugs au démarrage du timer.
        //Et comme on remet les scores à 0, on doit changer les labels donc on doit passer par un invoke
        private void startGameGridMe()
        {
            gridPlayerMe.start();
            btnReady.Enabled = false;
            btnReady.Text = "In game";
            PrintHandler p = new PrintHandler(printScore);
            lblScoreMe.Text = "score 0";// + gridPlayerMe.score.ToString();
            lblScoreRival.Text = "score 0";// + gridPlayerRival.score.ToString();
        }

        private void RemoteServer_ClientDisconnected(Client client, string message)
        {
            //Close l'appli si il n'y a plus de connexion 
            MessageBox.Show("You have been disconnected ! Window will now close.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.Close();
        }

    }
}
