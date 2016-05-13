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
    public class UtilsTest
    {
        [Test]
        public void LuhnCalcCheck()
        {
            IsTrue(LuhnCheck(49927398716));
            IsTrue(LuhnCheck(1234567812345670));
            IsFalse(LuhnCheck(49927398717));
            IsFalse(LuhnCheck(1234567812345678));
        }

        [Test]
        public void NumberPad()
        {
            That(NumberPadding(123, 5), EqualTo("00123"));
            That(NumberPadding(123, 5, 'x'), EqualTo("xx123"));
        }

        /*[Test]
        public void DateTimeConverters()
        {
            var dt = new DateTime(1992, 2, 9, 12, 30, 30);
            const int timestamp = 697631430;

            That(DateTimeToUnixTimestamp(dt), EqualTo(timestamp));

            var dt2 = UnixTimestampToDateTime(timestamp);

            That(dt2.Year, EqualTo(dt.Year));
            That(dt2.Month, EqualTo(dt.Month));
            That(dt2.Day, EqualTo(dt.Day));
        }*/
    }
}
