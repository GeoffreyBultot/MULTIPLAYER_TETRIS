﻿using System;
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
        TetrisGrid gridPlayer1;
        public formTetrisClientApp()
        {
            InitializeComponent();
            gridPlayer1 = new TetrisGrid();

        }

        private void btnAbandonner_Click(object sender, EventArgs e)
        {
            gridPlayer1.InitNewGame();
        }


    }
}
