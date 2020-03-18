using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris_ClientApp
{
    
    public class TetrisGrid : Control
    {
        const int rectSize = 10;
        const int numLines = 10;
        const int numCols = 10;


        Timer timer = new Timer();
        int score = 0;
        int lines = 0;
        int nextFig = 0;

        bool gameOver = false;
        bool gameStarted = false;
        bool newFigure = false;
        internal SingleSquare[] squares;
        public TetrisGrid()
        {
            BackColor = SystemColors.Window;

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);

            score = 0;
            lines = 0;
            nextFig = 0;

            gameOver = false;
            gameStarted = false;
            newFigure = false;

            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(TimerTick);
        }

        private void TimerTick(object sender, EventArgs e)
        {
            

        }

        public void InitNewGame()
        {
            InitRectangles();
        }
        void InitRectangles()
        {

            squares = new SingleSquare[numLines * numCols];

            int counter = 0;
            for (int i = 0; i < numLines * rectSize; i += rectSize)
            {
                for (int j = 0; j < numCols * rectSize; j += rectSize)
                {
                    squares[counter] = new SingleSquare(this);
                    squares[counter].rect = new Rectangle(j, i, rectSize, rectSize);
                    counter++;
                }
            }
        }

    }
}
