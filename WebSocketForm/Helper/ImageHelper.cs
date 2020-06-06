using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WebSocketForm.Helper
{
    static class ImageHelper
    {
        public static Bitmap BitmapImageToBitmap(BitmapImage bi)
        {
            using (var stream = new MemoryStream())
            {
                var encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bi));
                encoder.Save(stream);

                return new Bitmap(stream);
            }
        }

        public static byte[] BitmapImageToBytes(BitmapImage bi)
        {
            using (var stream = bi.StreamSource)
            {
                if (stream == null || stream.Length == 0)
                {
                    return null;
                }

                using (var reader = new BinaryReader(stream))
                {
                    return reader.ReadBytes((int)stream.Length);
                }
            }
        }

        public static BitmapImage BytesToBitmapImage(byte[] bytes)
        {
            try
            {
                var bmi = new BitmapImage();
                bmi.BeginInit();
                bmi.StreamSource = new MemoryStream(bytes);
                bmi.EndInit();

                return bmi;
            }
            catch
            {
                return null;
            }
        }

        public static BitmapImage BitmapToBitmapImage(Bitmap bm)
        {
            var bmi = new BitmapImage();
            using (var ms = new MemoryStream())
            {
                bm.Save(ms, ImageFormat.Png);
                bmi.BeginInit();
                bmi.StreamSource = ms;
                bmi.CacheOption = BitmapCacheOption.OnLoad;
                bmi.EndInit();
                bmi.Freeze();

                return bmi;
            }
        }
    }
}
