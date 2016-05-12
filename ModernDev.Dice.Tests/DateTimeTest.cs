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
using NUnit.Framework;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static ModernDev.Dice.Utils;

namespace ModernDev.Dice.Tests
{
    [TestFixture]
    public class DateTimeTest
    {
        private readonly Dice _dice = new Dice();
        private DateTime _dt;
        private readonly DateTime _dt1 = new DateTime(2010, 1, 1);
        private readonly DateTime _dt2 = new DateTime(2016, 1, 1);

        [Test]
        public void NextYear() => 1000.Times(() => That(_dice.NextYear(1992, 2144), InRange(1992, 2144)));

        [Test]
        public void NextMonth()
        {
            string str = null;

            Throws<ArgumentOutOfRangeException>(() => _dice.NextMonth(0));
            Throws<ArgumentOutOfRangeException>(() => _dice.NextMonth(max: 14));
            Throws<ArgumentOutOfRangeException>(() => _dice.NextMonth(15, 10));
            DoesNotThrow(() => str = _dice.NextMonth());

            That(str, Not.Empty);
            That(str.Length, GreaterThan(1));
        }

        [Test]
        public void NextSecond() => 1000.Times(() => That(_dice.NextSecond(), InRange(0, 59)));

        [Test]
        public void NextMinute()
        {
            Throws<ArgumentOutOfRangeException>(() => _dice.NextMinute(-1));
            Throws<ArgumentOutOfRangeException>(() => _dice.NextMinute(max: 62));
            Throws<ArgumentOutOfRangeException>(() => _dice.NextMinute(15, 10));
            DoesNotThrow(() => _dice.NextMinute());

            1000.Times(() => That(_dice.NextMinute(), InRange(0, 59)));
        }

        [Test]
        public void NextHour()
        {
            Throws<ArgumentOutOfRangeException>(() => _dice.NextHour(min: -1));
            Throws<ArgumentOutOfRangeException>(() => _dice.NextHour(max: 62));
            Throws<ArgumentOutOfRangeException>(() => _dice.NextHour(false, max: 14));
            Throws<ArgumentOutOfRangeException>(() => _dice.NextHour(true, 15, 10));
            DoesNotThrow(() => _dice.NextHour());

            1000.Times(() =>
            {
                That(_dice.NextHour(), InRange(0, 23));
                That(_dice.NextHour(false), InRange(1, 12));
            });
        }

        [Test]
        public void NextDate() => 1000.Times(() =>
        {
            _dt = _dice.NextDate();

            That(_dt, Not.Null);

            _dt = _dice.NextDate(_dt1, _dt2);

            That(_dt, Not.Null);
            That(DateTimeToUnixTimestamp(_dt), InRange(DateTimeToUnixTimestamp(_dt1), DateTimeToUnixTimestamp(_dt2)));
        });
    }
}
