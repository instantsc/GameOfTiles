using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Windows.Threading;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace Game
{


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private World _w;
        private bool _init;
        private Timer _drawTimer;
        public MainWindow()
        {
            _init = false;
            InitializeComponent();
        }
        private void OpenGLInit()
        {
            Graphics.Init();
            _init = true;
            glControl_Resize(this, new EventArgs());
        }
        private Rectangle view;
        int width = 100;
        int height = 100;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _drawTimer = new Timer();
            _drawTimer.Elapsed += DrawTimer_Tick;
            _drawTimer.Interval = 10;
            _drawTimer.Start();
            view = new Rectangle(0, 0, width, height);
            _w = World.Generate(width, height, 1);
            OpenGLInit();

        }

        private void DrawTimer_Tick(object sender, EventArgs e)
        {
            GlControl.Invalidate();
        }

        private void glControl_Resize(object sender, EventArgs e)
        {
            if (_init)
            {
                GL.Viewport(0, 0, GlControl.Width, GlControl.Height);
            }
        }
        private readonly Stopwatch _sw = new Stopwatch();
        private void glControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (_init)
            {
                _sw.Start();
                GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
                Graphics.DrawWorld(_w, _w.Units.First(), view);
                GlControl.SwapBuffers();
                _sw.Stop();
                mainWindow.Title = $"{_sw.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture)}";
                _sw.Reset();
            }
        }
        private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var unit = _w.Units.First();
            switch (e.Key)
            {
                case Key.Left:
                    {
                        if (unit.X > 0 && _w.Field[unit.Position.MoveXDown].Passable)
                            unit.X--;
                        break;
                    }
                case Key.Right:
                    {
                        if (unit.X < _w.Field.Width - 1 && _w.Field[unit.Position.MoveXUp].Passable)
                            unit.X++;
                        break;
                    }
                case Key.Up:
                    {
                        if (unit.Y > 0 && _w.Field[unit.Position.MoveYDown].Passable)
                            unit.Y--;
                        break;
                    }
                case Key.Down:
                    {
                        if (unit.Y < _w.Field.Height - 1 && _w.Field[unit.Position.MoveYUp].Passable)
                            unit.Y++;
                        break;
                    }
            }
        }
        private static double MultRel(double boundary, double pos, double k)
        {
            return pos - k * (pos - boundary);
        }
        Rectangle Refit(Rectangle r)
        {
            if (r.Right - r.Left > width)
            {
                r.Right = width;
                r.Left = 0;
            }
            else
            {
                if (r.Left < 0)
                {
                    r.Right = r.Right - r.Left;
                    r.Left = 0;
                }
                if (r.Right > width)
                {
                    r.Left = r.Left - (r.Right - width);
                    r.Right = width;
                }
            }
            if (r.Bottom - r.Top > height)
            {
                r.Bottom = height;
                r.Top = 0;
            }
            else
            {
                if (r.Top < 0)
                {
                    r.Bottom = r.Bottom - r.Top;
                    r.Top = 0;
                }
                if (r.Bottom > height)
                {
                    r.Top = r.Top - (r.Bottom - height);
                    r.Bottom = height;
                }
            }
            return r;
        }
        private void GlControl_OnMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            double k = e.Delta > 0 ? 0.95 : 1 / 0.95; //zoom coefficient
            double px = view.Left + view.Width * e.X / GlControl.Width;
            double py = view.Top + view.Height * e.Y / GlControl.Height;
            double left = MultRel(view.Left, px, k);
            double right = MultRel(view.Right, px, k);

            double top = MultRel(view.Top, py, k);
            double bottom = MultRel(view.Bottom, py, k);

            view = Refit(new Rectangle(left, top, right, bottom));
        }
        private Point? loc = null;
        private void GlControl_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (loc != null)
            {
                Mouse.Capture(FormHost);
                var diff = Mouse.GetPosition(FormHost) - loc.Value;
                loc = Mouse.GetPosition(FormHost);
                Mouse.Capture(null);
                diff.X *= view.Width / FormHost.ActualWidth;
                diff.Y *= view.Height / FormHost.ActualHeight;
                double left = view.Left - diff.X;
                double right = view.Right - diff.X;

                double top = view.Top - diff.Y;
                double bottom = view.Bottom - diff.Y;

                view = Refit(new Rectangle(left, top, right, bottom));
                glControl_Paint(null, null);
            }
        }
        private void GlControl_OnMouseLeave(object sender, EventArgs e)
        {
            loc = null;
        }
        private void GlControl_OnMouseEnter(object sender, EventArgs e)
        {

        }
        private void GlControl_OnMouseDown(object sender, MouseEventArgs e)
        {
            Mouse.Capture(FormHost);
            loc = Mouse.GetPosition(FormHost);
            Mouse.Capture(null);
        }
        private void GlControl_OnMouseUp(object sender, MouseEventArgs e)
        {
            loc = null;
        }
    }
}
