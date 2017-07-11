using System.IO;
using System.Windows.Media.Imaging;


namespace SteveHemond.MusicSheetViewer.Helpers
{
    public static class BitmapImageExtension
    {
        public static byte[] ToByteArray(this BitmapImage bitmapImage)
        {
            byte[] data;
            var encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }

            return data;
        }

        public static BitmapImage FromByteArray(this byte[] byteArray)
        {
            using (var ms = new MemoryStream(byteArray))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }
    }
}