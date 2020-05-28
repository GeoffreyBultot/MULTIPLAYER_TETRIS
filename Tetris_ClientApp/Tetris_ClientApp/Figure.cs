using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_ClientApp
{
    /**
     * Cette classe permet la génération des figures présentes dans le tetrisGrid
     * */
    public class Figure
    {
        public int[,] figure;
        public Color colorFigure;
        public int size = 0;
        Random rnd = new Random();
        #region constructors
        public Figure()
        {
            int num = rnd.Next(0, 7);
            figure = figures[num];
            colorFigure = colors[num];
            //La size : si une pièce prend une tableau 3x3, la taille sera 3, ce qui est la sqrt de 9, la taille revoyée par figure.Length
            size = (int)Math.Sqrt(figure.Length);
        }
        #endregion

        //Tableau de couleur des figures
        public static Color[] colors = new Color[7]
        {
            Color.Cyan,
            Color.DarkOrange,
            Color.Green,
            Color.DarkMagenta,
            Color.DarkViolet,
            Color.Red,
            Color.Blue,
        };
        //Forme des figures
        public static int[][,] figures = new int[7][,]
        {
                new int[3,3]
                {
                {0,0,0},
                {0,1,0},
                {1,1,1}
                },
                new int[3,3]
                {
                {0,0,0},
                {0,2,2},
                {2,2,0}
                },
                new int[3,3]
                {
                {0,0,0},
                {3,3,0},
                {0,3,3}
                },
                    new int[2,2]
                {
                {4,4},
                {4,4}
                },
                    new int[3,3]
                {
                {5,0,0},
                {5,0,0},
                {5,5,0}
                },
                    new int[3,3]
                {
                {0,6,0},
                {0,6,0},
                {6,6,0}
                },
                    new int[4,4]
                {
                {7,0,0,0},
                {7,0,0,0},
                {7,0,0,0},
                {7,0,0,0}
                },
    };

    }
}
