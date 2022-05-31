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
            Mat src = new Mat(path_image, ImreadModes.Color);

            //переводим в чб
            var readImage = Cv2.ImRead(path_image);
            grayScaleImageMatrix = new Mat();
            Cv2.CvtColor(readImage, grayScaleImageMatrix, ColorConversionCodes.RGB2GRAY);
            grayScaleImage = grayScaleImageMatrix.ToBitmap();
            //сохраняем для просмотра результата
            grayScaleImage.Save("first_blood.bmp");


            grayScaleImageMatrix = new Mat();
            grayScaleImageMatrix.ConvertTo(readImage, MatType.CV_8UC1);
            Mat gray = src.CvtColor(ColorConversionCodes.BGR2GRAY);
            //сохраняем для просмотра результата
            gray.SaveImage("first_blood_mat.bmp");

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
            Mat binar = src.Threshold(150, 255, ThresholdTypes.Binary);
            binar.SaveImage("simple_bin_mat.bmp");
            binarizedImage.Save("simple_bin.bmp");


            //Оцу
            //var otsuImage = new Bitmap(width, Height);
            //int threshold = Otsu.GetThreshold(grayScaleImage, br);
            //for (int i = 0; i < width; i++)
            //{
            //    for (int j = 0; j < Height; j++)
            //    {
            //        var color2 = grayScaleImage.GetPixel(i, j);
            //        int brightness = (color2.R + color2.G + color2.B) / 3;

            //        otsuImage.SetPixel(i, j, brightness < threshold ? Color.Black : Color.White);
            //    }
            //}
            ////сохраняем для просмотра результата
            //otsuImage.Save("otsu.bmp");
            //Оцу готовым методом
            var source = new Mat(path_image, ImreadModes.Grayscale);
            var bin = source.Threshold(0, 255, ThresholdTypes.Otsu);
            bin.SaveImage("для_сравнения_otsu_opencv_mat.bmp");

            //Оцу своим методом
            otsu_manual ot = new otsu_manual();
            Bitmap org = new Bitmap(path_image, true);
            Bitmap temp = (Bitmap)org.Clone();
            ot.Convert2GrayScaleFast(temp);
            int otsuThreshold = ot.getOtsuThreshold((Bitmap)temp);
            ot.threshold(temp, otsuThreshold);
            temp.Save("для_сравнения_otsu_manual.bmp");

            //Брэдли
            int d = 7;//ручной ввод
            double t = 0.15d;//ручной ввод
            var BradleyImage = new Bitmap(width, Height);
            BradleyImage = Bradley.Binarize(grayScaleImage, d, t);
            BradleyImage.Save("bradley.bmp");

        }
    }
}
