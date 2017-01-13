using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BresenhamLines
{
    public partial class Form1 : Form
    {

        DirectBitmap layer1;
        DirectBitmap layer2;

        public Form1()
        {
            InitializeComponent();
            layer1 = new DirectBitmap(20, 20);
            layer2 = new DirectBitmap(ClientRectangle.Width, ClientRectangle.Height);
            layer1.clear(0xffffffff);
            layer2.clear(0x00000000);
            layer2.drawGrid(layer1.width, layer1.height);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            layer1.Dispose();
            layer2.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            
            int x1 = rnd.Next(0, layer1.width);
            int x2 = rnd.Next(0, layer1.width);
            int y1 = rnd.Next(0, layer1.height);
            int y2 = rnd.Next(0, layer1.height);

            double widthRatio = ((layer2.width / (double)layer1.width))/2.0;
            double heightRatio = ((layer2.height / (double)layer1.height))/2.0;

            layer1.clear(0xffffffff);
            layer2.clear(0);
            layer2.drawGrid(layer1.width, layer1.height);

            layer1.drawBresenhamLine(x1, y1, x2, y2, 0xff0000ff);
            layer2.drawBresenhamLine(
                (int)((2*x1+1) * widthRatio),
                (int)((2*y1+1) * heightRatio),
                (int)((2*x2+1) * widthRatio),
                (int)((2*y2+1) * heightRatio),
                0xffff0000);

            this.Refresh();
            //takeScreenshot("Bresenham1.bmp");
        }

        private void takeScreenshot(String fileName)
        {
            Bitmap b = new Bitmap(Width, Height);
            DrawToBitmap(b, new Rectangle(0, 0, Width, Height));
            b.Save(fileName);
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            g.DrawImage(layer1.bmp, 0, 0, ClientRectangle.Width, ClientRectangle.Height);
            g.DrawImage(layer2.bmp, 0, 0, ClientRectangle.Width, ClientRectangle.Height);
            
            g.Dispose();
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            layer2.Dispose();
            layer2 = new DirectBitmap(ClientRectangle.Width, ClientRectangle.Height);
            layer2.clear(0);
            layer2.drawGrid(layer1.width, layer1.height);
            this.Refresh();
        }
    }
}
