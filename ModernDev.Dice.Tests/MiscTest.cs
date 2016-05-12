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
    public class MiscTest
    {
        private readonly Dice _dice = new Dice();
        private string _str;

        [OneTimeSetUp]
        public void SetUp() => _str = null;

        [Test]
        public void NextGUID() => 1000.Times(() => IsTrue(_dice.NextGUID(1).Match(
            @"([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-1([0-9a-fA-F]){3}-([ab89])([0-9a-fA-F]){3}-([0-9a-fA-F]){12}")));

        [Test]
        public void NextHash() => 1000.Times(() =>
        {
            _str = _dice.NextHash();

            That(_str, Not.Empty);
            That(_str, Has.Length.EqualTo(40));
            IsTrue(_str.Match(@"([0-9a-f]){40}"));

            _str = _dice.NextHash(64);

            That(_str, Has.Length.EqualTo(64));
        });
    }
}
