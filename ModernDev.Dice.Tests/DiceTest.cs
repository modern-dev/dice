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

namespace ModernDev.Dice.Tests
{
    [TestFixture]
    public class DiceTest
    {
        private Dice _dice;
        private readonly Func<double> _testGenerator = () => 0.5;

        [Test]
        public void Initialization()
        {
            Assert.Throws<ArgumentNullException>(() => _dice = new Dice(null));
            Assert.DoesNotThrow(() => _dice = new Dice(456));
            Assert.DoesNotThrow(() => _dice = new Dice(_testGenerator));
            Assert.DoesNotThrow(() => _dice = new Dice());
        }

        [Test]
        public void Seeding()
        {
            _dice = new Dice(123);
            var d2 = new Dice(789);
            int i1, i2;

            1000.Times(() =>
            {
                i1 = _dice.NextNatural(0, 10000000);
                i2 = d2.NextNatural(0, 10000000);
                
                Assert.That(i1, Is.Not.EqualTo(i2), "i1 != i2");
            });

            _dice = new Dice(456);
            d2 = new Dice(456);

            1000.Times(() =>
            {
                i1 = _dice.NextNatural(0, 10000000);
                i2 = d2.NextNatural(0, 10000000);

                Assert.That(i1, Is.EqualTo(i2), "i1 == i2");
            });
        }

        [Test]
        public void CustomRandomGenerator()
        {
            _dice = new Dice(_testGenerator);
            var i1 = _dice.NextNatural(0, 10000000);

            100.Times(() => Assert.IsTrue(i1 == _dice.NextNatural(0, 10000000), "i1 == _dice.NextNatural(0, 10000000)"));
        }
    }
}