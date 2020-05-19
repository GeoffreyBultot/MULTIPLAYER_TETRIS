using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris_ClientApp
{
    
    public class TetrisGrid : Panel
    {
        public int rectSize = 10;
        public int numLines = 20;
        public int numCols = 10;

        int lines = 0;

        public int score { get; set; }

        public PictureBox[,] pictBox_Case { get; set; }
        
        private int px;
        private int py;

        private int level = 1;
        bool game = false;

        Figure figure, nf;
        
        private Timer _timer = new Timer();

        public event EventHandler FigureMovedDown;
        public event EventHandler ScoreChanged;
        public event EventHandler GameOver;

        public delegate void HandlerFigureSet(Figure fg);

        public event HandlerFigureSet FigureSet;


        public TetrisGrid()
        {
            this.Height = 600;
            this.Width = 300;
            this.BackColor = Color.DarkGray;

            drawGrid(numLines, numCols);

            setfigure();

            _timer = new Timer();
            _timer.Interval = (int)(200 / (level * 0.7));
            _timer.Tick += new EventHandler(TimerTick);
        }

        public TetrisGrid(int width, int height, int rows, int cols)
        {
            this.Height = height;
            this.Width = width;
            this.BackColor = Color.DarkGray;
            numLines = rows;
            numCols = cols;
            drawGrid(rows, cols);

            setfigure();

            _timer = new Timer();
            _timer.Interval = (int)(200 / (level * 0.7));
            _timer.Tick += new EventHandler(TimerTick);
        }

        public void stop()
        {
            game = false;
            _timer.Stop();
            if(GameOver != null)
            {
                GameOver(this,EventArgs.Empty);
            }
        }

        public void asyncstopGrid()
        {
            _timer.Stop();
            game = false;
        }

        public void start()
        {
            game = true;
            setfigure();
            resetGrid();
            _timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (game)
            {
                if(moveDown() == false)
                {
                    Console.WriteLine(py);
                    //erasefigure(figure, px, py);
                    if (!check(figure, px, py+1))
                        stop();
                    else
                        drawfigure(figure, px, py);
                }
            }
        }

        private bool moveDown()
        {
            int sz = figure.size;
            erasefigure(figure, px, py);
            if (check(figure, px, py + 1))
            {
                py++;
                drawfigure(figure, px, py);
                if (FigureMovedDown != null)
                    FigureMovedDown(this, EventArgs.Empty);
                return true;
            }
            else
            {
                drawfigure(figure, px, py);
            }

            int count = 0;

            for (int j = 1; j < numLines; j++)
            {
                bool full = true;
                for (int i = 0; i < numCols; i++)
                {
                    if (pictBox_Case[j, i].BackColor == Color.Black) full = false;
                }
                if (full)
                {
                    lines++;
                    count++;
                    for (int k = j; k > 0; k--)
                        for (int i = 0; i < 10; i++)
                            pictBox_Case[k, i].BackColor = pictBox_Case[k - 1, i].BackColor;
                }
                full = true;

            }

            if (lines % 20 == 0 && lines != 0 && (level - 1) * 20 != lines) level++;

            if (count == 1) score += (10 * count);
            if (count == 2) score += (20 * count);
            if (count == 3) score += (30 * count);

            if (ScoreChanged != null)
                ScoreChanged(this, EventArgs.Empty);
            setfigure();
            return false;
        }

        void setfigure()
        {
            figure = nf;
            nf = new Figure();
            py = -1;
            px = 4;

            _timer.Interval = (int)(200 / (level * 0.7));
            if(FigureSet != null)
            {
                FigureSet(nf);
                //FigureSet(this, EventArgs.Empty, nf);
            }

        }

        public void drop()
        {
            if (game)
            {
                while (moveDown()) ;
            }
        }
        #region moves
        public void moveLeft()
        {
            if (game)
            {
                erasefigure(figure, px, py);
                if (check(figure, px - 1, py))
                {
                    px--;
                }
                drawfigure(figure, px, py);
            }
        }
        public void moveRight()
        {
            if (game)
            {
                erasefigure(figure, px, py);
                if (check(figure, px + 1, py))
                {
                    px++;
                    //FigureMoved(this, EventArgs.Empty);
                }
                drawfigure(figure, px, py);
            }
        }

        public void rotate()
        {
            if (game)
            {
                int sz = figure.size;
                Figure ff = new Figure();
                ff.figure = new int[sz, sz];
                ff.colorFigure = figure.colorFigure;
                ff.size = figure.size;
                erasefigure(figure, px, py);

                if (sz != 4){
                    for (int k = 0; k < sz; k++)
                        for (int j = 0; j < sz; j++)
                        {
                            ff.figure[j, (sz - 1) - k] = figure.figure[k, j];
                        }
                }
                else{
                    for (int k = 0; k < sz; k++)
                        for (int j = sz - 1; j >= 0; j--)
                            ff.figure[k, j] = figure.figure[j, k];
                }
                if (check(ff, px, py)){
                    figure = ff;
                    drawfigure(figure, px, py);
                }
            }
        }
        
        #endregion

        void drawfigure(Figure fg, int x, int y)
        {
            int sz = figure.size;

            for (int i = 0; i < sz; i++)
            {
                for (int j = 0; j < sz; j++)
                {
                    int rx = j + x;
                    int ry = i + y;
                    if ( (!(ry < 0 || ry >= numLines || rx < 0 || rx >= numCols)) && fg.figure[i, j] != 0)
                    {
                        pictBox_Case[i + y, j + x].BackColor = fg.colorFigure;//figure[i, j];
                    }
                }
            }
        }

        void erasefigure(Figure fg, int x, int y)
        {
            int sz = figure.size;

            for (int i = 0; i < sz; i++)
            {
                for (int j = 0; j < sz; j++)
                {
                    int rx = j + x;
                    int ry = i + y;
                    //Console.WriteLine(rx.ToString(),ry.ToString());
                    if ((!(ry < 0 || ry >= numLines || rx < 0 || rx >= numCols)) && fg.figure[i, j] != 0)
                    {
                        pictBox_Case[i + y, j + x].BackColor = Color.Black;
                    }
                }
            }
        }

        bool check(Figure fg, int x, int y)
        {
            int sz = figure.size;
            for (int i = 0; i < sz; i++)
                for (int j = 0; j < sz; j++)
                {
                    int rx = j + x;
                    int ry = i + y;
                    //Console.WriteLine(rx.ToString(),ry.ToString());
                    if ((rx < 0 || rx >= numCols || ry < 0 || ry >= numLines) && fg.figure[i, j] != 0)
                        return false;
                    if (!(rx < 0 || rx >= numCols || ry < 0 || ry >= numLines))
                    {
                        if (fg.figure[i, j] != 0)
                        {
                            if (pictBox_Case[ry, rx].BackColor != Color.Black)
                                return false;
                        }
                    }
                }
            return true;
        }

        private void drawGrid(int rows, int cols)
        {
            float interval = (float)(this.Width) / (float)(cols);
            float sizeSquare = interval * 0.9f;
            int i,j;
            
            pictBox_Case = new PictureBox[rows,cols];
            int sizeAddToCenter = (int)((interval-sizeSquare)/2);

            for (i = 0; i< rows; i++)
            {
                for (j = 0; j < cols; j++) 
                {
                    pictBox_Case[i, j] = new PictureBox();
                    pictBox_Case[i, j].BackColor = Color.Black;
                    pictBox_Case[i, j].Location = new Point(sizeAddToCenter + (int)interval * j, +sizeAddToCenter + (int)interval * i);
                    pictBox_Case[i, j].Name = "blockLabel" + i.ToString() + j.ToString();
                    pictBox_Case[i, j].Size = new Size((int)sizeSquare, (int)sizeSquare);
                    //pictBox_Case[i, j].TabIndex =;
                    this.Controls.Add(pictBox_Case[i, j]);
                }
            }
            Form parentForm = (this.Parent as Form);
        }

        private void resetGrid()
        {
            for(int i = 0; i < 20; i++) 
            {
                for (int j = 0; j < 10; j++)
                {
                    pictBox_Case[i,j].BackColor = Color.Black;
                }
            }
            score = 0;
            level = 1;
        }
    }
}
