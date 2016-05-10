//
// This file\code is part of InTouch project.
//
// InTouch - is a .NET wrapper for the vk.com API.
// https://github.com/virtyaluk/InTouch
//
// Copyright (c) 2016 Bohdan Shtepan
// http://modern-dev.com/
//
// Licensed under the MIT license.
//

using System;
using System.Linq;

namespace ModernDev.Dice
{
    /// <summary>
    /// Provides utils methods.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Calculates checksum for credit card number using Luhn Algorithm.
        /// </summary>
        /// <param name="num">Credit card number.</param>
        /// <returns>Returns a number representing credit card number checksum.</returns>
        public static int LuhnCalcualte(long num)
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
        /// Checks whether the given credit card number is valid.
        /// </summary>
        /// <param name="num">Credit card number.</param>
        /// <returns>Returns "true" if credit card number is valid; False otherwise.</returns>
        public static bool LuhnCheck(long num)
        {
            var str = num.ToString();
            var checkDigit = int.Parse(str.Substring(str.Length - 1));
            
            return checkDigit == LuhnCalcualte(int.Parse(str.Substring(0, str.Length - 1)));
        }

        /// <summary>
        /// Pad a number with some string until it reaches a desired width.
        /// </summary>
        /// <param name="num">A number.</param>
        /// <param name="width">Pad width.</param>
        /// <param name="pad">Pad symbol.</param>
        /// <returns>Returns string representation of a number padded with given symbols.</returns>
        public static string NumberPadding(int num, int width = 2, char pad = '0')
            => num.ToString().Length >= width
                ? $"{num}"
                : string.Join(pad.ToString(), new string[width - $"{num}".Length + 1]) + num;

        /// <summary>
        /// Converts <see cref="DateTime"/> object to Unix timestamp.
        /// </summary>
        /// <param name="dateTime"><see cref="DateTime"/> to convert.</param>
        /// <returns>Returns Unix timestamp from given <see cref="DateTime"/> object.</returns>
        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            var unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var unixTimeStampInTicks = (dateTime.ToUniversalTime() - unixStart).Ticks;

            return (double)unixTimeStampInTicks / TimeSpan.TicksPerSecond;
        }

        /// <summary>
        /// Converts Unix timestamp to the <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="unixTime">Unix timestamp.</param>
        /// <returns>Returns Unix timestamp converted to the <see cref="DateTime"/> object.</returns>
        public static DateTime UnixTimestampToDateTime(double unixTime)
        {
            var unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var unixTimeStampInTicks = (long)(unixTime * TimeSpan.TicksPerSecond);

            return new DateTime(unixStart.Ticks + unixTimeStampInTicks, DateTimeKind.Utc);
        }
    }
}