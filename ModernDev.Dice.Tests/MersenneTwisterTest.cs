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
    public class MersenneTwisterTest
    {
        [Test]
        public void Generator()
        {
            const uint seed = 321;

            using (var mt1 = new MersenneTwister(seed))
            {
                using (var mt2 = new MersenneTwister(seed))
                {
                    for (var i = 0; i < 5; i++)
                    {
                        IsTrue(mt1.GenRandInt32() == mt2.GenRandInt32(), "mt1.GenRandInt32() == mt2.GenRandInt32()");
                    }
                }
            }
        }
    }
}
