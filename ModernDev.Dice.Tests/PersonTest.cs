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
    public class PersonTest
    {
        private readonly Dice _dice = new Dice();
        private int _val;
        private string _str;

        [OneTimeSetUp]
        public void SetUp()
        {
            _val = 0;
            _str = null;
        }

        [Test]
        public void NextAge() => 1000.Times(() =>
        {
            _val = _dice.NextAge(AgeRanges.All);

            That(_val, InRange(1, 100));

            _val = _dice.NextAge(AgeRanges.Child);

            That(_val, InRange(1, 12));

            _val = _dice.NextAge(AgeRanges.Teen);

            That(_val, InRange(13, 19));

            _val = _dice.NextAge();

            That(_val, InRange(18, 65));

            _val = _dice.NextAge(AgeRanges.Senior);

            That(_val, InRange(65, 100));
        });

        [Test]
        public void NextSex() => 1000.Times(() => IsTrue(_dice.NextSex().Match(@"(Male|Female)")));

        [Test]
        public void NextCPF() => 1000.Times(() => IsTrue(_dice.NextCPF().Match(@"^\d{3}.\d{3}.\d{3}-\d{2}$")));

        [Test]
        public void NextFirstName() => 1000.Times(() =>
        {
            _str = _dice.NextFirstName();

            That(_str, Not.Empty);
            That(_str, Has.Length.InRange(2, 20));
            That(_str.Split(' '), Has.Length.EqualTo(1));
        });

        [Test]
        public void NextLastName() => 1000.Times(() =>
        {
            _str = _dice.NextLastName();

            That(_str, Not.Empty);
            That(_str, Has.Length.InRange(2, 20));
            That(_str.Split(' '), Has.Length.EqualTo(1));
        });

        [Test]
        public void NextNamePrefix() => 1000.Times(() =>
        {
            _str = _dice.NextNamePrefix();

            That(_str, Not.Empty);
            That(_str, Has.Length.LessThanOrEqualTo(5));

            _str = _dice.NextNamePrefix(gender: "female");

            That(_str, Not.EqualTo("Mr."));

            _str = _dice.NextNamePrefix(gender: "male");

            That(_str, Not.Contains(new[] {"Mrs.", "Miss"}));

            _str = _dice.NextNamePrefix(true);

            That(_str, Has.Length.GreaterThanOrEqualTo(3));
        });

        [Test]
        public void NextNameSuffix() => 1000.Times(() =>
        {
            _str = _dice.NextNameSuffix();

            That(_str, Not.Empty);
            That(_str, Has.Length.Not.GreaterThan(7));

            _str = _dice.NextNameSuffix(true);

            That(_str, Has.Length.GreaterThanOrEqualTo(5));
        });

        [Test]
        public void NextNationality() => 1000.Times(() => That(_dice.NextNationality(), Has.Length.InRange(3, 26)));

        [Test]
        public void NextSSN() => 1000.Times(() =>
        {
            _str = _dice.NextSSN();

            That(_str, Not.Empty);
            That(_str, Has.Length.EqualTo(11));
            IsTrue(_str.Match(@"^\d{3}-\d{2}-\d{4}$"));

            _str = _dice.NextSSN(true);

            That(_str, Has.Length.EqualTo(4));
            IsTrue(_str.Match(@"^\d{4}$"));
        });

        [Test]
        public void NextName() => 1000.Times(() =>
        {
            _str = _dice.NextName();

            That(_str, Not.Empty);
            That(_str, Has.Length.InRange(2, 30));
            That(_str.Split(' '), Has.Length.EqualTo(2));

            _str = _dice.NextName(middle: true);

            That(_str.Split(' '), Has.Length.EqualTo(3));

            _str = _dice.NextName(middleInitial: true);

            That(_str.Split(' '), Has.Length.EqualTo(3));
            IsTrue(_str.Split(' ')[1].Match(@"[A-Z]\."));
            That(_str.IndexOf('.'), GreaterThan(-1));

            _str = _dice.NextName(prefix: true);

            That(_str.Split(' '), Has.Length.EqualTo(3));

            _str = _dice.NextName(suffix: true);

            That(_str.Split(' '), Has.Length.EqualTo(3));
        });
    }
}
