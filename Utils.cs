using NumSharp;
using Soenneker.Utils.Random;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arnold_Co
{
    public static class Utils
    {
        public static T PickRandom<T>(this T[] array)
        {
            var index = RandomUtil.Next(array.Length);
            return array[index];
        }

        public static T PickRandom<T>(this List<T> list)
        {
            if (list.Count == 0)
                throw new InvalidOperationException("Cannot pick a random element from an empty list.");

            var index = RandomUtil.Next(list.Count);
            return list[index];
        }

        public static string GetRandomVoice()
        {
            var dir = @"C:\Users\adamk\source\repos\Arnold&Co\bin\Debug\net10.0-windows\voices";
            var files = Directory.GetFiles(dir);
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Path.GetFileNameWithoutExtension(files[i]);
            }
            return files.PickRandom();
        }
        public static float Similarity(this string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2)) return 0;

            int stepsToSame = ComputeLevenshtein(s1, s2);
            return 1.0f - (stepsToSame / (float)Math.Max(s1.Length, s2.Length));
        }

        private static int ComputeLevenshtein(string a, string b)
        {
            int[,] d = new int[a.Length + 1, b.Length + 1];

            for (int i = 0; i <= a.Length; i++) d[i, 0] = i;
            for (int j = 0; j <= b.Length; j++) d[0, j] = j;

            for (int i = 1; i <= a.Length; i++)
            {
                for (int j = 1; j <= b.Length; j++)
                {
                    int cost = a[i - 1] == b[j - 1] ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost
                    );
                }
            }

            return d[a.Length, b.Length];
        }
    }
}
