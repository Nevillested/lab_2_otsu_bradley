using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class Otsu
    {
        public static int GetThreshold(Bitmap image, Dictionary<int, int> br_ch)
        {
            int count1 = 1;
            int resTH = 0;
            double double_first = -1;
            long sum_int_first = 0;
            long sum_int_sec = 0;
            const int N = 256;
            int sum = image.Width * image.Height;
            int count2 = sum;

            for (int i = 0; i < br_ch.Count; i++)
            {
                sum_int_sec += br_ch[i] * i;
            }

            double mu1 = 0.0F;
            double mu2 = sum_int_sec / count2;

            for (int i = 1; i < N; i++)
            {
                int newbin = br_ch[i];
                count1 += newbin;
                count2 -= newbin;
                double w1 = (double)count1 / sum;
                double w2 = 1 - w1;
                sum_int_first += newbin * i;
                sum_int_sec -= newbin * i;
                mu1 = sum_int_first / count1;
                mu2 = sum_int_sec / (count2 == 0 ? 1 : count2);
                double sigma2 = w1 * w2 * (mu1 - mu2) * (mu1 - mu2);
                if (sigma2 > double_first)
                {
                    double_first = sigma2;
                    resTH = i;
                }
            }
            return resTH;
        }
    }
}
