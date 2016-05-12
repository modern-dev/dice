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

namespace ModernDev.Dice.Tests
{
    [TestFixture]
    public class LocationTest
    {
        private readonly Dice _dice = new Dice();
        private string _str;

        [OneTimeSetUp]
        public void SetUp() => _str = null;

        [Test]
        public void NextLatitude() => 1000.Times(() => That(_dice.NextLatitude(), InRange(-90, 90)));

        [Test]
        public void NextLongitude() => 1000.Times(() => That(_dice.NextLongitude(), InRange(-180, 180)));

        [Test]
        public void NextGeohash() => 1000.Times(() => That(_dice.NextGeohash(), Has.Length.AtLeast(7)));

        [Test]
        public void NextZipCode() => 1000.Times(() =>
        {
            _str = _dice.NextZipCode();

            That(_str, Not.Empty);
            IsTrue(_str.Match(@"^\d{5}-\d{4}$"));

            _str = _dice.NextZipCode(false);

            IsTrue(_str.Match(@"^\d{5}$"));
        });

        [Test]
        public void NextStreet() => 1000.Times(() =>
        {
            _str = _dice.NextStreet();

            That(_str, Not.Empty);
            That(_str, Has.Length.AtLeast(4));
            That(_str.Split(' '), Has.Length.AtLeast(2));
        });

        [Test]
        public void NextState() => 1000.Times(() =>
        {
            _str = _dice.NextState();

            That(_str, Not.Empty);
            That(_str, Has.Length.EqualTo(2));

            _str = _dice.NextState(true);

            That(_str, Has.Length.GreaterThan(2));
        });

        [Test]
        public void NextPostalCode()
            => 1000.Times(() => IsTrue(_dice.NextPostalCode().Match(@"^([A-Za-z]\d[A-Za-z][-]?\d[A-Za-z]\d)")));

        [Test]
        public void NextCountry() => 1000.Times(() =>
        {
            _str = _dice.NextCountry();

            That(_str, Not.Empty);
            That(_str, Has.Length.GreaterThan(2));

            _str = _dice.NextCountry(false);

            That(_str, Has.Length.EqualTo(2));
        });

        [Test]
        public void NextCity() => 1000.Times(() => That(_dice.NextCity(), Has.Length.AtLeast(6)));

        [Test]
        public void NextAddress() => 1000.Times(() => IsTrue(_dice.NextAddress().Match(@"^\d{1,4} \w+ \w+$")));
    }
}
