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
        



        System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        public int lines = 0, x2 = 0, x3 = 0, x4 = 0;
        int next = 0;
        int level = 1;
        public Label[,] labelsBlock;
        bool game = false;
        Pen[] pens = new Pen[9];
        Brush[] brushes = new Brush[9];
        int px, py;
        Figure figure, nf;


        public TetrisGrid(int width, int height, int rows, int cols)
        {
            
            this.Height = height;
            this.Width = width;
            this.BackColor = Color.DarkGray;
            numLines = rows;
            numCols = cols;
            drawGrid(rows, cols);

            setfigure();
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    labelsBlock[i, j].BackColor = Color.Black;
            myTimer.Interval = (int)(100 / (level * 1));
            myTimer.Start();
        }
        public void stop()
        {
            game = false;
            Console.WriteLine("STOP");
            //myTimer.Stop();
        }

        public void updateGrid()
        {
            moveDown();
            if (!check(figure, px, py)) stop();
            //Refresh();
        }
        void setfigure()
        {
            Random rnd = new Random();
            if (next == 0) { next = rnd.Next(1, 8); setfigure(); return; }
            int i = next;
            next = rnd.Next(1, 8);

            if (!(i > 0 && i < 9)) return;
            figure = new Figure(i);
            nf = new Figure(next);
            //figure = copyfigure(figures[i]);
            //nf = copyfigure(figures[next]);
            py = -1;
            px = 4;
        }
        int conversize(int sz)
        {
            int s = 0;
            if (sz == 4) s = 2;
            if (sz == 9) s = 3;
            if (sz == 16) s = 4;
            return s;
        }
        int[,] copyfigure(int[,] f)
        {
            int sz = conversize(f.Length);
            int[,] ff = new int[sz, sz];
            for (int k = 0; k < sz; k++)
                for (int j = 0; j < sz; j++)
                    ff[k, j] = f[k, j];
            return ff;
        }
        public void drop()
        {
            while (moveDown()) ;
            //Refresh();
        }
        public void moveLeft()
        {
            erasefigure(figure, px, py);
            if (check(figure, px - 1, py))
            {
                //Console.WriteLine("OK");
                px--;
                //Console.WriteLine("draw by moveleft");
            }
            drawfigure(figure, px, py);
            //Refresh();
        }
        public void moveRight()
        {
            erasefigure(figure, px, py);
            if (check(figure, px + 1, py))
            {
                //Console.WriteLine("draw by moveright");
                px++;
            }
            drawfigure(figure, px, py);
            //Refresh();
        }

        public void rotate()
        {
            int sz = conversize(figure.figure.Length);
            Figure ff = new Figure();
            ff.figure = new int[sz, sz];
            ff.colorFigure = figure.colorFigure;
            //int[,] ff = new int[sz, sz];
            erasefigure(figure, px, py);

            if (sz != 4)
            {
                for (int k = 0; k < sz; k++)
                    for (int j = 0; j < sz; j++)
                    {
                        ff.figure[j, (sz - 1) - k] = figure.figure[k, j];
                    }
            }
            else
            {
                for (int k = 0; k < sz; k++)
                    for (int j = sz - 1; j >= 0; j--)
                        ff.figure[k, j] = figure.figure[j, k];
            }
            if (check(ff, px, py))
            {

                figure = ff;//copyfigure(ff.figure);

                drawfigure(figure, px, py);
            }
        }
        private bool moveDown()
        {
            //labelsBlock[4, 4].BackColor = Color.Blue;
            //erase figure
            int sz = conversize(figure.figure.Length);

            erasefigure(figure, px, py);
            if (check(figure, px, py + 1))
            {
                py++;
                //Console.WriteLine("draw by movedown");
                drawfigure(figure, px, py);

                //Console.WriteLine(px.ToString() + ";" + py.ToString());

                return true;
            }
            else
            {
                drawfigure(figure, px, py);
            }
            //Console.WriteLine(py.ToString() + px.ToString());

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
                                labelsBlock[k, i].BackColor = labelsBlock[k-1, i].BackColor;//Color.Blue;//board[i, k] = board[i, k - 1];
                }
                full = true;
            }
            setfigure();
            if (lines % 20 == 0 && lines != 0 && (level - 1) * 20 != lines) level++;//start(level + 1);
            if (count == 2) x2++;
            if (count == 3) x3++;
            if (count == 4) x4++;
            return false;
        }

        void drawfigure(Figure fg, int x, int y)
        {
            int sz = conversize(fg.figure.Length);

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
            int sz = conversize(fg.figure.Length);

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
            int sz = conversize(fg.figure.Length);
            for (int i = sz - 1; i > -1; i--)
                for (int j = 0; j < sz; j++)
                {
                    int rx = j + x;
                    int ry = i + y;
                    //Console.WriteLine(rx.ToString(),ry.ToString());
                    if ((ry < 0 || ry > 19 || rx < 0 || rx > 9 ) && fg.figure[i, j] != 0)
                        ret = false;

                    if(ret == true)
                    {
                        if (!(ry < 0 || ry > 19 || rx < 0 || rx > 9))
                        {
                            if (fg.figure[i, j] != 0)
                            {
                                if (labelsBlock[ry, rx].BackColor != Color.Black)
                                    ret = false;
                            }
                        }
                    }
                    
                }
            return ret;
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

        
        class Figure
        {
            public int[,] figure;
            public Color colorFigure;
            List<Color> ColorList = new List<Color>();
            Random rnd = new Random();
            public Figure(int num)
            {
                ColorList.Add(Color.Cyan);
                ColorList.Add(Color.DarkOrange);
                ColorList.Add(Color.Green);
                ColorList.Add(Color.DarkMagenta);
                ColorList.Add(Color.Aqua);
                ColorList.Add(Color.Coral);
                ColorList.Add(Color.Red);
                ColorList.Add(Color.Blue);
                //int random = rnd.Next(0, ColorList.Count);
                figure = figures[num];
                colorFigure = ColorList[num];
            }

            public Figure()
            {
            }

            public static int[][,] figures = new int[8][,]
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
                {7,7,7,7},
                {0,0,0,0},
                {0,0,0,0},
                {0,0,0,0}
                },
                    new int[3,3]
                {
                {0,8,0},
                {8,8,8},
                {0,0,0}
                }

            };
        }
    }
}
