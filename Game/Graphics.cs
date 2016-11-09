using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
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
            var a=  GL.GetProgramInfoLog(ProgramID);
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
        public static void DrawArray(Vertex[] coords, PrimitiveType type, bool texture = false)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOId);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertex.Size*coords.Length, coords, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vertex.Size, 0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, Vertex.Size, Vector3.SizeInBytes);
            GL.DrawArrays(type, 0, coords.Length);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        public static int VAOId, VBOId;
        private static List<Vertex> _drawList;
        private static void DrawTriangle(ref Vector3 vertex1, ref Vector3 vertex2, ref Vector3 vertex3, Vector3 color)
        {
            var vertex = new Vertex(vertex1, color);
            _drawList.Add(vertex);
            vertex = new Vertex(vertex2, color);
            _drawList.Add(vertex);
            vertex = new Vertex(vertex3, color);
            _drawList.Add(vertex);
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
            _drawList = new List<Vertex>(w.Field.Height * w.Field.Height * 2 * 3);
            var visionmap = w.Vision.VisionField(u);
            for (int i = 0; i < w.Field.Width; i++)
            {
                for (int j = 0; j < w.Field.Height; j++)
                {
                    DrawTile(i, j, w.Field[new Position(i, j)].Passable,0,visionmap[i,j]?1:0);
                }
            }
            DrawArray(_drawList.ToArray(), PrimitiveType.Triangles);
        }
    }
}
