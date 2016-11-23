using System;
using System.Windows.Controls;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;

namespace Game
{
    public partial class AAGLControl : System.Windows.Forms.UserControl
    {
        public IntPtr GLContext;

        public AAGLControl()
        {
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        public void Init()
        {
            var a = Handle;
            GLContext = GL1.CreateContext(a);

        }

    }
}
