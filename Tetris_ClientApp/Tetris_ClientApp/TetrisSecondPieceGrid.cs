using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris_ClientApp
{
    class TetrisSecondPieceGrid:Panel
    {
        
        PictureBox[,] pictBox_Case { get; set; }

        int rows, cols;

        public TetrisSecondPieceGrid()
        {
            this.BackColor = Color.Transparent;
            this.Height = 80;
            this.Width= 80;
            rows = 4;
            cols = 4;
            drawGrid(4, 4);

        }

        public void setFigure(Figure fg)
        {
            if (fg != null)
            {
                int sz = fg.size;
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        pictBox_Case[i, j].BackColor = Color.Transparent;
                    }
                }
                for (int i = 0; i < sz; i++)
                {
                    for (int j = 0; j < sz; j++)
                    {
                        if (fg.figure[i, j] != 0)
                        {
                            pictBox_Case[i, j].BackColor = fg.colorFigure;
                        }
                    }
                }
            }
        }

        private void drawGrid(int rows, int cols)
        {
            float interval = (float)(this.Width) / (float)(cols);
            float sizeSquare = interval * 0.9f;
            int i, j;

            pictBox_Case = new PictureBox[rows, cols];

            for (i = 0; i < rows; i++)
            {
                for (j = 0; j < cols; j++)
                {
                    pictBox_Case[i, j] = new PictureBox();
                    //pictBox_Case[i, j].BackColor = Color.Black;
                    pictBox_Case[i, j].Location = new Point((int)interval * j, (int)interval * i);
                    pictBox_Case[i, j].Name = "blockLabel" + i.ToString() + j.ToString();
                    pictBox_Case[i, j].Size = new Size((int)sizeSquare, (int)sizeSquare);
                    //pictBox_Case[i, j].TabIndex =;
                    this.Controls.Add(pictBox_Case[i, j]);
                }
            }
            Form parentForm = (this.Parent as Form);
        }
    }
}
