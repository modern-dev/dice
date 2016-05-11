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
using NUnit.Framework;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;

namespace ModernDev.Dice.Tests
{
    [TestFixture]
    public class BasicTest
    {
        private readonly Dice _dice = new Dice();

        [Test]
        public void NextBool()
        {
            Throws<ArgumentOutOfRangeException>(() => _dice.NextBool(-10));
            Throws<ArgumentOutOfRangeException>(() => _dice.NextBool(110));
            DoesNotThrow(() => _dice.NextBool());

            var trueCount = 0;

            1000.Times(() =>
            {
                if (_dice.NextBool(30))
                {
                    trueCount++;
                }
            });
            
            That(trueCount, InRange(200, 400), "trueCount in [200, 400]");

            trueCount = 0;

            1000.Times(() =>
            {
                if (_dice.NextBool(99))
                {
                    trueCount++;
                }
            });

            That(trueCount, GreaterThanOrEqualTo(900), "trueCount >= 900");
        }

        [Test]
        public void NextInt()
        {
            int i;

            Throws<ArgumentException>(() => _dice.NextInt(100, 50));
            DoesNotThrow(() => _dice.NextInt());

            1000.Times(() =>
            {
                i = _dice.NextInt(0);
                
                That(i, GreaterThanOrEqualTo(0), "i >= 0");

                i = _dice.NextInt(-25, 10000000);
                
                That(i, GreaterThan(-26), "i > -26");

                i = _dice.NextInt(-25, -1);
                
                That(i, InRange(-26, 0), "i in [-26, 0]");
            });
        }

        [Test]
        public void NextLong()
        {
            long i;

            Throws<ArgumentException>(() => _dice.NextLong(100, 50));
            DoesNotThrow(() => _dice.NextLong());

            1000.Times(() =>
            {
                i = _dice.NextLong(0);

                That(i, GreaterThanOrEqualTo(0), "i >= 0");

                i = _dice.NextLong(-25, 10000000);
                
                That(i, GreaterThan(-26), "i > -26");

                i = _dice.NextLong(-25, -1);
                
                That(i, InRange(-26, 0), "i in [-26, 0]");
            });
        }

        [Test]
        public void NextNatural()
        {
            int i;

            Throws<ArgumentException>(() => _dice.NextNatural(-1));
            DoesNotThrow(() => _dice.NextNatural());

            1000.Times(() =>
            {
                i = _dice.NextNatural();
                
                That(i, GreaterThanOrEqualTo(0), "i >= 0");

                i = _dice.NextNatural(50, 100);

                That(i, InRange(50, 100), "i in [50, 100]");
            });
        }

        [Test]
        public void NextChar()
        {
            char ch;
            const string pool = "abcde";

            Throws<ArgumentException>(() => _dice.NextChar(alpha: true, symbols: true));
            DoesNotThrow(() => _dice.NextChar());

            1000.Times(() =>
            {
                ch = _dice.NextChar(pool);

                IsTrue(ch.Match(@"[abcde]"), "ch.Match('[abcde]')");

                ch = _dice.NextChar(alpha: true);

                IsTrue(ch.Match(@"[a-zA-Z]"), "ch.Match(@'[a-zA-Z]')");

                ch = _dice.NextChar(alpha: true, casing: CasingRules.UpperCase);

                IsTrue(ch.Match(@"[A-Z]"), "ch.Match(@'[A-Z]')");

                ch = _dice.NextChar(alpha: true, casing: CasingRules.LowerCase);

                IsTrue(ch.Match(@"[a-z]"), "ch.Match(@'[a-z]')");
            });
        }

        [Test]
        public void NextString()
        {
            string str;
            int len;

            Throws<ArgumentException>(() => _dice.NextString(-1));
            DoesNotThrow(() => _dice.NextString());

            1000.Times(() =>
            {
                len = _dice.NextNatural(1, 25);
                str = _dice.NextString(len);

                IsTrue(str.Length == len, "str.Length == len");

                str = _dice.NextString(alpha: true);

                IsTrue(str.Match(@"[a-zA-Z]+"), "str.Match(@'[a-zA-Z]+')");

                str = _dice.NextString(alpha: true, casing: CasingRules.UpperCase);

                IsTrue(str.Match(@"[A-Z]+"), "str.Match(@'[A-Z]+')");

                str = _dice.NextString(alpha: true, casing: CasingRules.LowerCase);

                IsTrue(str.Match(@"[a-z]+"), "str.Match(@'[a-z]+')");

                str = _dice.NextString(symbols: true);

                IsTrue(str.Match(@"[\!\@\#\$\%\^\&\*\(\)\[\]]+"), @"str.Match(@'[\!\@\#\$\%\^\&\*\(\)\[\]]+')");
            });
        }

        [Test]
        public void NextDouble()
        {
            double d;

            Throws<ArgumentException>(() => _dice.NextDouble(50, 10));
            DoesNotThrow(() => _dice.NextDouble());

            1000.Times(() =>
            {
                d = _dice.NextDouble(50, 100);
                
                That(d, InRange(50.0, 100.0), "d in [50.0, 100.0]");
            });
        }

        [Test]
        public void NextList()
        {
            var generator = new Func<string, int, string>(_dice.NextEmail);

            DoesNotThrow(() => _dice.NextList<string>(generator, 1, null, 7));
            Throws<ArgumentNullException>(() => _dice.NextList<string>(null));

            var list = _dice.NextList<string>(generator, 25, "example.com", 7);

            That(list, Not.Null, "list != null");
            That(list, Not.Empty, "list.Any()");
            IsTrue(list[0].Match(@"example\.com$"), @"list[0].Match(@'example\.com$')");
        }

        [Test]
        public void GenerateUniqueList()
        {
            var generator = new Func<string, int, string>(_dice.NextEmail);

            DoesNotThrow(() => _dice.GenerateUniqueList<string>(generator, 1, null, null, 7));
            Throws<ArgumentNullException>(() => _dice.GenerateUniqueList<string>(null));

            var list = _dice.GenerateUniqueList<string>(generator, 10, null, null, 7);

            That(list, Not.Null, "list != null");
            That(list, Not.Empty, "list.Any()");
            That(list, Unique, "list.Unique()");
            That(list.Count, EqualTo(10), "list.Count == 10");
        }

        [Test]
        public void ShuffleList()
        {
            var l1 = new List<string> {"foo", "bar"};
            List<string> l2 = null;

            DoesNotThrow(() => _dice.ShuffleList(l1));
            Throws<ArgumentNullException>(() => _dice.ShuffleList(l2));

            l2 = _dice.ShuffleList(l1);

            That(l2, Not.Null, "l2 != null");
            That(l2, Not.Empty, "l2.Any()");
        }

        [Test]
        public void PickRandomItem()
        {
            List<string> l1 = null;

            Throws<ArgumentNullException>(() => _dice.PickRandomItem(l1));
            Throws<ArgumentException>(() => _dice.PickRandomItem(new List<string>()));

            l1 = new List<string> {"foo", "bar", "baz"};
            string item = null;

            DoesNotThrow(() => item = _dice.PickRandomItem(l1));

            That(item, Not.Empty);
            Contains(item, l1, "item in l1");
        }

        [Test]
        public void PickRandomSet()
        {
            List<string> l1 = null;

            Throws<ArgumentNullException>(() => _dice.PickRandomSet(l1));
            Throws<ArgumentException>(() => _dice.PickRandomSet(new List<string>()));

            l1 = new List<string> {"foo", "bar", "baz", "test"};
            List<string> set = null;

            Throws<ArgumentException>(() => _dice.PickRandomSet(l1, -1));
            DoesNotThrow(() => set = _dice.PickRandomSet(l1));

            That(set, Not.Null, "set != null");
            That(set, Not.Empty, "set.Any()");
            That(set.Count, EqualTo(2));
            Contains(set[0], l1);
            Contains(set[1], l1);
        }
    }
}
