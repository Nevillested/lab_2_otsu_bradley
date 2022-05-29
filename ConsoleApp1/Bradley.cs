using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;


namespace ConsoleApp1
{
    public static class Bradley
    {
        public static Bitmap Binarize(Bitmap image, int d, double t)
        {
            string bm = Directory.GetCurrentDirectory();
            bm = bm + "/image.bmp";
            int width = image.Width;
            int height = image.Height;
            var result = new Bitmap(image.Width, image.Height);
            int sum;
            var intImg = new int[width, height];
            var brightness = new int[width, height];
            int s = width / d;
            for (int i = 0; i < width; i++)
            {
                sum = 0;
                for (int j = 0; j < height; j++)
                {
                    var color = image.GetPixel(i, j);
                    brightness[i, j] = (color.R + color.G + color.B) / 3;
                    sum += brightness[i, j];
                    if (i == 0)
                    {
                        intImg[i, j] = sum;
                    }
                    else
                    {
                        intImg[i, j] = intImg[i - 1, j] + sum;
                    }
                }
            }
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int x1 = i - s / 2;
                    int x2 = i + s / 2;
                    int y1 = j - s / 2;
                    int y2 = j + s / 2;
                    if (x1 < 1) x1 = 1;
                    if (x2 >= width) x2 = width - 1;
                    if (y1 < 1) y1 = 1;
                    if (y2 >= height) y2 = height - 1;
                    int count = (x2 - x1) * (y2 - y1);
                    sum = intImg[x2, y2] - intImg[x2, y1 - 1] - intImg[x1 - 1, y2] + intImg[x1 - 1, y1 - 1];
                    if (brightness[i, j] * count <= sum * (100 - t) / 100)
                    {
                        result.SetPixel(i, j, Color.Black);
                    }
                    else
                    {
                        result.SetPixel(i, j, Color.White);
                    }
                }
            }
            return result;
        }
    }
}
