//*******************************************************************************
//
//          ATTSoftware Confidential 
//          © Copyright ATTSoftware - 2007.
//-------------------------------------------------------------------------------
//  Initiator: Vince Winton.
//*******************************************************************************

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace SM.Utilities
{
    public class ImageUtils
    {
        /// <summary>
        ///     Get width and height of an image.
        /// </summary>
        /// <returns>Return false if process unsuccessful and true for successful processed.</returns>
        public static bool GetSize(Stream stream, out int width, out int height)
        {
            width = height = 0;
            try
            {
                using (Image img = Image.FromStream(stream))
                {
                    width = img.Width;
                    height = img.Height;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        ///     Resize dimensions of an image
        /// </summary>
        /// <param name="MaxWidth">MaxWidth for output image</param>
        /// <param name="MaxHeight">MaxHeight for output image</param>
        /// <param name="stream">Stream to the image</param>
        /// <param name="NewFileName">Filename for saving</param>
        public static void ResizeImage(int MaxWidth, int MaxHeight, Stream stream, string NewFileName)
        {
            // load up the image, figure out a "best fit" resize,
            // and then save that new image
            var OriginalBmp =
                (Bitmap) Image.FromStream(stream).Clone();
            Size ResizedDimensions =
                GetDimensions(MaxWidth, MaxHeight, ref OriginalBmp);
            var NewBmp = new Bitmap(OriginalBmp, ResizedDimensions);

            string ext = Path.GetExtension(NewFileName).ToUpper();
            switch (ext)
            {
                case ".JPG":
                    NewBmp.Save(NewFileName, ImageFormat.Jpeg);
                    break;
                case ".GIF":
                    NewBmp.Save(NewFileName, ImageFormat.Gif);
                    break;
                case ".PNG":
                    NewBmp.Save(NewFileName, ImageFormat.Png);
                    break;
                case ".BMP":
                    NewBmp.Save(NewFileName, ImageFormat.Bmp);
                    break;
            }
        }

        /// <summary>
        ///     Get the best size for an image.
        /// </summary>
        /// <returns></returns>
        public static Size GetDimensions(int MaxWidth, int MaxHeight, ref Bitmap Bmp)
        {
            int Width;
            int Height;
            float Multiplier;

            Height = Bmp.Height;
            Width = Bmp.Width;

            // this means you want to shrink an image that is already shrunken!
            if (Height <= MaxHeight && Width <= MaxWidth)
                return new Size(Width, Height);

            // check to see if we can shrink it width first
            Multiplier = MaxWidth/(float) Width;
            if ((Height*Multiplier) <= MaxHeight)
            {
                Height = (int) (Height*Multiplier);
                return new Size(MaxWidth, Height);
            }

            // if we can't get our max width, then use the max height
            Multiplier = MaxHeight/(float) Height;
            Width = (int) (Width*Multiplier);
            return new Size(Width, MaxHeight);
        }

        /// <summary>
        ///     Check type file to update image
        /// </summary>
        /// <returns></returns>
        public static bool IsImageType(string fileExtension)
        {
            var arrtype = new List<string>
            {
                ".bmp",
                ".dib",
                ".jpg",
                ".jpeg",
                ".jpe",
                ".jfif",
                ".gif",
                ".tif",
                ".tiff",
                ".png"
            };
            return arrtype.Contains(fileExtension.ToLower());
        }
    }
}