using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace Game
{

    public struct Vertex
    {
        public Vertex(Vector3 pos = default(Vector3), Vector3 color = default(Vector3), Vector2 tex = default(Vector2))
        {
            Pos = pos;
            Color = color;
            Tex = tex;
        }
        public Vector3 Pos, Color;
        public Vector2 Tex;
        public static readonly int Size = Marshal.SizeOf(typeof(Vertex));
    }
    static class Graphics
    {
        public static void DrawArray(Vertex[] coords, PrimitiveType type, bool texture = false)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOId);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.VertexPointer(3, VertexPointerType.Float, Vertex.Size, 0);
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.ColorPointer(3, ColorPointerType.Float, Vertex.Size, Vector3.SizeInBytes);
            if (texture)
            {
                GL.Enable(EnableCap.Texture2D);
                // GL.BindTexture(TextureTarget.Texture2D, textures);
                GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)TextureEnvMode.Modulate);
                GL.EnableClientState(ArrayCap.TextureCoordArray);
                GL.TexCoordPointer(2, TexCoordPointerType.Float, Vertex.Size, 2 * Vector3.SizeInBytes);
            }
            else
            {
                GL.Disable(EnableCap.Texture2D);
                GL.DisableClientState(ArrayCap.TextureCoordArray);
            }
            GL.BufferData(BufferTarget.ArrayBuffer, Vertex.Size * coords.Length, coords, BufferUsageHint.StaticDraw);
            GL.DrawArrays(type, 0, coords.Length);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
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
                    DrawTile(i, j, w.Field[new Position(i, j)].Passable, 0.5, visionmap[i, j] ? 1 : 0);
                }
            }
            DrawArray(_drawList.ToArray(), PrimitiveType.Triangles);
        }
    }
}
