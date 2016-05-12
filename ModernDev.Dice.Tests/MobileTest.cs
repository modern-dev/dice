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

namespace ModernDev.Dice.Tests
{
    [TestFixture]
    public class MobileTest
    {
        private readonly Dice _dice = new Dice();

        [Test]
        public void NextAndroidId()
            => 1000.Times(() => IsTrue(_dice.NextAndroidId().Match(@"APA91([0-9a-zA-Z-_]){178}")));

        [Test]
        public void NextApplePushToken()
            => 1000.Times(() => IsTrue(_dice.NextApplePushToken().Match(@"([0-9a-fA-F]){64}")));

        [Test]
        public void NextWP7ANID()
            => 1000.Times(() => IsTrue(_dice.NextWP7ANID().Match(@"^A=[0-9A-F]{32}&E=[0-9a-f]{3}&W=\d$")));

        [Test]
        public void NextBlackBerryPIN() => 1000.Times(() => IsTrue(_dice.NextBlackBerryPIN().Match(@"([0-9a-f]){8}")));
    }
}
