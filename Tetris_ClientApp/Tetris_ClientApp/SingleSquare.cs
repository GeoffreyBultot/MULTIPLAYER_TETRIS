using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace Tetris_ClientApp
{
    class SingleSquare
	{
		#region Constructor

		public SingleSquare(TetrisGrid tg)
        {
            parent = tg;
            rect = Rectangle.Empty;
        }


        #endregion

        #region Methods

        internal void Draw(Graphics g)
        {
            Rectangle r = rect;
            r.Width -= 2;
            r.Height -= 2;

            SolidBrush brush = new SolidBrush(color);

            g.FillRectangle(brush, r.Left + 1, r.Top + 1, r.Width - 1, r.Height - 1);

            g.DrawLine(new Pen(SystemColors.Control), r.Left, r.Bottom, r.Left, r.Top);
            g.DrawLine(new Pen(SystemColors.Control), r.Left, r.Top, r.Right, r.Top);
            g.DrawLine(new Pen(SystemColors.Control), r.Right, r.Top, r.Right, r.Bottom);
            g.DrawLine(new Pen(SystemColors.Control), r.Right, r.Bottom, r.Left, r.Bottom);

            brush.Dispose();
        }

        #endregion

        #region Fields

        internal Rectangle rect;
        internal bool filled;
        internal Color color;
        internal TetrisGrid parent;

        #endregion
    }
}