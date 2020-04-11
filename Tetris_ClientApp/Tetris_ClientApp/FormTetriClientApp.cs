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
        TetrisGrid gridPlayerMe;
        TetrisGrid gridPlayerRival;
        private Timer _timer = new Timer();
        public formTetrisClientApp()
        {
            InitializeComponent();

            FormConnectServer serverConnection = new FormConnectServer();
            serverConnection.ShowDialog();

            gridPlayerMe = new TetrisGrid(300, 600, 20, 10);
            gridPlayerRival = new TetrisGrid(300, 600, 20, 10);
            
            gridPlayerMe.Location = new Point(100, this.Height - gridPlayerMe.Height - 100); 
            gridPlayerRival.Location = new Point(600, this.Height - gridPlayerRival.Height - 100);

            this.Controls.Add(gridPlayerMe);
            this.Controls.Add(gridPlayerRival);
            _timer = new Timer();
            _timer.Interval = 300;
            _timer.Tick += new EventHandler(TimerTick);
        }

        private void TimerTick(object sender, EventArgs e)
        {

            gridPlayerMe.updateGrid();
            //Console.WriteLine("Tick");
        }

        private void btnAbandonner_Click(object sender, EventArgs e)
        {
            _timer.Enabled = true;
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
    }
}
