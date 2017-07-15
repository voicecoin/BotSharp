using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.Utility
{
    public static class ObjectExtensions
    {
        public static T Map<T>(this Object source)
        {
            return Mapper.Map<T>(source);
        }

        public static T MapByJsonString<T>(this Object Source)
        {
            string json = JsonConvert.SerializeObject(Source);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string GetRandomColor()
        {
            Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
            System.Threading.Thread.Sleep(RandomNum_First.Next(50));
            Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);

            //  为了在白色背景上显示，尽量生成深色
            int int_Red = RandomNum_First.Next(256);
            int int_Green = RandomNum_Sencond.Next(256);
            int int_Blue = (int_Red + int_Green > 400) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;

            return "#" + int_Red.ToString("X").PadLeft(2, '0') + int_Green.ToString("X").PadLeft(2, '0') + int_Blue.ToString("X").PadLeft(2, '0');
        }

        public static T Random<T>(this IEnumerable<T> source)
        {
            int randomMax = source.Count();
            int messageIdx = new Random().Next(0, randomMax);
            return source.ElementAt(messageIdx);
        }

        public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

    }
}
