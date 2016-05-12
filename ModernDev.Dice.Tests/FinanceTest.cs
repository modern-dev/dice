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

using NUnit.Framework;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static ModernDev.Dice.Utils;

namespace ModernDev.Dice.Tests
{
    [TestFixture]
    public class FinanceTest
    {
        private readonly Dice _dice = new Dice();
        private string _str;
        private long _val;

        [OneTimeSetUp]
        public void SetUp()
        {
            _str = null;
            _val = 0;
        }

        [Test]
        public void NextCurrency() => 1000.Times(() =>
        {
            _str = _dice.NextCurrency();

            That(_str, Not.Empty);
            That(_str, Has.Length.GreaterThan(3));

            _str = _dice.NextCurrency(true);

            That(_str, Has.Length.EqualTo(3));
        });

        [Test]
        public void NextCreditCardNumber() => 1000.Times(() =>
        {
            _val = _dice.NextCreditCardNumber();

            IsTrue(LuhnCheck(_val));
        });
    }
}
