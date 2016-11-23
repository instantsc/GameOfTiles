using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using GLsizei = System.Int32;
using GLuint = System.UInt32;
using GLenum = System.UInt32;
using GLint = System.Int32;
using GLbitfield = System.UInt32;
using GLsizeiptr = System.IntPtr;
using GLboolean = System.Boolean;
namespace Game
{
    public static class GL1
    {
        [DllImport("opengl32.dll", EntryPoint = "wglGetProcAddress")]
        public static extern IntPtr wglGetProcAddress(string name);

        static GL1()
        {
            var a = wglGetProcAddress("glGenVertexArrays");
            GenVertexArray = (glGenVertexArray)Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glGenVertexArrays"), typeof(glGenVertexArray));
            BindVertexArray = (glBindVertexArray)Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glBindVertexArray"), typeof(glBindVertexArray));
            GenBuffer = (glGenBuffers)Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glGenBuffers"), typeof(glGenBuffers));
            BindBuffer = (glBindBuffer)Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glBindBuffer"), typeof(glBindBuffer));
            BufferData = (glBufferData)Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glBufferData"), typeof(glBufferData));
            VertexAttribPointer = (glVertexAttribPointer)Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glVertexAttribPointer"), typeof(glVertexAttribPointer));
            EnableVertexAttribArray =
                (glEnableVertexAttribArray)
                Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glEnableVertexAttribArray"),
                    typeof(glEnableVertexAttribArray));
            UniformMatrix4 = (glUniformMatrix4fv)Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glUniformMatrix4fv"), typeof(glUniformMatrix4fv));
            CreateShader = (glCreateShader)Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glCreateShader"), typeof(glCreateShader));
            CreateProgram = (glCreateProgram)Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glCreateProgram"), typeof(glCreateProgram));
            CompileShader = (glPassShader)Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glCompileShader"), typeof(glPassShader));
            DeleteShader = (glPassShader)Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glDeleteShader"), typeof(glPassShader));
            LinkProgram = (glPassShader)Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glLinkProgram"), typeof(glPassShader));
            UseProgram = (glPassShader)Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glUseProgram"), typeof(glPassShader));
            DeleteProgram = (glPassShader)Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glDeleteProgram"), typeof(glPassShader));
            ShaderSource =
                (glShaderSource)
                Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glShaderSource"), typeof(glShaderSource));
            AttachShader =
                (glAttachShader)
                Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glAttachShader"), typeof(glAttachShader));
            DetachShader =
                (glAttachShader)
                Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glDetachShader"), typeof(glAttachShader));
            GetUniformLocation =
                (glGetUniformLocation)
                Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glGetUniformLocation"),
                    typeof(glGetUniformLocation));
        }
        public delegate void glGenVertexArray(int size, out uint arrays);
        public static glGenVertexArray GenVertexArray;

        public delegate void glBindVertexArray(uint arrays);
        public static glBindVertexArray BindVertexArray;

        public delegate void glGenBuffers(GLsizei n, ref GLuint buffers);
        public static glGenBuffers GenBuffer;

        public delegate void glBindBuffer(GLenum target, GLuint buffer);
        public static glBindBuffer BindBuffer;

        public delegate void glUniformMatrix4fv(GLint location, GLsizei count, GLboolean transpose, ref Matrix4 value);

        public static glUniformMatrix4fv UniformMatrix4;
        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ReleaseDC(IntPtr hwnd, IntPtr dc);

        public static IntPtr CreateContext(IntPtr handle)
        {
            var dc = GetDC(handle);
            var a = CreateContext1(dc);
            ReleaseDC(handle, dc);
            return a;
        }
        [DllImport("gdi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SwapBuffers(IntPtr dc);
        [SuppressUnmanagedCodeSecurity]
        [DllImport("opengl32.dll", EntryPoint = "wglCreateContext", ExactSpelling = true)]
        static extern IntPtr CreateContext1(IntPtr c);
        [SuppressUnmanagedCodeSecurity]
        [DllImport("opengl32.dll", EntryPoint = "wglMakeCurrent", ExactSpelling = true)]
        public static extern bool MakeCurrent(IntPtr a, IntPtr b);

        public delegate GLint glGetUniformLocation(GLuint program, string name);

        public static glGetUniformLocation GetUniformLocation;
        [DllImport("opengl32.dll", EntryPoint = "glClearColor")]
        public static extern void ClearColor(float red, float green, float blue, float alpha);

        [DllImport("opengl32.dll", EntryPoint = "glClear")]
        public static extern void Clear(GLbitfield mask);

        [DllImport("opengl32.dll", EntryPoint = "glDrawArrays")]
        public static extern void DrawArrays(GLenum mode, GLint first, GLsizei count);

        public delegate void glEnableVertexAttribArray(GLuint index);

        public static glEnableVertexAttribArray EnableVertexAttribArray;

        public delegate void glBufferData(GLenum target, GLsizeiptr size, Vector3[] data, GLenum usage);

        public static glBufferData BufferData;

        public delegate void glVertexAttribPointer(
            GLuint index, GLint size, GLenum type, GLboolean normalized, GLsizei stride, IntPtr pointer);

        public static glVertexAttribPointer VertexAttribPointer;

        public delegate uint glCreateShader(GLenum type);

        public static glCreateShader CreateShader;

        public delegate uint glCreateProgram();
        public static glCreateProgram CreateProgram;

        public delegate void glPassShader(uint shader);

        public static glPassShader CompileShader;
        public static glPassShader DeleteShader;
        public static glPassShader LinkProgram;
        public static glPassShader UseProgram;
        public static glPassShader DeleteProgram;


        public delegate void glShaderSource(uint shader, GLsizei count, string[] source, GLint[] length);

        public static glShaderSource ShaderSource;

        public delegate void glAttachShader(uint program, uint shader);

        public static glAttachShader AttachShader;
        public static glAttachShader DetachShader;

    }
    public class ShaderProgram : IDisposable
    {
        public uint ProgramID { get; }
        public ShaderProgram(string vertexPath, string fragPath)
        {
            var vID = GL1.CreateShader((uint)ShaderType.VertexShader);
            var fID = GL1.CreateShader((uint)ShaderType.FragmentShader);
            var vertexText = File.ReadAllText(vertexPath);
            var fragText = File.ReadAllText(fragPath);
            GL1.ShaderSource(vID, 1, new[] { vertexText }, new[] { vertexText.Length });
            GL1.ShaderSource(fID, 1, new[] { fragText }, new[] { fragText.Length });
            GL1.CompileShader(vID);
            GL1.CompileShader(fID);
            ProgramID = GL1.CreateProgram();
            GL1.AttachShader(ProgramID, vID);
            GL1.AttachShader(ProgramID, fID);
            GL1.LinkProgram(ProgramID);
            GL1.DetachShader(ProgramID, vID);
            GL1.DetachShader(ProgramID, fID);
            GL1.DeleteShader(vID);
            GL1.DeleteShader(fID);
        }
        public void Use()
        {
            GL1.UseProgram(ProgramID);
        }
        public void Dispose()
        {
            GL1.UseProgram(0);
            GL1.DeleteProgram(ProgramID);
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
            GL1.GenVertexArray(1, out VAOId);
            GL1.BindVertexArray(VAOId);

            GL1.GenBuffer(1, ref VBOId);

            GL1.GenBuffer(1, ref VBOId2);

            GL1.BindBuffer(0x8892, VBOId); //////////////

            ShaderProgram = new ShaderProgram("shaders\\vertex.glsl", "shaders\\fragment.glsl");
            ShaderProgram.Use();
            matrixLocation = GL1.GetUniformLocation(ShaderProgram.ProgramID, "MVP");

            GL1.ClearColor((float)1, (float)1, (float)1, (float)0);
            GL1.Clear(0x00000100 | 0x00004000); /////
        }



        private static void DrawArray(Vector3[] pos, Vector3[] color, PrimitiveType type, bool texture = false)
        {
            if (pos != null)
            {
                GL1.EnableVertexAttribArray(0);
                GL1.BindBuffer((uint)BufferTarget.ArrayBuffer, VBOId);
                GL1.BufferData((uint)BufferTarget.ArrayBuffer, (IntPtr)(Vector3.SizeInBytes * pos.Length), pos, (uint)BufferUsageHint.DynamicDraw);
                GL1.VertexAttribPointer(0, 3, (uint)VertexAttribPointerType.Float, false, Vector3.SizeInBytes, (IntPtr)0);
            }
            if (color != null)
            {
                GL1.EnableVertexAttribArray(1);
                GL1.BindBuffer((uint)BufferTarget.ArrayBuffer, VBOId2);
                GL1.BufferData((uint)BufferTarget.ArrayBuffer, (IntPtr)(Vector3.SizeInBytes * color.Length), color, (uint)BufferUsageHint.StreamDraw);
                GL1.VertexAttribPointer(1, 3, (uint)VertexAttribPointerType.Float, false, Vector3.SizeInBytes, (IntPtr)0);
            }
            GL1.DrawArrays((uint)type, 0, color.Length);
            GL1.BindBuffer((uint)BufferTarget.ArrayBuffer, 0);
        }
        private static uint VAOId;
        private static uint VBOId;
        private static uint VBOId2;
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
        public static void DrawWorld(World w, Rectangle view, IntPtr hwnd, IntPtr glContext, params Unit[] u)
        {
            var dc = GL1.GetDC(hwnd);
            GL1.MakeCurrent(dc, glContext);
            GL1.Clear((uint)(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit));
            var mat = Matrix4.CreateOrthographicOffCenter((float)view.Left, (float)view.Right, (float)view.Bottom, (float)view.Top, -1, 1);
            GL1.UniformMatrix4(matrixLocation, 1, false, ref mat);
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
            GL1.SwapBuffers(dc);
            GL1.MakeCurrent((IntPtr)0, (IntPtr)0);
            GL1.ReleaseDC(hwnd, dc);
        }
    }
}
