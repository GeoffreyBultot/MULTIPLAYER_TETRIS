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
        public int rectSize = 10;
        public int numLines = 10;
        public int numCols = 10;

        int lines = 0;
        int x2 = 0;
        int x3 = 0;
        int x4 = 0;

        public int score { get; set; }

        public Label[,] labelsBlock { get; set; }
        
        private int px;
        private int py;
        private int next = 0;
        private int level = 1;
        bool game = false;

        Pen[] pens = new Pen[9];
        Brush[] brushes = new Brush[9];
        Figure figure, nf;
        
        private Timer _timer = new Timer();

        public event EventHandler FigureMovedDown;
        public event EventHandler ScoreChanged;
        public event EventHandler GameOver;

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
            _timer.Interval = (int)(300 / (level * 0.7));
            _timer.Tick += new EventHandler(TimerTick);
        }
        public void stop()
        {
            game = false;
            _timer.Stop();
            //Console.WriteLine("STOP");
            /*if(GameOver != null)
            {
                GameOver(this,EventArgs.Empty);
            }*/
            //myTimer.Stop();
        }
        public void start()
        {
            game = true;
            setfigure();
            resetGrid();
            //_timer.Interval = (int)(100 / (level * 0.7));
            _timer.Start();
            
        }

        private void TimerTick(object sender, EventArgs e)
        {
           //Console.WriteLine("tick");
            updateGrid();
        }

        public void updateGrid()
        {
            if (game)
            {
                if (!moveDown())
                {
                    stop();
                    
                    if (GameOver != null)
                    {
                        GameOver(this, EventArgs.Empty);
                    }
                }
            }
        }
        void setfigure()
        {
            Random rnd = new Random();
            /*if (next == 0) 
            { 
                next = rnd.Next(1, 7); 
                setfigure(); return;
            }
            int i = next;
            */
            int i = next;
            next = rnd.Next(0, 7);
            if (!(i > 0 && i < 8)) return;
            figure = new Figure(i);
            nf = new Figure(next);
            py = 0;
            px = 4;
        }
        
        public void drop()
        {
            if (game)
            {
                while (moveDown()) ;
            }
            //FigureMoved(this, EventArgs.Empty);
            //Refresh();
        }
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
        private bool moveDown()
        {
            //labelsBlock[4, 4].BackColor = Color.Blue;
            //erase figure
            int sz = (int)Math.Sqrt(figure.figure.Length);

            erasefigure(figure, px, py);
            if (check(figure, px, py + 1))
            {
                py++;
                //Console.WriteLine("draw by movedown");
                drawfigure(figure, px, py);

                //Console.WriteLine(px.ToString() + ";" + py.ToString());
                if (FigureMovedDown != null)
                    FigureMovedDown(this, EventArgs.Empty);
                return true;
            }
            else
            {
                drawfigure(figure, px, py);
            }
            
            int count = 0;

            for (int j = 1; j < 20; j++)
            {
                bool full = true;
                for (int i = 0; i < 10; i++)
                {
                    if (labelsBlock[j, i].BackColor == Color.Black) full = false;
                }
                if (full)
                {
                    lines++;
                    count++;
                    for (int k = j; k > 0; k--)
                        for (int i = 0; i < 10; i++)
                                labelsBlock[k, i].BackColor = labelsBlock[k-1, i].BackColor;
                }
                full = true;
            }

            setfigure();

            if (lines % 20 == 0 && lines != 0 && (level - 1) * 20 != lines) level++;

            if (count == 1) score += (10 * count);
            if (count == 2) score += (20 * count);
            if (count == 3) score += (30 * count);

            if (ScoreChanged != null)
                ScoreChanged(this, EventArgs.Empty);

            return false;
        }

        void drawfigure(Figure fg, int x, int y)
        {
            int sz = figure.size;

            for (int i = 0; i < sz; i++)
            {
                for (int j = 0; j < sz; j++)
                {
                    int rx = j + x;
                    int ry = i + y;
                    //Console.WriteLine(rx.ToString(),ry.ToString());
                    if ( (!(ry < 0 || ry > 19 || rx < 0 || rx > 9)) && fg.figure[i, j] != 0)
                    {
                        //Console.WriteLine("from drawing:" + (i + x).ToString());
                        //Console.WriteLine("from drawing:" + x.ToString() + ";"+ fg[i, j].ToString());
                        labelsBlock[i + y, j + x].BackColor = fg.colorFigure;//figure[i, j];
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
                    if ((!(ry < 0 || ry > 19 || rx < 0 || rx > 9)) && fg.figure[i, j] != 0)
                    {
                        labelsBlock[i + y, j + x].BackColor = Color.Black;//figure[i, j];
                    }
                }
            }
        }

        bool check(Figure fg, int x, int y)
        {
            bool ret = true;
            int sz = figure.size;
            for (int i = sz - 1; i > -1; i--)
                for (int j = 0; j < sz; j++)
                {
                    int rx = j + x;
                    int ry = i + y;
                    //Console.WriteLine(rx.ToString(),ry.ToString());
                    if ((rx < 0 || rx > 9 || ry < 0 || ry > 19) && fg.figure[i, j] != 0)
                        return false;
                    if (!(rx < 0 || rx > 9 || ry < 0 || ry > 19))
                    {
                        if (fg.figure[i, j] != 0)
                        {
                            if (labelsBlock[ry, rx].BackColor != Color.Black)
                                return false;
                        }
                    }
                }
            return true;
        }

        private void drawGrid(int rows, int cols)
        {
            //LabelsCreation
            float interval = (float)(this.Width) / (float)(cols);
            float sizeSquare = interval * 0.9f;

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

        private void resetGrid()
        {
            for(int i = 0; i < 20; i++) 
            {
                for (int j = 0; j < 10; j++)
                {
                    labelsBlock[i,j].BackColor = Color.Black;
                }
            }
        }

        
        class Figure
        {
            public int[,] figure;
            public Color colorFigure;
            public int size = 0;
            List<Color> ColorList = new List<Color>();
            Random rnd = new Random();
            public Figure(int num)
            {
                ColorList.Add(Color.Cyan);
                ColorList.Add(Color.DarkOrange);
                ColorList.Add(Color.Green);
                ColorList.Add(Color.DarkMagenta);
                ColorList.Add(Color.Aqua);
                //ColorList.Add(Color.Coral);
                ColorList.Add(Color.Red);
                ColorList.Add(Color.Blue);
                //int random = rnd.Next(0, ColorList.Count);
                figure = figures[num];
                colorFigure = ColorList[num];
                size = (int)Math.Sqrt(figure.Length);
            }

            public Figure()
            {
            }

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
                {0,0,0},
                {0,0,5},
                {5,5,5}
                },
                    new int[3,3]
                {
                {0,0,0},
                {6,0,0},
                {6,6,6}
                },
                    new int[4,4]
                {
                {7,0,0,0},
                {7,0,0,0},
                {7,0,0,0},
                {7,0,0,0}
                }
            };
        }
    }
}
