using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HomeWork_1_Aziz_Gasimov_CLGHGW

{   //HomeWork 1 
    //Hw.: implement Binom  with the Multiplicative formula 
    //https://en.wikipedia.org/wiki/Binomial_coefficient

    public partial class Form1 : Form
    {
        Graphics g;

        Color colorControl = Color.Black;
        Color colorBezier = Color.Blue;
        Color colorBSpline = Color.Red;
        List<PointF> P = new List<PointF>();
        int grab = -1;

        public Form1()
        {
            InitializeComponent();
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;

            DrawBezier(new Pen(colorBezier, 3f), P);

            for (int i = 0; i < P.Count; i++)
                g.FillRectangle(new SolidBrush(colorControl), P[i].X - 5, P[i].Y - 5, 10, 10);
            for (int i = 0; i < P.Count - 1; i++)
                g.DrawLine(new Pen(colorControl), P[i], P[i + 1]);
        }

        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < P.Count; i++)
            {
                if (IsGrab(P[i], e.Location))
                    grab = i;
            }

            if (grab == -1)
            {
                P.Add(e.Location);
                grab = P.Count - 1;
                canvas.Invalidate();
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (grab != -1)
            {
                P[grab] = e.Location;
                canvas.Refresh();
            }
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            grab = -1;
        }
        private bool IsGrab(PointF p, PointF mouseLocation)
        {
            return p.X - 5 <= mouseLocation.X && mouseLocation.X <= p.X + 5 &&
                   p.Y - 5 <= mouseLocation.Y && mouseLocation.Y <= p.Y + 5;
        }

        //Implement the Pow as a Multiplicative formula
        private float B(int i, int n, float t)
        { 
            return BinomMulti(n, i) * (float)Math.Pow(1 - t, n - i) * (float)Math.Pow(t, i); 
        }
        //didnt work yet!
        private float power(float number, float degree)
        {
            if (degree == 0)
                return 1;
            else if (degree % 2 == 0)
                return power(number, degree / 2) * power(number, degree / 2);
            else
                return number * power(number, degree / 2) * power(number, degree / 2);
        }

        private int BinomMulti(int n, int k)
        {
            int res = 1;
            for (int i = 1; i <= k; i++)
            {
                res = res * (n + 1 - i) / i;
            }
            return res;
        }

        /*private int BinomRecursive(int n, int k)
        {
            if (k == 0) return 1;
            if (k == n) return 1;
            if (n == 0) return 0;
            return Binom(n - 1, k - 1) + Binom(n - 1, k);
        }*/
      
        private void DrawBezier(Pen pen, List<PointF> P)
        {
            float a = 0f;
            float t = a;
            float h = 1.0f / 500.0f;
            PointF d0, d1;
            int deg = P.Count - 1;
            d0 = new PointF(0, 0);
            for (int i = 0; i < P.Count; i++)
            {
                d0.X += B(i, deg, t) * P[i].X;
                d0.Y += B(i, deg, t) * P[i].Y;
            }
            while (t < 1)
            {
                t += h;
                d1 = new PointF(0, 0);
                for (int i = 0; i < P.Count; i++)
                {
                    d1.X += B(i, deg, t) * P[i].X;
                    d1.Y += B(i, deg, t) * P[i].Y;
                }
                g.DrawLine(pen, d0, d1);
                d0 = d1;
            }
        }
    }
}
