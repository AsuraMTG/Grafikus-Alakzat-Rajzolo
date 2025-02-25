using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grafikus_Alakzat_Rajzolo
{
    public partial class Form1 : Form
    {
        private int currentSides = 3;
        private float opacity = 0.0f;
        private Timer timer;
        private int frameCounter = 0;
        private bool fadingIn = true;

        public Form1()
        {
            InitializeComponent();

            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int width = pictureBox2.Width;
            int height = pictureBox2.Height;

            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);

            Color color = Color.FromArgb((int)(opacity * 255), Color.Black);
            Brush brush = new SolidBrush(color);

            PointF[] points = CalculatePolygonPoints(currentSides, width, height);

            g.FillPolygon(brush, points);
            g.DrawPolygon(Pens.White, points);

            pictureBox2.Image = bmp;

            if (fadingIn)
            {
                opacity += 0.05f;
                if (opacity >= 1.0f)
                {
                    fadingIn = false;
                }
            }
            else
            {
                opacity -= 0.05f;
                if (opacity <= 0)
                {
                    if (frameCounter < 3)
                    {
                        frameCounter++;
                        opacity = 0.0f;
                        currentSides++;
                        fadingIn = true;
                    }
                    else
                    {
                        timer.Stop();
                    }
                }
            }
        }
        private PointF[] CalculatePolygonPoints(int sides, int width, int height)
        {
            PointF center = new PointF(width / 2, height / 2);

            float radius = Math.Min(width, height) / 3;

            PointF[] points = new PointF[sides];
            for (int i = 0; i < sides; i++)
            {
                float angle = (float)(i * 2 * Math.PI / sides);
                points[i] = new PointF(
                    center.X + radius * (float)Math.Cos(angle),
                    center.Y + radius * (float)Math.Sin(angle)
                );
            }
            return points;
        }
    }
}
