using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_ClientApp
{
    /*
     * Permet se serialiser les infos d'une grid. pour afficher la grid du rival chez le joueur
     * **/
    [Serializable]
    class TetrisClientInfo
    {

        public Color[,] tbColors { get; set; }
        public int score;
        

        public TetrisClientInfo(TetrisGrid tg)
        {
            int size = tg.pictBox_Case.Length;
            int rows = tg.numLines;
            int cols = tg.numCols;
            this.tbColors = new Color[rows,cols];
            score = tg.score;
            for (int i = 0; i < rows; i++)
            {//Copie des couleurs de la grid
                for (int j = 0; j < cols; j++)
                {
                    this.tbColors[i, j] = (tg.pictBox_Case[i, j].BackColor);
                }//this.BackColor = s.BackColor;

            }
        }
    }
}