using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private World _w;
        private bool _init;
        public MainWindow()
        {
            _init = false;
            
            InitializeComponent();
        }
        private void OpenGLInit()
        {
            GL.ClearColor(Color4.White);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            _init = true;
            glControl_Resize(this, new EventArgs());
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _w = World.Generate(20, 20, 0);

            OpenGLInit();
        }

        private void glControl_Resize(object sender, EventArgs e)
        {
            if (_init)
            {
                GL.Viewport(0, 0, GlControl.Width, GlControl.Height);
                GL.MatrixMode(MatrixMode.Projection);
                Matrix4d mat = Matrix4d.CreateOrthographicOffCenter(0, _w.Field.Width, _w.Field.Height, 0, -10, 10);
                GL.LoadMatrix(ref mat);
                GlControl.Invalidate();
            }
        }
        Stopwatch sw = new Stopwatch();

        private void glControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (_init)
            {
                sw.Start();
                GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
                Graphics.DrawWorld(_w,_w.Units.First());
                GlControl.SwapBuffers();
                sw.Stop();
                mainWindow.Title = sw.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture);
                sw.Reset();
            }
        }
    }
}
