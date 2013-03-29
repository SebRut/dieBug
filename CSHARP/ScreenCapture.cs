using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace dieBug
{
    public static class ScreenCapture
    {
        

        public static Bitmap Window(IntPtr handle)
        {
            NativeMethods.WINDOWINFO info = new NativeMethods.WINDOWINFO();
            info.cbSize = (uint)Marshal.SizeOf(info);
            NativeMethods.GetWindowInfo(handle, ref info);

            NativeMethods.SetForegroundWindow(handle);

            Bitmap tmp = Area(info.rcWindow.X, info.rcWindow.Y, info.rcWindow.Width, info.rcWindow.Height);
            NativeMethods.SetForegroundWindow(Process.GetCurrentProcess().MainWindowHandle);
            return tmp;
        }

        public static Bitmap Screen()
        {
            Rectangle bounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            return Area(0, 0, bounds);
        }

        public static Bitmap Area(int x, int y, int width, int height)
        {
            return Area(x, y, new Rectangle(0, 0, width, height));
        }

        public static Bitmap Area(int x, int y, Rectangle bounds)
        {
            var bitmap = new Bitmap(bounds.Width, bounds.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(new Point(x, y), Point.Empty, bounds.Size);
                return bitmap;
            }
        }

        public static IntPtr GetWindowHandleByMouse()
        {
            NativeMethods.POINT mPoint = new NativeMethods.POINT();
            NativeMethods.GetCursorPos(out mPoint);
            IntPtr windowHandle = NativeMethods.WindowFromPoint(mPoint);
            return windowHandle;
        }

    }
}