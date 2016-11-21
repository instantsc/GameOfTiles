using System;
using System.IO;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
namespace Game
{
    public class ShaderProgram : IDisposable
    {
        public int ProgramID { get; }
        public ShaderProgram(string vertexPath, string fragPath)
        {
            var vID = GL.CreateShader(ShaderType.VertexShader);
            var fID = GL.CreateShader(ShaderType.FragmentShader);
            var vertexText = File.ReadAllText(vertexPath);
            var fragText = File.ReadAllText(fragPath);
            GL.ShaderSource(vID, vertexText);
            GL.ShaderSource(fID, fragText);
            GL.CompileShader(vID);
            GL.CompileShader(fID);
            ProgramID = GL.CreateProgram();
            GL.AttachShader(ProgramID, vID);
            GL.AttachShader(ProgramID, fID);
            GL.LinkProgram(ProgramID);
            GL.DetachShader(ProgramID, vID);
            GL.DetachShader(ProgramID, fID);
            GL.DeleteShader(vID);
            GL.DeleteShader(fID);
            var a = GL.GetProgramInfoLog(ProgramID);
        }
        public void Use()
        {
            GL.UseProgram(ProgramID);
        }
        public void Dispose()
        {
            GL.UseProgram(0);
            GL.DeleteProgram(ProgramID);
        }
    }

    internal struct Rectangle
    {
        public Rectangle(double left, double top, double right, double bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }
        public double Top, Bottom, Left, Right;
        public double Width => Right - Left;
        public double Height => Bottom - Top;
    }

    internal static class Graphics
    {
        public static ShaderProgram ShaderProgram;
        private static bool sceneShaped;
        private static int matrixLocation;
        public static void Init()
        {
            sceneShaped = false;
            VAOId = GL.GenVertexArray();
            GL.BindVertexArray(VAOId);
            VBOId = GL.GenBuffer();
            VBOId2 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOId);
            ShaderProgram = new ShaderProgram("shaders\\vertex.glsl", "shaders\\fragment.glsl");
            ShaderProgram.Use();
            matrixLocation = GL.GetUniformLocation(ShaderProgram.ProgramID, "MVP");
            GL.ClearColor(Color4.White);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
        }
        private static void DrawArray(Vector3[] pos, Vector3[] color, PrimitiveType type, bool texture = false)
        {
            if (pos != null)
            {
                GL.EnableVertexAttribArray(0);
                GL.BindBuffer(BufferTarget.ArrayBuffer, VBOId);
                GL.BufferData(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * pos.Length, pos, BufferUsageHint.DynamicDraw);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
            }
            if (color != null)
            {
                GL.EnableVertexAttribArray(1);
                GL.BindBuffer(BufferTarget.ArrayBuffer, VBOId2);
                GL.BufferData(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * color.Length, color, BufferUsageHint.StreamDraw);
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
            }
            GL.DrawArrays(type, 0, color.Length);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        private static int VAOId;
        private static int VBOId;
        private static int VBOId2;
        private static Vector3[] _poses;
        private static Vector3[] _colors;

        private static int _iii;

        private static void UpdateTile(int s, bool passable, ref Vector3 c1, ref Vector3 c2)
        {
            if (passable)
            {

                for (var i = s; i < s + 3; i++)
                {
                    _colors[i] = c1;
                }
                for (var i = s + 3; i < s + 6; i++)
                {
                    _colors[i] = c2;
                }
            }
        }
        private static void ShapeTile(int x, int y, bool passable = true)
        {
            var vert1 = new Vector3(x, y, 0);
            var vert2 = new Vector3(x + 1, y, 0);
            var vert3 = new Vector3(x + 1, y + 1, 0);
            var vert4 = new Vector3(x, y + 1, 0);

            if (passable)
            {
                _poses[_iii++] = vert1;
                _poses[_iii++] = vert2;
                _poses[_iii++] = vert4;
                _poses[_iii++] = vert2;
                _poses[_iii++] = vert3;
                _poses[_iii++] = vert4;
            }
            else
            {
                _poses[_iii++] = vert1;
                _poses[_iii++] = vert2;
                _poses[_iii++] = vert4;
                _poses[_iii++] = vert2;
                _poses[_iii++] = vert3;
                _poses[_iii++] = vert4;
            }
        }
        public static void DrawWorld(World w, Rectangle view, params Unit[] u)
        {
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            var mat = Matrix4.CreateOrthographicOffCenter((float)view.Left, (float)view.Right, (float)view.Bottom, (float)view.Top, -1, 1);
            GL.UniformMatrix4(matrixLocation, false, ref mat);
            if (!sceneShaped)
            {
                _poses = new Vector3[w.Field.Height * w.Field.Height * 2 * 3];
                _colors = new Vector3[w.Field.Height * w.Field.Height * 2 * 3];
            }
            var visionmap = new bool[w.Field.Width, w.Field.Height];
            foreach (var unit in u)
            {
                var tmp = w.Vision.VisionField(unit);
                for (int i = 0; i < w.Field.Width; i++)
                {
                    for (int j = 0; j < w.Field.Height; j++)
                    {
                        visionmap[i, j] = visionmap[i, j] || tmp[i, j];
                    }
                }
            }
            if (!sceneShaped)
            {
                for (var i = 0; i < w.Field.Width; i++)
                {
                    for (var j = 0; j < w.Field.Height; j++)
                    {
                        ShapeTile(i, j, w.Field[i, j].Passable);
                    }
                }
                DrawArray(_poses, _colors, PrimitiveType.Triangles);
                sceneShaped = true;
            }

            var cVisible = new Vector3(0, 0, 1);
            var cDefault = new Vector3(1, 1, 1);
            var cPlayer = new Vector3(0, 1, 0);
            var cEnemy = new Vector3(1, 0, 0);
            Parallel.For(0, w.Field.Width, i =>
              {
                  for (var j = 0; j < w.Field.Height; j++)
                  {
                      if (visionmap[i, j])
                      {
                          if (w.Field[i, j].Unit != null)
                          {
                              UpdateTile(6 * (i * w.Field.Height + j), w.Field[i, j].Passable, ref cEnemy, ref cVisible);
                          }
                          else
                          {
                              UpdateTile(6 * (i * w.Field.Height + j), w.Field[i, j].Passable, ref cDefault, ref cVisible);
                          }
                      }
                      else
                      {
                          UpdateTile(6 * (i * w.Field.Height + j), w.Field[i, j].Passable, ref cDefault, ref cDefault);
                      }
                  }
              });
            foreach (var unit in u)
            {
                UpdateTile(6 * (unit.Position.X * w.Field.Height + unit.Position.Y), true, ref cPlayer, ref cVisible);
            }
            DrawArray(null, _colors, PrimitiveType.Triangles);

        }
    }
}
