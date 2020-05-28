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
    /*
     * Cette classe hérite de Panel et permet de créer une grille de tetris.
     * Les cases sont des Picturebox dont on modifie la couleur
     * Les espacements entre cases sont la couleur du picturebox derrière
     * **/
    public class TetrisGrid : Panel
    {
        //Taille par défaut d'un bloc
        public int rectSize = 10;
        //Lignes/col par défaut
        public int numLines = 20;
        public int numCols = 10;

        //Score du joueur
        public int score { get; set; }

        //Grille
        public PictureBox[,] pictBox_Case { get; set; }
        
        //Position actuelle de la pièce
        private int px;
        private int py;

        //Niveau (plus on gagne, plus le niveau augmente et donc plus la rapidité du jeux augmente)
        private int level = 1;
        //Is in game ?
        bool game = false;

        //Figure actuelle et nf veut vire new figure qui correspond à la prochaine figure
        Figure figure;
        Figure nf;
        
        //Timer de la descente des pièces
        private Timer _timer = new Timer();

        //Evenements
        public event EventHandler FigureMovedDown;
        public event EventHandler ScoreChanged;
        public event EventHandler GameOver;

        //Utilisation d'un delegate pour pouvoir passer un argument dans l'event
        public delegate void HandlerFigureSet(object sender, Figure fg);
        public event HandlerFigureSet FigureSet;

        #region constructors
        public TetrisGrid()
        {
            this.Height = 600;
            this.Width = 300;
            //Dessine la grille
            drawGrid(numLines, numCols);
            //Génère les figures
            setfigure();
            
            //Config timer
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
        #endregion

        //La différence entre stop() et stopGrid() C'est que (stop) est appelée par THIS pour dire qu'il a gameOver et donc appelle l'event GameOver
        //Alors que stopGrid peut être appelée de l'extérieur pour quand le rival a perdu, on stoppe la grid du joueur qui joue sur l'appli
        private void stop()
        {
            game = false;
            _timer.Stop();
            if(GameOver != null)
            {
                Console.WriteLine("gmd");
                GameOver(this,EventArgs.Empty);
            }
        }

        public void stopGrid()
        {
            game = false;
            _timer.Stop();
        }
        //quand on start le jeux, on reset tout
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
                    //Si on ne sait pas placer la pièce, il y a 2 hypothèse.
                    //La première, il n'y a plus de place donc le joueur perd.
                    //La deuxième est que la pièce est placée dans le fond de la grille/ sur une autre pièce et du coup, moveDown aura remis py = -1. on vérifie alors 
                    //Si la pièce peut se placeer en py + 1. Si elle peut, c'est que py = -1 parce qu'on est arrivé au fond et si non, c'est qu'il n'y a plus de place
                    //Console.WriteLine(py);
                    
                    if (check(figure, px, py+1))
                        drawfigure(figure, px, py);
                    else
                        stop();
                    
                }
            }
        }

        


        #region moves
        /*Fait tomber la pièce dans le fond*/
        public void drop()
        {
            if (game)
            {
                while (moveDown()) ;

                if (check(figure, px, py + 1))
                    drawfigure(figure, px, py);
                else
                    stop();
                Console.WriteLine("oui");
            }
        }
        /*Pour moveLeft et moveRight le principe est le même : on supprime la pièce, on check si on sait la placer à PX +1 ou -1. 
         * Si oui, on augmente ou diminue PX et on dessine la pièce ensuite*/
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
            int sz = figure.size;

            //Enlève la figure pour voir si elle sait se mettre au prochain (si une figure prend au moins 2 lignes pour la dessiner et qu'on regarde le py après sans l'enlever,
            // on verra un conflit avec la case au-dessus.
            erasefigure(figure, px, py);
            //Peut-on la placer à py+1 ?
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
                //Même si on ne peut pas la placer, on la redessine pour des raisons graphiques
                drawfigure(figure, px, py);
            }
            //Si on ne sait plus la placer, (il y a le return true si oui) soit on est dans le cas où on a perdu soit on est dans le cas où la pièce est placée et donc on vérifie si il y a une ligne complète
            int count = 0;
            int lines = 0;
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
            //Score en fonction du nombre de lignes d'un coup
            if (count == 1) score += (10 * count);
            if (count == 2) score += (20 * count);
            if (count == 3) score += (30 * count);
            if (count == 4) score += (40 * count);

            if (ScoreChanged != null)
                ScoreChanged(this, EventArgs.Empty);
            setfigure();
            return false;
        }


        #endregion

        void setfigure()
        {
            figure = nf;
            nf = new Figure();
            py = -1;
            px = 4;

            _timer.Interval = (int)(200 / (level * 0.7));
            if (FigureSet != null)
            {
                FigureSet(this, nf);
                //FigureSet(this, EventArgs.Empty, nf);
            }

        }

        /*Dessine la figure DONNEE A LA POSITION DONNEE de la grid*/
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

        /*Efface la figure DONNEE A LA POSITION DONNEE de la grid*/
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

        /*Permet de savoir si la piège fg peut être placée à la poxition [x,y]*/
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
        /*
         * Dessine la grid. L'interval est le pitch entre chaque case
         * SizeSquare la taille d'un carré
         * sizeAddToCenter est la taille qu'il faut ajouter de chaque côté pour que les bordures soient les mêmes à gauche et à droite**/
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

        /*Reset la grid, le level et le score*/
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
