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
            char[] c = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            Random r = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(c[r.Next(0, 9)]);
            }
            return sb.ToString();
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
        public static string ModelToQueryString(Object obj)
        {
            var result = new List<string>();
            foreach (var property in typeof(Object).GetProperties())
            {
                result.Add(property.Name + "=" + property.GetValue(obj));
            }
            return string.Join("&", result);
        }
    }
}