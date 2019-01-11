using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace HomeSecurityAPI.Logic
{
    public class ImageHandler
    {

        public void ConvertAndStore(string base64, string fileName)
        {
            StoreImage(ConverToImage(base64), fileName);
        }
        public Image<Rgba32> ConverToImage(string base64)
        {
            if (base64.Contains(','))
            {
                string[] base64HeaderSplit = base64.Split(',');
                base64 = base64HeaderSplit[1];
            }

            byte[] bytes = Convert.FromBase64String(base64);

            Image<Rgba32> image;
            image = Image.Load(bytes);

            return image;
        }

        public void StoreImage(Image<Rgba32> image, string fileName)
        {
                image.Save("wwwroot/Images/" + fileName);
        }
    }
}
