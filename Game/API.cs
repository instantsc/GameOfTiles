using System.Runtime.InteropServices;

namespace Game
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct PixelFormatDescriptor
    {
        internal short Size;
        internal short Version;
        internal int Flags;
        internal int PixelType;
        internal byte ColorBits;
        internal byte RedBits;
        internal byte RedShift;
        internal byte GreenBits;
        internal byte GreenShift;
        internal byte BlueBits;
        internal byte BlueShift;
        internal byte AlphaBits;
        internal byte AlphaShift;
        internal byte AccumBits;
        internal byte AccumRedBits;
        internal byte AccumGreenBits;
        internal byte AccumBlueBits;
        internal byte AccumAlphaBits;
        internal byte DepthBits;
        internal byte StencilBits;
        internal byte AuxBuffers;
        internal byte LayerType;
        private readonly byte Reserved;
        internal int LayerMask;
        internal int VisibleMask;
        internal int DamageMask;
        public PixelFormatDescriptor(object a)
        {
            Size = (short) Marshal.SizeOf(typeof(PixelFormatDescriptor));
            Version = 1;
            Flags = 0x00000004 | 0x00000020 | 0x00000001;
            PixelType = 0;
            LayerMask = 0;
            ColorBits = 24;
            RedBits = 0;
            RedShift = 0;
            GreenBits = 0;
            GreenShift = 0;
            BlueBits = 0;
            BlueShift = 0;
            AlphaBits = 0;
            AlphaShift = 0;
            AccumBits = 0;
            AccumRedBits = 0;
            AccumGreenBits = 0;
            AccumBlueBits = 0;
            AccumAlphaBits = 0;
            DepthBits = 32;
            StencilBits = 0;
            AuxBuffers = 0;
            LayerType = 0;
            Reserved = 0;
            VisibleMask = 0;
            DamageMask = 0;
        }
    }
}