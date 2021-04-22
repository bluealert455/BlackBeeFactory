using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tests
{
    public partial class frmRectTest : Form
    {
        public frmRectTest()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            RoundingAndTruncatingRectangles(e);
            RectangleInflateTest2(e);
        }

        public void RectangleInflateTest2(PaintEventArgs e)
        {

            // Create a rectangle.
            Rectangle rect = new Rectangle(300, 300, 50, 50);

            // Draw the uninflated rectangle to screen.
            e.Graphics.DrawRectangle(Pens.Black, rect);

            // Set up the inflate size.
            Size inflateSize = new Size(50, 50);

            // Call Inflate.
            rect.Inflate(inflateSize);

            // Draw the inflated rectangle to screen.
            e.Graphics.DrawRectangle(Pens.Red, rect);
        }
        private void RoundingAndTruncatingRectangles(PaintEventArgs e)
        {

            // Construct a new RectangleF.
            RectangleF myRectangleF =
                new RectangleF(30.6F, 30.7F, 100.8F, 200.9F);

            // Call the Round method.
            Rectangle roundedRectangle = Rectangle.Round(myRectangleF);

            // Draw the rounded rectangle in red.
            Pen redPen = new Pen(Color.Red,1);
            e.Graphics.DrawRectangle(redPen, roundedRectangle);

            // Call the Truncate method.
            Rectangle truncatedRectangle = Rectangle.Truncate(myRectangleF);

            // Draw the truncated rectangle in white.
            Pen whitePen = new Pen(Color.Black,1);
            e.Graphics.DrawRectangle(whitePen, truncatedRectangle);

            Pen bluePen = new Pen(Color.Orange, 1);
            e.Graphics.DrawRectangle(bluePen, myRectangleF.X, myRectangleF.Y, myRectangleF.Width, myRectangleF.Height);


            // Dispose of the custom pens.
            redPen.Dispose();
            whitePen.Dispose();
            bluePen.Dispose();
        }
    }
}
