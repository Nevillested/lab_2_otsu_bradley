using System;
using System.Collections.Generic;
using System.Drawing;
using AForge.Imaging.Filters;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Bitmap grayScaleImage;
            Mat grayScaleImageMatrix;
            Dictionary<int, int> br;

            //загружаем картинку
            string path_image = Directory.GetCurrentDirectory();
            path_image = path_image + "/image.bmp";

            //переводим в чб
            var readImage = Cv2.ImRead(path_image);
            grayScaleImageMatrix = new Mat();
            Cv2.CvtColor(readImage, grayScaleImageMatrix, ColorConversionCodes.RGB2GRAY);
            grayScaleImage = grayScaleImageMatrix.ToBitmap();
            //сохраняем для просмотра результата
            grayScaleImage.Save("first_blood.bmp");

        
            int width;
            int Height;
            int TS = 5;//ручное значение
            br = new Dictionary<int, int>();
            width = grayScaleImage.Width;
            Height = grayScaleImage.Height;
            //простая бинаризация
            var binarizedImage = new Bitmap(width, Height);
            for (int i = 0; i < 256; i++)
            {
                br.Add(i, 0);
            }
            Color color;
            for (int i = 0; i < grayScaleImage.Width; i++)
            {
                for (int j = 0; j < grayScaleImage.Height; j++)
                {
                    color = grayScaleImage.GetPixel(i, j);
                    int brightness = ((color.R + color.G + color.B) / 3);
                    ++br[brightness];
                    binarizedImage.SetPixel(i, j, brightness < TS ? Color.Black : Color.White);
                }
            }
            //сохраняем для просмотра результата
            binarizedImage.Save("simple_bin.bmp");
            

            //Оцу
            var otsuImage = new Bitmap(width, Height);
            int threshold = Otsu.GetThreshold(grayScaleImage, br);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    var color2 = grayScaleImage.GetPixel(i, j);
                    int brightness = (color2.R + color2.G + color2.B) / 3;

                    otsuImage.SetPixel(i, j, brightness < threshold ? Color.Black : Color.White);
                }
            }
            //сохраняем для просмотра результата
            otsuImage.Save("otsu.bmp");
            

            //Брэдли
            int d = 7;//ручной ввод
            double t = 0.15d;//ручной ввод
            var BradleyImage = new Bitmap(width, Height);
            BradleyImage = Bradley.Binarize(grayScaleImage, d, t);
            BradleyImage.Save("bradley.bmp");
            
            Console.ReadLine();
        }
    }
}
