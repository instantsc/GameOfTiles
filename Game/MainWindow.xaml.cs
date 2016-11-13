using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using System.Windows.Threading;

namespace Game
{


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private World _w;
        private bool _init;
        private DispatcherTimer drawTimer;
        public MainWindow()
        {
            _init = false;
            InitializeComponent();
        }
        private void OpenGLInit()
        {
            Graphics.Init();
            Matrix4 mat = Matrix4.CreateOrthographicOffCenter(0, _w.Field.Width, _w.Field.Height, 0, -1, 1);
            int loc = GL.GetUniformLocation(Graphics.ShaderProgram.ProgramID, "MVP");
            GL.UniformMatrix4(loc, false, ref mat);
            GL.ClearColor(Color4.White);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            _init = true;
            glControl_Resize(this, new EventArgs());
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            drawTimer=new DispatcherTimer();
            drawTimer.Tick += DrawTimer_Tick;
            drawTimer.Interval=new TimeSpan(0,0,0,0,50);
            drawTimer.Start();
            _w = World.Generate(100, 100, 1);
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
        Stopwatch sw = new Stopwatch();

        private void glControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (_init)
            {
                sw.Start();
                GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
                Graphics.DrawWorld(_w, _w.Units.First());
                GlControl.SwapBuffers();
                sw.Stop();
                mainWindow.Title = sw.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture);
                sw.Reset();
            }
        }
        private void MainWindow_OnPreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var unit = _w.Units.First();


            switch (e.Key)
            {
                case Key.Left:
                    {
                        if (unit.X>0&&_w.Field[unit.Position.MoveXDown].Passable)
                            unit.X--;
                        break;
                    }
                case Key.Right:
                    {
                        if (unit.X<_w.Field.Width-1&&_w.Field[unit.Position.MoveXUp].Passable)
                            unit.X++;
                        break;
                    }
                case Key.Up:
                    {
                        if (unit.Y>0&&_w.Field[unit.Position.MoveYDown].Passable)
                            unit.Y--;
                        break;
                    }
                case Key.Down:
                    {
                        if (unit.Y<_w.Field.Height-1&&_w.Field[unit.Position.MoveYUp].Passable)
                            unit.Y++;
                        break;
                    }
            }
        }
    }
}
