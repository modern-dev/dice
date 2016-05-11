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
using System.Text.RegularExpressions;

namespace ModernDev.Dice.Tests
{
    internal static class Ex
    {
        public static void Times(this int @this, Action action)
        {
            for (var i = 0; i < @this; i++)
            {
                action?.Invoke();
            }
        }

        public static bool Match(this string @this, string pattern)
            => new Regex(pattern, RegexOptions.Multiline).IsMatch(@this);

        public static bool Match(this char @this, string pattern)
            => new Regex(pattern).IsMatch(@this.ToString());
    }
}