using OpenTK;
using OpenTK.Graphics;

namespace Game
{
    public partial class AAGLControl : GLControl
    {
        public AAGLControl() : base(new GraphicsMode(32, 24, 0, 8), 3, 0, GraphicsContextFlags.ForwardCompatible)
        {}
    }
}
