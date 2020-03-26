using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris_ClientApp
{
    
    public class TetrisGrid : Panel
    {
        const int rectSize = 10;
        const int numLines = 10;
        const int numCols = 10;


        

        Label[,] labelsBlock;
        public TetrisGrid(int width, int height, int rows, int cols)
        {
            
            this.Height = height;
            this.Width = width;
            this.BackColor = Color.DarkGray;
            drawGrid(rows, cols);
            Brick iiii = new Brick();
            iiii.Height = 50;
            iiii.Width = 50;
            this.Controls.Add(iiii);
            //iiii.Location = new Point(this.Width / 2, this.Height / 2);
        }

        private void drawGrid(int rows, int cols)
        {
            //LabelsCreation

            Console.WriteLine(rows.ToString());
            Console.WriteLine(cols.ToString());
            float interval = (float)(this.Width) / (float)(cols);
            float sizeSquare = interval * 0.9f;

            Console.WriteLine(interval.ToString());
            Console.WriteLine(sizeSquare.ToString());

            int i,j;
            
            labelsBlock = new Label[rows,cols];
            int sizeAddToCenter = (int)((interval-sizeSquare)/2);

            for (i = 0; i< rows; i++)
            {
                for (j = 0; j < cols; j++) 
                {
                    labelsBlock[i, j] = new Label();
                    labelsBlock[i, j].BackColor = Color.Black;
                    labelsBlock[i, j].Location = new Point(sizeAddToCenter + (int)interval * j, +sizeAddToCenter + (int)interval * i);
                    labelsBlock[i, j].Name = "blockLabel" + i.ToString() + j.ToString();
                    labelsBlock[i, j].Size = new Size((int)sizeSquare, (int)sizeSquare);
                    //labelsBlock[i, j].TabIndex =;
                    this.Controls.Add(labelsBlock[i, j]);
                }
                
            }
        }

    }
}
