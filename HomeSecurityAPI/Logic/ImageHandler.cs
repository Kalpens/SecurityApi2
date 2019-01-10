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
            string[] base64HeaderSplit = base64.Split(',');
            byte[] bytes = Convert.FromBase64String(base64HeaderSplit[1]);

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
