using System.Windows.Forms;

namespace Game
{
    public sealed partial class AAGLControl : UserControl
    {
        public AAGLControl()
        {
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            DoubleBuffered = false;
        }
    }
}