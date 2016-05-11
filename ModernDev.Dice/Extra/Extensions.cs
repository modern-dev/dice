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
using System.Collections.Generic;

namespace ModernDev.Dice
{
    /// <summary>
    /// Provides extensions methods.
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// Capitalize the first letter of a string.
        /// </summary>
        /// <param name="this"></param>
        /// <returns>Returns a copy of this <see cref="string"/> with capitalized first letter.</returns>
        public static string Capitalize(this string @this)
            => @this[0].ToString().ToUpperInvariant() + @this.Substring(1);

        /// <summary>
        /// Adds a sequence of objects to the end of the <see cref="List{T}"/>.
        /// </summary>
        /// <typeparam name="T1">The element type of the <c>item1</c>.</typeparam>
        /// <typeparam name="T2">The element type of the <c>item2</c>.</typeparam>
        /// <typeparam name="T3">The element type of the <c>item3</c>.</typeparam>
        /// <typeparam name="T4">The element type of the <c>item4</c>.</typeparam>
        /// <param name="this">The list itself.</param>
        /// <param name="item1">The first item of the sequence.</param>
        /// <param name="item2">The second item of the sequence.</param>
        /// <param name="item3">The third item of the sequence.</param>
        /// <param name="item4">The fourth item of the sequence.</param>
        public static void Add<T1, T2, T3, T4>(this List<Tuple<T1, T2, T3, T4>> @this, T1 item1, T2 item2, T3 item3, T4 item4)
        {
            @this.Add(new Tuple<T1, T2, T3, T4>(item1, item2, item3, item4));
        }

        /// <summary>
        /// Adds a sequence of objects to the end of the <see cref="List{T}"/>.
        /// </summary>
        /// <typeparam name="T1">The element type of the <c>item1</c>.</typeparam>
        /// <typeparam name="T2">The element type of the <c>item2</c>.</typeparam>
        /// <param name="this">The list itself.</param>
        /// <param name="item1">The first item of the sequence.</param>
        /// <param name="item2">The second item of the sequence.</param>
        public static void Add<T1, T2>(this List<Tuple<T1, T2>> @this, T1 item1, T2 item2)
        {
            @this.Add(new Tuple<T1, T2>(item1, item2));
        }

        /// <summary>
        /// Adds a sequence of objects to the end of the <see cref="List{T}"/>.
        /// </summary>
        /// <typeparam name="T1">The element type of the <c>item1</c>.</typeparam>
        /// <typeparam name="T2">The element type of the <c>item2</c>.</typeparam>
        /// <param name="this">The list itself.</param>
        /// <param name="item1">The first item of the sequence.</param>
        /// <param name="item2">The second item of the sequence.</param>
        public static void Add<T1, T2>(this List<KeyValuePair<T1, T2>> @this, T1 item1, T2 item2)
            => @this.Add(new KeyValuePair<T1, T2>(item1, item2));
    }
}