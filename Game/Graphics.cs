using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace Game
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vertex
    {
        public Vertex(Vector3 pos = default(Vector3), Vector3 color = default(Vector3))
        {
            Pos = pos;
            Color = color;
        }
        public Vector3 Pos;
        public Vector3 Color;
        public static readonly int Size = Marshal.SizeOf(typeof(Vertex));
    }

    public class ShaderProgram : IDisposable
    {
        public int ProgramID { get; }
        public ShaderProgram(string vertexPath, string fragPath)
        {
            int vID = GL.CreateShader(ShaderType.VertexShader);
            int fID = GL.CreateShader(ShaderType.FragmentShader);
            string vertexText = File.ReadAllText(vertexPath);
            string fragText = File.ReadAllText(fragPath);
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
    static class Graphics
    {
        public static ShaderProgram ShaderProgram;
        private static bool sceneShaped;
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

        private static int _iii = 0;
        private static void DrawTriangle(ref Vector3 vertex1, ref Vector3 vertex2, ref Vector3 vertex3, Vector3 color)
        {
            _poses[_iii] = vertex1;
            _colors[_iii++] = color;
            _poses[_iii] = vertex2;
            _colors[_iii++] = color;
            _poses[_iii] = vertex3;
            _colors[_iii++] = color;
        }

        private static void UpdateTile(int s, bool passable, double threat, double vision)
        {
            if (passable)
            {
                var c1 = new Vector3(1 - (float)vision, 1 - (float)vision, 1);
                var c2 = new Vector3(1, 1 - (float)threat, 1 - (float)threat);
                for (int i = s; i < s + 3; i++)
                {
                    _colors[i] = c1;
                }
                for (int i = s + 3; i < s + 6; i++)
                {
                    _colors[i] = c2;
                }
            }
        }
        private static void DrawTile(int x, int y, bool passable = true, double threat = 0, double vision = 0)
        {
            Vector3 vert1 = new Vector3(x, y, 0);
            Vector3 vert2 = new Vector3(x + 1, y, 0);
            Vector3 vert3 = new Vector3(x + 1, y + 1, 0);
            Vector3 vert4 = new Vector3(x, y + 1, 0);

            if (passable)
            {
                DrawTriangle(ref vert1, ref vert2, ref vert4, Vector3.UnitZ * (float)vision + Vector3.One * (float)(1 - vision));
                DrawTriangle(ref vert2, ref vert3, ref vert4, Vector3.UnitX * (float)threat + Vector3.One * (float)(1 - threat));
            }
            else
            {
                DrawTriangle(ref vert1, ref vert2, ref vert4, Vector3.Zero);
                DrawTriangle(ref vert2, ref vert3, ref vert4, Vector3.Zero);
            }
        }
        public static void DrawWorld(World w, Unit u)
        {
            if (!sceneShaped)
            {
                _poses = new Vector3[w.Field.Height * w.Field.Height * 2 * 3];
                _colors = new Vector3[w.Field.Height * w.Field.Height * 2 * 3];
            }
            var visionmap = w.Vision.VisionField(u);
            if (!sceneShaped)
            {
                for (int i = 0; i < w.Field.Width; i++)
                {
                    for (int j = 0; j < w.Field.Height; j++)
                    {
                        DrawTile(i, j, w.Field[i, j].Passable, 0, visionmap[i, j] ? 1 : 0);
                    }
                }
                DrawArray(_poses, _colors, PrimitiveType.Triangles);
                sceneShaped = true;
            }
            else
            {
                Parallel.For(0, w.Field.Width, i =>
                  {
                      for (int j = 0; j < w.Field.Height; j++)
                      {
                          UpdateTile(6 * (i * w.Field.Height + j), w.Field[i, j].Passable, 0, visionmap[i, j] ? 1 : 0);
                      }
                  });
                DrawArray(null, _colors, PrimitiveType.Triangles);
            }
        }
    }
}
