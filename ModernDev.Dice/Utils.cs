//
// This file\code is part of InTouch project.
//
// InTouch - is a .NET wrapper for the vk.com API.
// https://github.com/virtyaluk/InTouch
//
// Copyright (c) 2016 Bohdan Shtepan
// http://modern-dev.com/
//
// Licensed under the GPLv3 license.
//

using System;
using System.Linq;

namespace ModernDev.Dice
{
    /// <summary>
    /// TODO:
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// TODO:
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int LuhnCalcualte(int num)
        {
            var digits = num.ToString().ToCharArray().Reverse().Select(val => int.Parse(val.ToString())).ToList();
            var sum = 0;

            for (int i = 0, l = digits.Count; l > i; ++i)
            {
                var digit = digits[i];

                if (i % 2 == 0)
                {
                    digit *= 2;

                    if (digit > 9)
                    {
                        digit -= 9;
                    }
                }

                sum += digit;
            }

            return sum * 9 % 10;
        }

        /// <summary>
        /// TODO:
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool LuhnCheck(int num)
        {
            var str = num.ToString();
            var checkDigit = int.Parse(str.Substring(str.Length - 1));
            
            return checkDigit == LuhnCalcualte(int.Parse(str.Substring(0, str.Length - 1)));
        }

        /// <summary>
        /// TODO:
        /// </summary>
        /// <param name="num"></param>
        /// <param name="width"></param>
        /// <param name="pad"></param>
        /// <returns></returns>
        public static string NumberPadding(int num, int width = 2, char pad = '0')
            => num.ToString().Length >= width
                ? $"{num}"
                : string.Join(pad.ToString(), new string[width - $"{num}".Length + 1]) + num;

        /// <summary>
        /// TODO:
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            var unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var unixTimeStampInTicks = (dateTime.ToUniversalTime() - unixStart).Ticks;

            return (double)unixTimeStampInTicks / TimeSpan.TicksPerSecond;
        }

        /// <summary>
        /// TODO:
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime UnixTimestampToDateTime(double unixTime)
        {
            var unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var unixTimeStampInTicks = (long)(unixTime * TimeSpan.TicksPerSecond);

            return new DateTime(unixStart.Ticks + unixTimeStampInTicks, DateTimeKind.Utc);
        }
    }
}