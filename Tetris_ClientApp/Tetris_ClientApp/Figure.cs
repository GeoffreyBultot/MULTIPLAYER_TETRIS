using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_ClientApp
{
    class Figure
    {
        public int[,] figure;
        public Color colorFigure;
        public int size = 0;
        Random rnd = new Random();
        public Figure()
        {
            int num = rnd.Next(0, 7);
            figure = figures[num];
            colorFigure = colors[num];
            size = (int)Math.Sqrt(figure.Length);
        }

        public static Color[] colors = new Color[7]
        {
            Color.Cyan,
            Color.DarkOrange,
            Color.Green,
            Color.DarkMagenta,
            Color.Aqua,
            Color.Red,
            Color.Blue,
        };

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
