﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;
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
        private Rectangle _view;
        int width = 100;
        int height = 100;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GL1.Init(GlControl.Handle);
            _drawTimer = new Timer();
            _drawTimer.Elapsed += DrawTimer_Tick;
            _drawTimer.Interval = 10;
            _drawTimer.Start();
            _view = new Rectangle(0, 0, width, height);
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
                GL1.Viewport(0, 0, GlControl.Width, GlControl.Height);
            }
        }
        private readonly Stopwatch _sw = new Stopwatch();
        private void glControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (_init)
            {
                _w.UpdateUnitPositions();
                if (_w.Units.First().Position == new Position(_w.Field.Width / 2, _w.Field.Height / 2))
                {
                    GlControl.Paint -= glControl_Paint;
                    MessageBox.Show("Player 1 wins");
                    Application.Current.Shutdown();
                }
                if (_w.Units.Last().Position == new Position(_w.Field.Width / 2, _w.Field.Height / 2))
                {
                    GlControl.Paint -= glControl_Paint;
                    MessageBox.Show("Player 2 wins");
                    Application.Current.Shutdown();
                }
                _sw.Start();
                Graphics.DrawWorld(_w, _view, _w.Units.ToArray());
                GL1.Flush();
                //GL1.Finish();
                GL1.SwapBuffers(GL1.DC);
                _sw.Stop();
                mainWindow.Title = $"{_sw.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture)}";
                _sw.Reset();
            }
        }
        private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var p1 = _w.Units.First();
            var p2 = _w.Units.Last();
            switch (e.Key)
            {
                case Key.Left:
                    {
                        if (p1.X > 0 && _w.Field[p1.Position.MoveXDown].Passable)
                            p1.X--;
                        break;
                    }
                case Key.Right:
                    {
                        if (p1.X < _w.Field.Width - 1 && _w.Field[p1.Position.MoveXUp].Passable)
                            p1.X++;
                        break;
                    }
                case Key.Up:
                    {
                        if (p1.Y > 0 && _w.Field[p1.Position.MoveYDown].Passable)
                            p1.Y--;
                        break;
                    }
                case Key.Down:
                    {
                        if (p1.Y < _w.Field.Height - 1 && _w.Field[p1.Position.MoveYUp].Passable)
                            p1.Y++;
                        break;
                    }
                case Key.A:
                    {
                        if (p2.X > 0 && _w.Field[p2.Position.MoveXDown].Passable)
                            p2.X--;
                        break;
                    }
                case Key.D:
                    {
                        if (p2.X < _w.Field.Width - 1 && _w.Field[p2.Position.MoveXUp].Passable)
                            p2.X++;
                        break;
                    }
                case Key.W:
                    {
                        if (p2.Y > 0 && _w.Field[p2.Position.MoveYDown].Passable)
                            p2.Y--;
                        break;
                    }
                case Key.S:
                    {
                        if (p2.Y < _w.Field.Height - 1 && _w.Field[p2.Position.MoveYUp].Passable)
                            p2.Y++;
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
        private void GlControl_OnMouseWheel(object sender, MouseEventArgs e)
        {
            double k = e.Delta > 0 ? 0.95 : 1 / 0.95; //zoom coefficient
            double px = _view.Left + _view.Width * e.X / GlControl.Width;
            double py = _view.Top + _view.Height * e.Y / GlControl.Height;
            double left = MultRel(_view.Left, px, k);
            double right = MultRel(_view.Right, px, k);

            double top = MultRel(_view.Top, py, k);
            double bottom = MultRel(_view.Bottom, py, k);

            _view = Refit(new Rectangle(left, top, right, bottom));
        }
        private Point? _loc;
        private void GlControl_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_loc != null)
            {
                Mouse.Capture(FormHost);
                var diff = Mouse.GetPosition(FormHost) - _loc.Value;
                _loc = Mouse.GetPosition(FormHost);
                Mouse.Capture(null);
                diff.X *= _view.Width / FormHost.ActualWidth;
                diff.Y *= _view.Height / FormHost.ActualHeight;
                double left = _view.Left - diff.X;
                double right = _view.Right - diff.X;

                double top = _view.Top - diff.Y;
                double bottom = _view.Bottom - diff.Y;

                _view = Refit(new Rectangle(left, top, right, bottom));
                glControl_Paint(null, null);
            }
        }
        private void GlControl_OnMouseLeave(object sender, EventArgs e)
        {
            _loc = null;
        }
        private void GlControl_OnMouseEnter(object sender, EventArgs e)
        {

        }
        private void GlControl_OnMouseDown(object sender, MouseEventArgs e)
        {
            Mouse.Capture(FormHost);
            _loc = Mouse.GetPosition(FormHost);
            Mouse.Capture(null);
        }
        private void GlControl_OnMouseUp(object sender, MouseEventArgs e)
        {
            _loc = null;
        }
    }
}
