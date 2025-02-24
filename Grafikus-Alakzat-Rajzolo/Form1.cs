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
        private int currentSides = 3;    // Kezdetben háromszöget rajzolunk
        private float opacity = 0.0f;     // Kezdő átlátszóság, hogy halványan jelenjen meg
        private Timer timer;
        private int frameCounter = 0;    // Framelépések száma
        private int maxSides = 12;       // Maximális oldalszám (például 12 oldalú sokszög)
        private bool fadingIn = true;

        public Form1()
        {
            InitializeComponent();

            timer = new Timer();
            timer.Interval = 100; // 100 ms (0.1 másodpercenként) történik az animáció
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // A kép mérete
            int width = pictureBox2.Width;
            int height = pictureBox2.Height;

            // Kép készítése a PictureBox-on
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);  // Minden animációs lépésnél töröljük a képet

            // A szín beállítása (halványodás az átlátszóság segítségével)
            Color color = Color.FromArgb((int)(opacity * 255), Color.Black); // Piros szín
            Brush brush = new SolidBrush(color);

            // Poligon csúcsainak kiszámítása
            PointF[] points = CalculatePolygonPoints(currentSides, width, height);

            // Sokszög kirajzolása
            g.FillPolygon(brush, points);  // A sokszög kitöltése
            g.DrawPolygon(Pens.White, points);   // A sokszög körvonalának megrajzolása

            // A képet a PictureBox-ra állítjuk
            pictureBox2.Image = bmp;

            // Ha megjelenik (fadingIn == true), növeljük az átlátszóságot, ha eltűnik (fadingIn == false), csökkentjük
            if (fadingIn)
            {
                opacity += 0.05f;  // Növeljük az átlátszóságot
                if (opacity >= 1.0f)  // Ha teljesen látható, akkor váltunk az eltűnésre
                {
                    fadingIn = false;
                }
            }
            else
            {
                opacity -= 0.05f;  // Csökkentjük az átlátszóságot
                if (opacity <= 0)  // Ha teljesen eltűnt, új sokszöget rajzolunk
                {
                    if (frameCounter < 3) // Csak három sokszöget jelenítünk meg
                    {
                        frameCounter++;  // Következő sokszög
                        opacity = 0.0f;  // Új sokszög indul átlátszóként
                        currentSides++;  // Növeljük az oldalszámot
                        fadingIn = true;  // A következő sokszög újra meg fog jelenni
                    }
                    else
                    {
                        timer.Stop(); // Animáció befejezése
                    }
                }
            }
        }
        private PointF[] CalculatePolygonPoints(int sides, int width, int height)
        {
            // A sokszög középpontja
            PointF center = new PointF(width / 2, height / 2);

            // A sugár fixálása, hogy a méret ne változzon
            float radius = Math.Min(width, height) / 3;

            // Csúcsok kiszámítása
            PointF[] points = new PointF[sides];
            for (int i = 0; i < sides; i++)
            {
                // Az egyes szögeket a középpont körül egyenletesen osztjuk el
                float angle = (float)(i * 2 * Math.PI / sides); // Szög az egyes csúcsokhoz
                points[i] = new PointF(
                    center.X + radius * (float)Math.Cos(angle),
                    center.Y + radius * (float)Math.Sin(angle)
                );
            }
            return points;
        }
    }
}
