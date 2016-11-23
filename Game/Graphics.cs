using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;

namespace Game
{
    public static class GL1
    {
        public delegate void glAttachShader(uint program, uint shader);

        public delegate void glBindBuffer(uint target, uint buffer);

        public delegate void glBindVertexArray(uint arrays);

        public delegate void glBufferData(uint target, IntPtr size, Vector3[] data, uint usage);

        public delegate uint glCreateProgram();

        public delegate uint glCreateShader(uint type);

        public delegate void glEnableVertexAttribArray(uint index);

        public delegate void glGenBuffers(int n, ref uint buffers);

        public delegate void glGenVertexArray(int size, out uint arrays);

        public delegate int glGetUniformLocation(uint program, string name);

        public delegate void glPassShader(uint shader);

        public delegate void glShaderSource(uint shader, int count, string[] source, int[] length);

        public delegate void glUniformMatrix4fv(int location, int count, bool transpose, ref Matrix4 value);

        public delegate void glVertexAttribPointer(
            uint index, int size, uint type, bool normalized, int stride, IntPtr pointer);

        private static IntPtr GLContext;
        public static IntPtr DC;
        public static glGenVertexArray GenVertexArray;
        public static glBindVertexArray BindVertexArray;
        public static glGenBuffers GenBuffer;
        public static glBindBuffer BindBuffer;

        public static glUniformMatrix4fv UniformMatrix4;

        public static glGetUniformLocation GetUniformLocation;

        public static glEnableVertexAttribArray EnableVertexAttribArray;

        public static glBufferData BufferData;

        public static glVertexAttribPointer VertexAttribPointer;

        public static glCreateShader CreateShader;
        public static glCreateProgram CreateProgram;

        public static glPassShader CompileShader;
        public static glPassShader DeleteShader;
        public static glPassShader LinkProgram;
        public static glPassShader UseProgram;
        public static glPassShader DeleteProgram;

        public static glShaderSource ShaderSource;

        public static glAttachShader AttachShader;
        public static glAttachShader DetachShader;
        [DllImport("opengl32.dll", EntryPoint = "wglGetProcAddress")]
        private static extern IntPtr wglGetProcAddress(string name);
        [DllImport("gdi32.dll")]
        private static extern int ChoosePixelFormat(IntPtr dc, ref PixelFormatDescriptor pfd);
        [DllImport("opengl32.dll", EntryPoint = "glViewport", ExactSpelling = true)]
        internal static extern void Viewport(int x, int y, int width, int height);
        [DllImport("opengl32.dll", EntryPoint = "glFlush")]
        public static extern void Flush();
        [DllImport("opengl32.dll", EntryPoint = "glDrawBuffer")]
        public static extern void DrawBuffer(int buf);
        public static void Init(IntPtr hwnd)
        {
            var dc = GetDC(hwnd);
            DC = dc;
            var pfd = new PixelFormatDescriptor(null);
            var pfn = ChoosePixelFormat(dc, ref pfd);
            DescribePixelFormat(dc, pfn, (short) Marshal.SizeOf(typeof(PixelFormatDescriptor)), ref pfd);
            SetPixelFormat(dc, pfn, ref pfd);
            GLContext = CreateContext(dc);
            MakeCurrent(dc, GLContext);
            GenVertexArray =
                (glGenVertexArray)
                Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glGenVertexArrays"), typeof(glGenVertexArray));
            BindVertexArray =
                (glBindVertexArray)
                Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glBindVertexArray"), typeof(glBindVertexArray));
            GenBuffer =
                (glGenBuffers)
                Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glGenBuffers"), typeof(glGenBuffers));
            BindBuffer =
                (glBindBuffer)
                Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glBindBuffer"), typeof(glBindBuffer));
            BufferData =
                (glBufferData)
                Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glBufferData"), typeof(glBufferData));
            VertexAttribPointer =
                (glVertexAttribPointer)
                Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glVertexAttribPointer"),
                    typeof(glVertexAttribPointer));
            EnableVertexAttribArray =
                (glEnableVertexAttribArray)
                Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glEnableVertexAttribArray"),
                    typeof(glEnableVertexAttribArray));
            UniformMatrix4 =
                (glUniformMatrix4fv)
                Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glUniformMatrix4fv"),
                    typeof(glUniformMatrix4fv));
            CreateShader =
                (glCreateShader)
                Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glCreateShader"), typeof(glCreateShader));
            CreateProgram =
                (glCreateProgram)
                Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glCreateProgram"), typeof(glCreateProgram));
            CompileShader =
                (glPassShader)
                Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glCompileShader"), typeof(glPassShader));
            DeleteShader =
                (glPassShader)
                Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glDeleteShader"), typeof(glPassShader));
            LinkProgram =
                (glPassShader)
                Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glLinkProgram"), typeof(glPassShader));
            UseProgram =
                (glPassShader)
                Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glUseProgram"), typeof(glPassShader));
            DeleteProgram =
                (glPassShader)
                Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glDeleteProgram"), typeof(glPassShader));
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
        [DllImport("User32.dll")]
        private static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("gdi32.dll")]
        private static extern int DescribePixelFormat(IntPtr deviceContext, int pixel, int pfdSize,
            ref PixelFormatDescriptor pixelFormat);
        [DllImport("gdi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetPixelFormat(IntPtr dc, int format, ref PixelFormatDescriptor pfd);
        [DllImport("gdi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SwapBuffers(IntPtr dc);
        [SuppressUnmanagedCodeSecurity]
        [DllImport("opengl32.dll", EntryPoint = "wglCreateContext", ExactSpelling = true)]
        private static extern IntPtr CreateContext(IntPtr c);
        [SuppressUnmanagedCodeSecurity]
        [DllImport("opengl32.dll", EntryPoint = "wglMakeCurrent", ExactSpelling = true)]
        private static extern bool MakeCurrent(IntPtr dc, IntPtr glcontext);
        [DllImport("opengl32.dll", EntryPoint = "glClearColor")]
        public static extern void ClearColor(float red, float green, float blue, float alpha);

        [DllImport("opengl32.dll", EntryPoint = "glClear")]
        public static extern void Clear(uint mask);

        [DllImport("opengl32.dll", EntryPoint = "glDrawArrays")]
        public static extern void DrawArrays(uint mode, int first, int count);
    }

    public enum ShaderType
    {
        /// <summary>
        ///     Original was GL_FRAGMENT_SHADER = 0x8B30
        /// </summary>
        FragmentShader = 0x8B30,

        /// <summary>
        ///     Original was GL_VERTEX_SHADER = 0x8B31
        /// </summary>
        VertexShader = 0x8B31
    }

    public class ShaderProgram : IDisposable
    {
        public ShaderProgram(string vertexPath, string fragPath)
        {
            var vID = GL1.CreateShader((uint) ShaderType.VertexShader);
            var fID = GL1.CreateShader((uint) ShaderType.FragmentShader);
            var vertexText = File.ReadAllText(vertexPath);
            var fragText = File.ReadAllText(fragPath);
            GL1.ShaderSource(vID, 1, new[] {vertexText}, new[] {vertexText.Length});
            GL1.ShaderSource(fID, 1, new[] {fragText}, new[] {fragText.Length});
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
        public uint ProgramID { get; }
        public void Dispose()
        {
            GL1.UseProgram(0);
            GL1.DeleteProgram(ProgramID);
        }
        public void Use()
        {
            GL1.UseProgram(ProgramID);
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
        private static ShaderProgram ShaderProgram;
        private static bool sceneShaped;
        private static int matrixLocation;
        private static uint VAOId;
        private static uint VBOId;
        private static uint VBOId2;
        private static Vector3[] _poses;
        private static Vector3[] _colors;

        private static int _iii;
        public static void Init()
        {
            sceneShaped = false;
            GL1.GenVertexArray(1, out VAOId);
            GL1.BindVertexArray(VAOId);

            GL1.GenBuffer(1, ref VBOId);

            GL1.GenBuffer(1, ref VBOId2);

            GL1.BindBuffer(0x8892, VBOId); 

            ShaderProgram = new ShaderProgram("shaders\\vertex.glsl", "shaders\\fragment.glsl");
            ShaderProgram.Use();
            matrixLocation = GL1.GetUniformLocation(ShaderProgram.ProgramID, "MVP");

            GL1.ClearColor(0.5f, 0, 1, 0);

            GL1.Clear(0x00000100 | 0x00004000); 
        }
        private static void DrawArray(Vector3[] pos, Vector3[] color, PrimitiveType type)
        {
            if (pos != null)
            {
                GL1.EnableVertexAttribArray(0);
                GL1.BindBuffer(0x8892, VBOId);
                GL1.BufferData(0x8892, (IntPtr) (Vector3.SizeInBytes*pos.Length), pos,
                    (uint) BufferUsageHint.DynamicDraw);
                GL1.VertexAttribPointer(0, 3, (uint) VertexAttribPointerType.Float, false, Vector3.SizeInBytes,
                    (IntPtr) 0);
            }
            if (color != null)
            {
                GL1.EnableVertexAttribArray(1);
                GL1.BindBuffer(0x8892, VBOId2);
                GL1.BufferData(0x8892, (IntPtr) (Vector3.SizeInBytes*color.Length), color,
                    (uint) BufferUsageHint.StreamDraw);
                GL1.VertexAttribPointer(1, 3, (uint) VertexAttribPointerType.Float, false, Vector3.SizeInBytes,
                    (IntPtr) 0);
            }
            GL1.DrawArrays((uint) type, 0, color.Length);
            GL1.BindBuffer(0x8892, 0);
        }

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
            GL1.DrawBuffer(0x0405);
            GL1.Clear(0x00000100 | 0x00004000);

            var mat = Matrix4.CreateOrthographicOffCenter((float) view.Left, (float) view.Right, (float) view.Bottom,
                (float) view.Top, -1, 1);
            GL1.UniformMatrix4(matrixLocation, 1, false, ref mat);
            if (!sceneShaped)
            {
                _poses = new Vector3[w.Field.Height*w.Field.Height*2*3];
                _colors = new Vector3[w.Field.Height*w.Field.Height*2*3];
            }
            var visionmap = new bool[w.Field.Width, w.Field.Height];
            foreach (var unit in u)
            {
                var tmp = w.Vision.VisionField(unit);
                for (var i = 0; i < w.Field.Width; i++)
                {
                    for (var j = 0; j < w.Field.Height; j++)
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
                            UpdateTile(6*(i*w.Field.Height + j), w.Field[i, j].Passable, ref cEnemy, ref cVisible);
                        }
                        else
                        {
                            UpdateTile(6*(i*w.Field.Height + j), w.Field[i, j].Passable, ref cDefault, ref cVisible);
                        }
                    }
                    else
                    {
                        UpdateTile(6*(i*w.Field.Height + j), w.Field[i, j].Passable, ref cDefault, ref cDefault);
                    }
                }
            });
            foreach (var unit in u)
            {
                UpdateTile(6*(unit.Position.X*w.Field.Height + unit.Position.Y), true, ref cPlayer, ref cVisible);
            }
            DrawArray(null, _colors, PrimitiveType.Triangles);
        }

        private enum VertexAttribPointerType
        {
            /// <summary>
            ///     Original was GL_FLOAT = 0x1406
            /// </summary>
            Float = 0x1406
        }

        private enum BufferUsageHint
        {
            /// <summary>
            ///     Original was GL_STREAM_DRAW = 0x88E0
            /// </summary>
            StreamDraw = 0x88E0,

            /// <summary>
            ///     Original was GL_DYNAMIC_DRAW = 0x88E8
            /// </summary>
            DynamicDraw = 0x88E8
        }

        private enum PrimitiveType
        {
            /// <summary>
            ///     Original was GL_TRIANGLES = 0x0004
            /// </summary>
            Triangles = 0x0004
        }
    }
}