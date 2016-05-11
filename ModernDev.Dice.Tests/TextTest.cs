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

namespace ModernDev.Dice.Tests
{
    [TestFixture]
    public class TextTest
    {
        private readonly Dice _dice = new Dice();

        [Test]
        public void NextSyllable()
        {
            string syllable = null;

            DoesNotThrow(() => syllable = _dice.NextSyllable());

            That(syllable, Not.Null, "syllable != null");
            That(syllable, Not.Empty);
            That(syllable.Length, InRange(2, 3), "syllable.Length in [2, 3]");
        }

        [Test]
        public void NextWord()
        {
            string word = null;

            Throws<ArgumentException>(() => _dice.NextWord(length: 3, syllablesCount: 3));
            DoesNotThrow(() => word = _dice.NextWord(syllablesCount: 3));

            That(word.Length, InRange(6, 9), "word.Length in [6, 9]");

            word = _dice.NextWord(syllablesCount: null, length: 5);

            That(word.Length, EqualTo(5), "word.Length == 5");
        }

        [Test]
        public void NextSentence()
        {
            string sentence = null;

            DoesNotThrow(() => sentence = _dice.NextSentence(5));

            That(sentence, Not.Empty);
            That(sentence.Split(' ').Length, EqualTo(5));
        }

        [Test]
        public void NextParagraph()
        {
            string paragraph = null;

            DoesNotThrow(() => paragraph = _dice.NextParagraph());

            That(paragraph, Not.Empty);
            That(paragraph.Split('.').Length, EqualTo(6));
        }
    }
}
