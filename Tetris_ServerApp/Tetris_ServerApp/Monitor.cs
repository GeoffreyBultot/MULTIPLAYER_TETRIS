using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris_ServerApp
{
    public class Monitor : TextBox
    {
        public int NumberOfLines { get; set; }

        public Monitor()
        {
            Multiline = true;
            BackColor = Color.Black;
            ForeColor = Color.Green;
            NumberOfLines = 2;
        }
        public void AddMessage(string msg)
        {
            this.AppendText(msg + "\r\n");

            while (Lines.Length > NumberOfLines + 1)
                Text = Text.Remove(0, Lines[0].Length + 2);
        }
    }
}
