using System;
using System.Text;
using System.ComponentModel;
using AirXSDKBase.Model;
using System.Collections.Generic;

namespace AirXSDKBase.Common
{
    public static class CommonTools
    {
        /// <summary>
        /// 随机字符串
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <returns>随机字符串</returns>
        public static string Nonce(int length)
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
        public static string Nonce()
        {
            Random r = new Random();
            return r.Next(1000, 99999).ToString();
        }

        public static string ModelToQueryString<T>(T obj)
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
        public static string ModelToQueryStringSort(object obj)
        {
            var result = new List<string>();
            foreach (var property in obj.GetType().GetProperties())
            {
                result.Add(property.Name + "=" + property.GetValue(obj));
            }
            result.Sort();
            return string.Join("&", result);
        }

        public static IEnumerable<KeyValuePair<string, string>> ModelToKeyValueSort(object obj)
        {
            var result = new SortedDictionary<string, string>();
            foreach (var property in obj.GetType().GetProperties())
            {
                result.Add(property.Name, property.GetValue(obj).ToString());
            }
            return result;
        }
    }
}