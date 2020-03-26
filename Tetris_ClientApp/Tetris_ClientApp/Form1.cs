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
            gridPlayerMe = new TetrisGrid(300, 600, 20, 10);
            gridPlayerRival = new TetrisGrid(300, 600, 20, 10);
            
            gridPlayerMe.Location = new Point(100, this.Height - gridPlayerMe.Height); 
            gridPlayerRival.Location = new Point(600, this.Height - gridPlayerRival.Height);

            this.Controls.Add(gridPlayerMe);
            this.Controls.Add(gridPlayerRival);

            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.Tick += new EventHandler(TimerTick);
        }

        private void TimerTick(object sender, EventArgs e)
        {

        }

        private void btnAbandonner_Click(object sender, EventArgs e)
        {
            
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {


            if (keyData == Keys.Down)
            {
                Console.WriteLine("down");
                //gridPlayerMe.
                //p_player1.Y += 20;
            }
            if (keyData == Keys.Up)
            {
                //p_player1.Y -= 20;
            }
            if (keyData == Keys.Z)
            {
                //p_player2.Y -= 20;
            }
            if (keyData == Keys.S)
            {
                //p_player2.Y += 20;
            }
            return base.ProcessCmdKey(ref msg, keyData);

        }

    }
}
