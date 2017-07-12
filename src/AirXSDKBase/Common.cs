using System;
using System.Collections.Generic;

namespace Dwing.AirXSDKBase.Common
{
    public static class CommonTools
    {
        /// <summary>
        /// 随机字符串
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <returns>随机字符串</returns>
        internal static string Nonce(int length)
        {
            Random r = new Random();
            var min = Math.Pow(10, (length - 1));
            var max = Math.Pow(10, (double)(length) + 1);
            return r.Next((int)min, (int)max).ToString();
        }

        /// <summary>
        /// 随机字符串
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <returns>随机字符串</returns>
        internal static string Nonce()
        {
            Random r = new Random();
            return r.Next(1000, 99999).ToString();
        }

        internal static string ModelToQueryString<T>(T obj)
        {
            var result = new List<string>();
            foreach (var property in typeof(T).GetProperties())
            {
                result.Add(property.Name + "=" + property.GetValue(obj));
            }
            return string.Join("&", result);
        }

        /// <summary>
        /// 排序版
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static string ModelToQueryStringSort(object obj)
        {
            var result = new List<string>();
            foreach (var property in obj.GetType().GetProperties())
            {
                result.Add(property.Name + "=" + property.GetValue(obj));
            }
            result.Sort();
            return string.Join("&", result);
        }

        internal static IEnumerable<KeyValuePair<string, string>> ModelToKeyValueSort(object obj)
        {
            var result = new SortedDictionary<string, string>();
            foreach (var property in obj.GetType().GetProperties())
            {
                result.Add(property.Name, property.GetValue(obj).ToString());
            }
            return result;
        }
    }

#if (NET45 || NET452)

    public static class DateTimeOffSetEx
    {
        public static long ToUnixTimeSeconds(this DateTimeOffset date)
        {
            return ((date.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
        }
    }

#endif
}