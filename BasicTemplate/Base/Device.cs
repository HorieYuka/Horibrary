using Emgu.CV.Structure;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace BasicTemplate.Base
{
    internal class Device
    {
        class CameraFunc
        {
            private static BitmapPalette Pal;

            private const int CropWidth = 776;
            private const int CropHeight = 720;
            private const int OffsetX = 240;
            private const int OffsetY = 0;

            private const int Rect_PosX = 145;
            private const int Rect_PosY = 220;
            private const int Rect_Width = 490;
            private const int Rect_Height = 490;

            private const short Rect_Margin = 5;
            private const double ImageScaleRate = 8;

            private static int CropMargin = 50;


            public static RenderTargetBitmap GetImage(Mat CapturedImage, ImageSource ReconImage = null)
            {

                WriteableBitmap WB = new WriteableBitmap(Byte2BmpImg(CapturedImage.ToImage<Bgr, byte>().ToJpegData()));
                CroppedBitmap CB = new CroppedBitmap(WB, new Int32Rect(OffsetX, OffsetY, CropWidth, CropHeight));

                DrawingVisual DV = new DrawingVisual();
                using (DrawingContext DC = DV.RenderOpen())
                {
                    DC.DrawImage(CB, new Rect(new System.Windows.Size(CropWidth, CropHeight)));
                    DC.DrawRectangle(null, new System.Windows.Media.Pen(System.Windows.Media.Brushes.Crimson, 6),
                        new Rect(Rect_PosX, Rect_PosY, Rect_Width, Rect_Height));
                    DC.DrawRectangle(null, new System.Windows.Media.Pen(System.Windows.Media.Brushes.Yellow, 6),
                        new Rect(Rect_PosX + Rect_Margin, Rect_PosY + Rect_Margin, Rect_Width - Rect_Margin * 2, Rect_Height - Rect_Margin * 2));
                    if (ReconImage != null)
                        DC.DrawImage(ReconImage, new Rect(Rect_PosX + Rect_Margin * 2, Rect_PosY + Rect_Margin * 2,
                            Rect_Width - Rect_Margin * 4, Rect_Height - Rect_Margin * 4));
                }

                RenderTargetBitmap RTBM = new RenderTargetBitmap((int)CropWidth, (int)CropHeight, 96, 96, PixelFormats.Default);
                RTBM.Render(DV);

                return RTBM;
            }



            public static BitmapSource Byte2ReconImg(byte[] ReconData)
            {
                if (ReconData == null) return null;

                var ActualLength = (int)Math.Sqrt(ReconData.Length);

                BitmapSource Out = BitmapSource.Create(ActualLength, ActualLength, ActualLength, ActualLength,
                      PixelFormats.Indexed8, Pal, ReconData, ActualLength);

                var Temp = new TransformedBitmap(Out, new ScaleTransform(ImageScaleRate, ImageScaleRate));


                return new CroppedBitmap(Temp, new Int32Rect(CropMargin, CropMargin, (int)(Temp.PixelWidth - CropMargin), (int)(Temp.PixelHeight - CropMargin)));
            }

            private static BitmapImage Byte2BmpImg(byte[] Arr)
            {
                using (var Stream = new System.IO.MemoryStream(Arr))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = Stream;
                    image.EndInit();
                    return image;
                }
            }

            // Generate JET Palette
            public static void UpdatePalette(PaletteDataModel PalModel)
            {
                // 8-bit(256) ARGB indexed Color
                int[,] Out = new int[4, 256];

                for (int i = 0; i < PalModel.ColorWidth; i++)
                {
                    Out[1, 255 - i] = 255;

                    Out[1, 255 - PalModel.ColorWidth * 1 - i] = 255;
                    Out[2, 255 - PalModel.ColorWidth * 1 - i] = 255;

                    Out[2, 255 - PalModel.ColorWidth * 2 - i] = 255;
                    Out[3, 255 - PalModel.ColorWidth * 2 - i] = 255;

                    Out[3, 255 - PalModel.ColorWidth * 3 - i] = 255;
                }

                for (int i = 255; i > 255 - (PalModel.ColorWidth * 4); i--) Out[0, i] = PalModel.Alpha;

                // Debug
                //for (int i = 0; i < 256; i++) Out[0, i] = Alpha;

                List<System.Windows.Media.Color> Palette_Colors = new List<System.Windows.Media.Color>();
                for (int i = 0; i < 256; i++) Palette_Colors.Add(System.Windows.Media.Color.FromArgb((byte)Out[0, i],
                    (byte)Out[1, i], (byte)Out[2, i], (byte)Out[3, i]));

                Pal = new BitmapPalette(Palette_Colors);
            }

        }
    }
}
