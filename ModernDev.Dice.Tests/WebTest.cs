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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;

namespace ModernDev.Dice.Tests
{
    [TestFixture]
    public class WebTest
    {
        private readonly Dice _dice = new Dice();
        private string _str;

        [OneTimeSetUp]
        public void SetUp() => _str = null;

        [Test]
        public void NextTopLevelDomain() => 1000.Times(() =>
        {
            _str = _dice.NextTopLevelDomain();

            That(_str, Not.Empty);
            That(_str.Length, Not.GreaterThan(6));
        });

        [Test]
        public void NextDomainName() => 1000.Times(() =>
        {
            _str = _dice.NextDomainName();

            That(_str, Not.Empty);
            That(_str.Length, GreaterThan(1));

            _str = _dice.NextDomainName("com");

            That(_str.Split('.')[1], EqualTo("com"));
        });

        [Test]
        public void NextEmail() => 1000.Times(() =>
        {
            _str = _dice.NextEmail();

            That(_str, Not.Empty);
            That(_str.Split('@').Length, GreaterThan(1));

            _str = _dice.NextEmail("modern-dev.com");

            That(_str.Split('@')[1], EqualTo("modern-dev.com"));

            _str = _dice.NextEmail(length: 5);

            That(_str.Split('@')[0].Length, EqualTo(5));
        });

        [Test]
        public void NextGoogleAnalyticsId() => 1000.Times(() =>
        {
            _str = _dice.NextGoogleAnalyticsId();

            That(_str, Not.Empty);
            That(_str.Length, EqualTo(12));
            That(_str.IndexOf("UA-", StringComparison.Ordinal), Not.EqualTo(-1));
        });

        [Test]
        public void NextHashtag() => 1000.Times(() =>
        {
            _str = _dice.NextHashtag();

            That(_str, Not.Empty);
            IsTrue(_str.Match(@"^\#\w+$"), @"_str.Match(@'^\#\w+$')");
        });

        [Test]
        public void NextIP() => 1000.Times(() =>
        {
            _str = _dice.NextIP();

            That(_str, Not.Empty);
            That(_str.Split('.').Length, EqualTo(4));
        });

        [Test]
        public void NextIPv6() => 1000.Times(() =>
        {
            _str = _dice.NextIPv6();

            That(_str, Not.Empty);
            That(_str.Split(':').Length, EqualTo(8));
        });

        [Test]
        public void NextTwitterName() => 1000.Times(() =>
        {
            _str = _dice.NextTwitterName();

            That(_str, Not.Empty);
            IsTrue(_str.Match(@"\@[A-Za-z]+"), @"_str.Match(@'\@[A-Za-z]+')");
        });

        [Test]
        public void NextColor() => 1000.Times(() =>
        {
            // hex
            _str = _dice.NextColor();

            That(_str, Not.Empty);
            That(_str.Length, EqualTo(7));
            IsTrue(_str.Match(@"#[a-z0-9]+"), "_str.Match(@'#[a-z0-9]+')");

            // shorthex
            _str = _dice.NextColor(ColorFormats.ShortHex);

            That(_str.Length, EqualTo(4));
            IsTrue(_str.Match(@"#[a-z0-9]+"), "_str.Match(@'#[a-z0-9]+')");

            // rgb
            _str = _dice.NextColor(ColorFormats.RGB);
            var rg = new Regex(@"rgb\((\d{1,3}),(\d{1,3}),(\d{1,3})\)");
            var match = rg.Match(_str);

            That(match.Groups.Count, EqualTo(4));
            That(int.Parse(match.Groups[1].Value), InRange(0, 255), "r component");
            That(int.Parse(match.Groups[2].Value), InRange(0, 255), "g component");
            That(int.Parse(match.Groups[3].Value), InRange(0, 255), "b component");

            // rgba
            _str = _dice.NextColor(ColorFormats.RGBA);
            rg = new Regex(@"rgba\((\d{1,3}),(\d{1,3}),(\d{1,3}),([01]\.?\d*?)\)");
            match = rg.Match(_str);

            That(match.Groups.Count, EqualTo(5));
            That(int.Parse(match.Groups[1].Value), InRange(0, 255), "r component");
            That(int.Parse(match.Groups[2].Value), InRange(0, 255), "g component");
            That(int.Parse(match.Groups[3].Value), InRange(0, 255), "b component");
            That(double.Parse(match.Groups[4].Value), InRange(0, 1), "a component");

            // 0x
            _str = _dice.NextColor(ColorFormats.Ox);

            That(_str.Length, EqualTo(8));
            IsTrue(_str.Match(@"0x[a-z0-9]+"), "_str.Match(@'0x[a-z0-9]+')");

            // name
            _str = _dice.NextColor(ColorFormats.Named);

            That(_str, Not.Empty);
        });

        [Test]
        public void NextUrl() => 1000.Times(() =>
        {
            _str = _dice.NextUrl();

            That(_str, Not.Empty);
            That(_str.Split('.').Length, GreaterThan(1));
            That(_str.Split(new [] {"://"}, StringSplitOptions.None).Length, GreaterThan(1));

            _str = _dice.NextUrl(domain: "modern-dev.com");

            That(_str.Split('.').Length, GreaterThan(1));
            That(_str.Split(new[] { "://" }, StringSplitOptions.None).Length, GreaterThan(1));
            That(_str.Split(new[] {"modern-dev.com"}, StringSplitOptions.None).Length, GreaterThan(1));

            _str = _dice.NextUrl(domainPrefix: "www");

            That(_str.Split('.').Length, GreaterThan(1));
            That(_str.Split(new[] { "://" }, StringSplitOptions.None).Length, GreaterThan(1));
            That(_str.Split(new[] { "www." }, StringSplitOptions.None).Length, GreaterThan(1));

            _str = _dice.NextUrl(path: "docs");

            That(_str.Split('.').Length, GreaterThan(1));
            That(_str.Split(new[] { "://" }, StringSplitOptions.None).Length, GreaterThan(1));
            That(_str.Split(new[] { "/docs" }, StringSplitOptions.None).Length, GreaterThan(1));

            _str = _dice.NextUrl(extensions: new List<string> {"html"});

            That(_str.Split('.').Length, GreaterThan(1));
            That(_str.Split(new[] { "://" }, StringSplitOptions.None).Length, GreaterThan(1));
            That(_str.Split(new[] { ".html" }, StringSplitOptions.None).Length, GreaterThan(1));
        });
    }
}
