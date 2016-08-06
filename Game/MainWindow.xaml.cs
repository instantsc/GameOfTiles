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

namespace Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private World w;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void DrawTile(DrawingContext dc, int x, int y, bool passable = true, double threat = 0, double vision = 0)
        {
            const double width = 20;
            const double height = 20;
            if (passable)
            {
                double xup = x * width;
                double yup = y * height;
                PathFigure pu = new PathFigure(new Point(xup, yup),
                    new[]
                    {
                        new LineSegment(new Point(xup + width, yup), false),
                        new LineSegment(new Point(xup, yup + height), false)
                    }, true);
                PathFigure pl = new PathFigure(new Point(xup + width, yup + height),
                    new[]
                    {
                        new LineSegment(new Point(xup + width, yup), false),
                        new LineSegment(new Point(xup, yup + height), false)
                    }, true);
                Geometry gu = new PathGeometry(new[] { pu });
                Geometry gl = new PathGeometry(new[] { pl });
                dc.DrawGeometry(new SolidColorBrush(Color.FromScRgb((float)vision, 0, 0, 1)), null, gu);
                dc.DrawGeometry(new SolidColorBrush(Color.FromScRgb((float)threat, 1, 0, 0)), null, gl);
            }
            else
            {
                double xup = x * width;
                double yup = y * height;
                PathFigure p = new PathFigure(new Point(xup, yup),
                    new[]
                    {
                        new LineSegment(new Point(xup + width, yup), false),
                        new LineSegment(new Point(xup+width, yup + height), false),
                        new LineSegment(new Point(xup, yup + height), false),
                    }, false);
                Geometry g = new PathGeometry(new[] { p });
                dc.DrawGeometry(new SolidColorBrush(Color.FromScRgb(1, 0, 0, 0)), null, g);
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            w=World.Generate(256,256,22);
            Tile start = w.Field[new Position(1, 0)], end = w.Field[new Position(255, 255)];
            Stopwatch sw=new Stopwatch();
            sw.Start();
           var path=AStar.Pather.FindPath(w, start, end);
            sw.Stop();
            mainWindow.Title = sw.ElapsedMilliseconds.ToString();
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                //dc.PushTransform(new RotateTransform(12));
                for (int i = 0; i < w.Field.Width; i++)
                {
                    for (int j = 0; j < w.Field.Height; j++)
                    {
                       
                        DrawTile(dc, i, j, w.Field[new Position(i, j)].Passable, 0.5, 1);
                            //path.Contains(w.Field[new Position(i,j)])?1:0);
                    }
                    
                }
            }

            RenderTargetBitmap r = new RenderTargetBitmap(w.Field.Width*20, w.Field.Height*20, 96, 96, PixelFormats.Default);
            r.Render(dv);
            MainImage.Source = r;
        }
    }
}
