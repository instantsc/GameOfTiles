using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace Game
{
    static class Graphics
    {
        private static void DrawTriangle(ref Vector3d vertex1, ref Vector3d vertex2, ref Vector3d vertex3, Vector3 color, bool needBeginEnd = false)
        {
            if (needBeginEnd) GL.Begin(PrimitiveType.Triangles);
            GL.Color3(color);
            GL.Vertex3(ref vertex1.X);
            GL.Vertex3(ref vertex2.X);
            GL.Vertex3(ref vertex3.X);
            if (needBeginEnd) GL.End();
        }
        private static void DrawTile(int x, int y, bool passable = true, double threat = 0, double vision = 0)
        {
            Vector3d vert1 = new Vector3d(x, y, 0);
            Vector3d vert2 = new Vector3d(x + 1, y, 0);
            Vector3d vert3 = new Vector3d(x + 1, y + 1, 0);
            Vector3d vert4 = new Vector3d(x, y + 1, 0);

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
        public static void DrawWorld(World w,Unit u)
        {
            GL.Begin(PrimitiveType.Triangles);
            var visionmap = w.Vision.VisionField(u);
            for (int i = 0; i < w.Field.Width; i++)
            {
                for (int j = 0; j < w.Field.Height; j++)
                {
                    DrawTile(i, j, w.Field[new Position(i, j)].Passable, 0.5, visionmap[i,j]?1:0);
                }
            }
            GL.End();
        }
    }
}
