using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HomeWork_2
{
    public partial class Form1 : Form
    {
        /*
         * HomeWork: 2
         * //Private void DrawClosedBSpline(Pen pen, List<PointF> P)
         * //https://pages.mtu.edu/~shene/COURSES/cs3621/NOTES/spline/B-spline/bspline-curve-closed.html
         * i add thi step accroting to this  exaplanation 
         */
        Graphics g;

        Color colorControl = Color.Black;
        Color colorBezier = Color.Blue;
        Color colorBSpline = Color.Red;
        List<PointF> P = new List<PointF>();
        int grab = -1;
        bool showClosedBspline = false;
        bool addPoint = true;
        public Form1()
        {
            InitializeComponent();
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;

            DrawClosedBSpline(new Pen(colorBSpline, 3f), P);

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

            if (grab == -1 && addPoint)
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

        private double N0(double t) { return 1.0 / 6.0 * (1 - t) * (1 - t) * (1 - t); }
        private double N1(double t) { return 0.5 * t * t * t - t * t + 2.0 / 3.0; }
        private double N2(double t) { return -0.5 * t * t * t + 0.5 * t * t + 0.5 * t + 1.0 / 6.0; }
        private double N3(double t) { return 1.0 / 6.0 * t * t * t; }

        private void DrawBSplineArc(Pen pen, PointF p0, PointF p1, PointF p2, PointF p3)
        {
            double a = 0;
            double t = a;
            double h = 1.0 / 500.0;
            PointF d0, d1;
            d0 = new PointF((float)(N0(t) * p0.X + N1(t) * p1.X + N2(t) * p2.X + N3(t) * p3.X),
                            (float)(N0(t) * p0.Y + N1(t) * p1.Y + N2(t) * p2.Y + N3(t) * p3.Y));
            while (t < 1)
            {
                t += h;
                d1 = new PointF((float)(N0(t) * p0.X + N1(t) * p1.X + N2(t) * p2.X + N3(t) * p3.X),
                                (float)(N0(t) * p0.Y + N1(t) * p1.Y + N2(t) * p2.Y + N3(t) * p3.Y));
                g.DrawLine(pen, d0, d1);
                d0 = d1;
            }
        }
        private void DrawClosedBSpline(Pen pen, List<PointF> P)
        {
            for (int i = 0; i < P.Count-3; ++i)
            {   
                if(i <= 3)
                {
                    DrawBSplineArc(pen, P[i], P[i + 1], P[i + 2], P[i + 3]);
                }
                   
                
                if (i == 5)
                {
                    DrawBSplineArc(pen, P[i - 2], P[i - 1], P[i], P[0]);
                    DrawBSplineArc(pen, P[i - 1], P[i], P[0], P[1]);
                    DrawBSplineArc(pen, P[i], P[0], P[1], P[2]);
                    //https://pages.mtu.edu/~shene/COURSES/cs3621/NOTES/spline/B-spline/bspline-curve-closed.html
                    // i add thi step accroting to this  exaplanation 
                    P[P.Count - 3] = P[0];
                    P[P.Count - 2] = P[1];
                    P[P.Count - 1] = P[2];
                    addPoint = false;
                    break;
                }
            }

 
        }

        private void closedBs_Click(object sender, EventArgs e)
        {
        }
    }
}
