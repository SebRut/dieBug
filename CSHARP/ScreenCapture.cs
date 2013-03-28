using System.Drawing;

namespace dieBug
{
    public static class ScreenCapture
    {
        public static Bitmap Screen()
        {
            Rectangle bounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            return Area(0, 0, bounds);
        }

        public static Bitmap Area(int x, int y, int width, int heigth)
        {
            return Area(x, y, new Rectangle(0, 0, width, heigth));
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
    }
}