using System;
using System.Collections.Generic;
using System.Text;

namespace Arnold_Co
{
    public static class Utils
    {
        public static T PickRandom<T>(this T[] array)
        {
            var index = Random.Shared.Next(array.Length);
            return array[index];
        }

        public static T PickRandom<T>(this List<T> list)
        {
            if (list.Count == 0)
                throw new InvalidOperationException("Cannot pick a random element from an empty list.");

            var index = Random.Shared.Next(list.Count);
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
    }
}
