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
using System.Linq;
using System.Text.RegularExpressions;
using static ModernDev.Dice.Dice.Data;

namespace ModernDev.Dice
{
    /// <summary>
    /// Represents a pseudo-random object generator.
    /// </summary>
    public partial class Dice
    {
        #region Fields

        private readonly Func<double> _random;

        #endregion

        #region Constructors

        private Dice(uint? seed, bool test)
        {
            var mt = seed.HasValue && !test? new MersenneTwister(seed.Value) : new MersenneTwister();
            _random = mt.GenRandReal2;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Dice"/> class, using a time-dependent default seed value and default random generator function.
        /// </summary>
        public Dice() : this(null, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Dice"/> class, using the specified seed value.
        /// </summary>
        /// <param name="seed">A number used to calculate a starting value for the pseudo-random number sequence.</param>
        public Dice(uint seed) : this(seed, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Dice"/> class, using the specified random generator function.
        /// </summary>
        /// <param name="randomGenerator">A function used to generate random <see cref="double"/> values.</param>
        /// <exception cref="ArgumentNullException">Thrown when given <c>randomGenerator</c> is null.</exception>
        public Dice(Func<double> randomGenerator)
        {
            if (randomGenerator == null)
            {
                throw new ArgumentNullException(nameof(randomGenerator));
            }

            _random = randomGenerator;
        }

        #endregion

        #region Methods

        #region Basics

        /// <summary>
        /// Returns a random bool, either true or false.
        /// </summary>
        /// <param name="likelihood">The default likelihood of success (returning true) is 50%.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>likelihood</c> is less than 0 or greater than 100.</exception>
        /// <returns>Returns a random bool, either true or false.</returns>
        public bool NextBool(int likelihood = 50)
        {
            if (likelihood < 0 || likelihood > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(likelihood), "Likelihood accepts values from 0 to 100.");
            }

            return _random()*100 < likelihood;
        }

        /// <summary>
        /// Returns a random integer.
        /// </summary>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <exception cref="ArgumentException">Thrown when <c>min</c> is greater than <c>max</c>.</exception>
        /// <returns>Returns a random integer.</returns>
        public int NextInt(int min = int.MinValue, int max = int.MaxValue)
        {
            if (min > max)
            {
                throw new ArgumentException("Min cannot be greater than Max.", nameof(min));
            }

            return (int) Math.Floor(_random()*(max - min + 1) + min);
        }

        /// <summary>
        /// Returns a random long.
        /// </summary>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <exception cref="ArgumentException">Thrown when <c>min</c> is greater than <c>max</c>.</exception>
        /// <returns>Returns a random long.</returns>
        public long NextLong(long min = long.MinValue, long max = long.MaxValue)
        {
            if (min > max)
            {
                throw new ArgumentException("Min cannot be greater than Max.", nameof(min));
            }

            return (long) Math.Floor(_random()*(max - min + 1) + min);
        }

        /// <summary>
        /// Returns a random natural integer.
        /// </summary>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <exception cref="ArgumentException">Thrown when <c>min</c> is less than 0.</exception>
        /// <returns>Returns a random natural integer.</returns>
        public int NextNatural(int min = 0, int max = int.MaxValue)
        {
            if (min < 0)
            {
                throw new ArgumentException("Min cannot be less than zero.", nameof(min));
            }

            return NextInt(min, max);
        }

        /// <summary>
        /// Returns a random character.
        /// </summary>
        /// <param name="pool">Characters pool</param>
        /// <param name="alpha">Set to True to use only an alphanumeric character.</param>
        /// <param name="symbols">Set to true to return only symbols.</param>
        /// <param name="casing">Default casing.</param>
        /// <returns>Returns a random character.</returns>
        public char NextChar(string pool = null, bool alpha = false, bool symbols = false, CasingRules casing = CasingRules.MixedCase)
        {
            const string s = "!@#$%^&*()[]";
            string letters, p;

            if (casing == CasingRules.LowerCase)
            {
                letters = CharsLower;
            }
            else if (casing == CasingRules.UpperCase)
            {
                letters = CharsUpper;
            }
            else
            {
                letters = CharsLower + CharsUpper;
            }

            if (!string.IsNullOrEmpty(pool))
            {
                p = pool;
            }
            else if (alpha)
            {
                p = letters;
            }
            else if (symbols)
            {
                p = s;
            }
            else
            {
                p = letters + Numbers + s;
            }

            return p[NextNatural(max: p.Length - 1)];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public double NextDouble(double min = double.MinValue, double max = double.MaxValue, uint decimals = 4)
        {
            var _fixed = Math.Pow(10, decimals);
            var num = NextLong((int) (min*_fixed), (int) (max*_fixed));
            var numFixed = (num/_fixed).ToString("N" + decimals);

            return double.Parse(numFixed);
        }

        /// <summary>
        /// Returns a random string.
        /// </summary>
        /// <param name="length">The length of the string.</param>
        /// <param name="args">Arguments that will be passed to the <see cref="NextChar"/> method.</param>
        /// <exception cref="ArgumentException">Thrown when <c>length</c> is less than 0.</exception>
        /// <returns>Returns a random string.</returns>
        public string NextString(int length = 10, params object[] args)
        {
            if (length < 0)
            {
                throw new ArgumentException("Length cannot be less than zero.", nameof(length));
            }

            return string.Join("", NextList<string>(new Func<string, bool, bool, CasingRules, char>(NextChar), length, args));
        }

        /// <summary>
        /// Returns a list of n random terms.
        /// </summary>
        /// <param name="generator">Generator function to produce items.</param>
        /// <param name="count">The count of produced items.</param>
        /// <param name="args">The arguments list that will be passed to the generator function.</param>
        /// <exception cref="ArgumentNullException">Thrown when <c>generator</c> is null.</exception>
        /// <returns>Returns a list of n random terms.</returns>
        public List<T> NextList<T>(Delegate generator, int count = 1, params object[] args)
        {
            if (generator == null)
            {
                throw new ArgumentNullException(nameof(generator));
            }

            var i = count > 0 ? count : 1;
            var arr = new List<T>();

            for (; i-- != 0;)
            {
                arr.Add((T) (args.Any() ? generator.DynamicInvoke(args) : generator.DynamicInvoke()));
            }

            return arr;
        }

        /// <summary>
        /// Given a function that generates something random and a number of items to generate.
        /// </summary>
        /// <typeparam name="T">The type of the returned list.</typeparam>
        /// <param name="generator">The function that generates something random.</param>
        /// <param name="count">Number of terms to generate.</param>
        /// <param name="comparator">Comparator function.</param>
        /// <param name="args">The arguments list that will be passed to the generator function.</param>
        /// <exception cref="ArgumentNullException">Thrown when <c>generator</c> is null.</exception>
        /// <returns>Returns an array of items where none repeat.</returns>
        public List<T> GenerateUniqueList<T>(Delegate generator, int count = 1, Func<List<T>, T, bool> comparator = null,
            params object[] args)
        {
            if (generator == null)
            {
                throw new ArgumentNullException(nameof(generator));
            }

            var cmprt = comparator ?? ((arr, item) => arr.IndexOf(item) != -1);

            var list = new List<T>();
            int cnt = 0, maxDuplicates = count*50;

            while (list.Count < count)
            {
                var res = (T) generator.DynamicInvoke(args);

                if (!cmprt(list, res))
                {
                    list.Add(res);

                    cnt = 0;
                }

                if (++cnt > maxDuplicates)
                {
                    throw new RankException("count is likely too large for sample set");
                }
            }

            return list;
        }

        /// <summary>
        /// Given a list, scramble the order and return it.
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="list">A list to shuffle.</param>
        /// <returns>Returns shuffled list.</returns>
        public List<T> ShuffleList<T>(List<T> list)
        {
            var oldList = list.ToList();
            var newList = new List<T>();
            var len = oldList.Count;

            for (var i = 0; i < len; i++)
            {
                var j = NextNatural(max: oldList.Count - 1);

                newList.Add(oldList[j]);
                oldList.RemoveAt(j);
            }

            return newList;
        }

        /// <summary>
        /// Given a list, returns a single random element.
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="list">The list to pick from.</param>
        /// <exception cref="ArgumentNullException">Thrown when given list is null.</exception>
        /// <exception cref="ArgumentException">Thrown when given list is empty.</exception>
        /// <returns>Returns random item from the given list.</returns>
        public T PickRandomItem<T>(List<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (!list.Any())
            {
                throw new ArgumentException("Cannot pick from an empty list.", nameof(list));
            }

            return list[NextNatural(max: list.Count - 1)];
        }

        /// <summary>
        /// Given a list, returns a random set with <c>count</c> elements.
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="list">THe list to pick from.</param>
        /// <param name="count">Number of items to pick.</param>
        /// <exception cref="ArgumentNullException">Thrown when given list is null.</exception>
        /// <exception cref="ArgumentException">Thrown when given list is empty.</exception>
        /// <exception cref="ArgumentException">Thrown when <c>count</c> is less than 0.</exception>
        /// <returns>Returns the set of picked items.</returns>
        public List<T> PickRandomSet<T>(List<T> list, int count = 2)
        {
            if (count == 0)
            {
                return new List<T>();
            }

            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (!list.Any())
            {
                throw new ArgumentException("Cannot pick from an empty list.", nameof(list));
            }

            if (count < 0)
            {
                throw new ArgumentException("count must be positive number.", nameof(count));
            }

            return count == 1 ? new List<T>(new[] {PickRandomItem(list)}) : ShuffleList(list).Take(count).ToList();
        }

        /// <summary>
        /// Returns a single item from a list with relative weighting of odds.
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="list">A list of items.</param>
        /// <param name="weights">A list of items specifying the relative weights.</param>
        /// <param name="trim">True, to trim the list.</param>
        /// <exception cref="ArgumentException">Thrown when lists counts is not the same.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the sum of all the items in <c>weights</c> list is 0.</exception>
        /// <returns>Returns the item from the list that obeying the specified weight.</returns>
        public T Weighted<T>(List<T> list, List<int> weights, bool trim = false)
        {
            if (list.Count != weights.Count)
            {
                throw new ArgumentException("Length of list and weights must match.");
            }

            var sum = weights.Where(val => val > 0).Sum();

            if (sum == 0)
            {
                throw new InvalidOperationException("No valid entries in array weights.");
            }

            var selected = _random()*sum;

            int total = 0, lastGoodIndx = -1, chosenIndx = -1;

            for (var wi = 0; wi < weights.Count; ++wi)
            {
                var val = weights[wi];

                total += val;

                if (val > 0)
                {
                    if (selected <= total)
                    {
                        chosenIndx = wi;

                        break;
                    }

                    lastGoodIndx = wi;
                }

                if (wi == weights.Count - 1)
                {
                    chosenIndx = lastGoodIndx;
                }
            }

            var chosen = list[chosenIndx];

            if (trim)
            {
                list.RemoveAt(chosenIndx);
                weights.RemoveAt(chosenIndx);
            }

            return chosen;
        }

        #endregion

        #region Text

        /// <summary>
        /// Return a random paragraph generated from sentences populated by semi-pronounceable random (nonsense) words.
        /// </summary>
        /// <param name="sentencesCount">Number of sentences.</param>
        /// <returns>Returns random generated paragraph.</returns>
        public string NextParagraph(int sentencesCount = 5)
            => string.Join(" ", NextList<string>(new Func<int, bool, string>(NextSentence), sentencesCount, 15, true));

        /// <summary>
        /// Return a random sentence populated by semi-pronounceable random (nonsense) words.
        /// </summary>
        /// <param name="wordsCount">Number of words.</param>
        /// <param name="punctuation">True to use punctuation.</param>
        /// <returns>Returns random generated sentence.</returns>
        public string NextSentence(int wordsCount = 15, bool punctuation = true)
        {
            var punct = "";
            var text = string.Join(" ",
                NextList<string>(new Func<bool, int?, int?, string>(NextWord), wordsCount, false, 2, null)).Capitalize();

            if (punctuation && !new Regex(@"/^[\.\?;!:]$/", RegexOptions.IgnoreCase).IsMatch(text))
            {
                punct = ".";
            }

            if (punctuation)
            {
                text += punct;
            }

            return text;
        }

        /// <summary>
        /// Return a semi-pronounceable random (nonsense) word.
        /// </summary>
        /// <param name="capitalize">True to capitalize a word.</param>
        /// <param name="syllablesCount">Number of syllables which the word will have.</param>
        /// <param name="length">Length of a word.</param>
        /// <returns>Returns random generated word.</returns>
        public string NextWord(bool capitalize = false, int? syllablesCount = 2, int? length = null)
        {
            if (syllablesCount.HasValue && length.HasValue)
            {
                throw new ArgumentException("Cannot specify both syllablesCount AND length.");
            }

            var text = "";

            if (length.HasValue)
            {
                do
                {
                    text += NextSyllable();
                } while (text.Length < length.Value);

                text = text.Substring(0, length.Value);
            }
            else if (syllablesCount.HasValue)
            {
                for (var i = 0; i < syllablesCount.Value; i++)
                {
                    text += NextSyllable();
                }
            }

            if (capitalize)
            {
                text = text.Capitalize();
            }

            return text;
        }

        /// <summary>
        /// Return a semi-speakable syllable, 2 or 3 letters.
        /// </summary>
        /// <param name="length">Length of a syllable.</param>
        /// <param name="capitalize">True to capitalize a syllable.</param>
        /// <returns>Returns random generated syllable.</returns>
        public string NextSyllable(int length = 2, bool capitalize = false)
        {
            const string consonats = "bcdfghjklmnprstvwz";
            const string vowels = "aeiou";
            const string all = consonats + vowels;
            var text = "";
            var chr = -1;

            for (var i = 0; i < length; i++)
            {
                if (i == 0)
                {
                    chr = NextChar(all);
                } else if (consonats[chr] == -1)
                {
                    chr = NextChar(consonats);
                }
                else
                {
                    chr = NextChar(vowels);
                }

                text += (char) chr;
            }

            if (capitalize)
            {
                text = text.Capitalize();
            }

            return text;
        }

        #endregion

        #region Person

        /// <summary>
        /// Generates a random age
        /// </summary>
        /// <param name="types">Age range.</param>
        /// <returns>Returns random generated age.</returns>
        public int NextAge(AgeRanges types = AgeRanges.Adult)
        {
            int[] range;

            switch (types)
            {
                case AgeRanges.Child:
                    range = new[] {1, 12};
                    break;

                case AgeRanges.Teen:
                    range = new[] {13, 19};
                    break;

                case AgeRanges.Senior:
                    range = new[] {65, 100};
                    break;

                case AgeRanges.All:
                    range = new[] {1, 100};
                    break;

                default:
                    range = new[] {18, 65};
                    break;
            }

            return NextNatural(range[0], range[1]);
        }

        /// <summary>
        /// Generates a random Brazilian tax Id.
        /// </summary>
        /// <returns>Returns random generated Brazilian tax Id.</returns>
        public string NextCPF()
        {
            var n = NextList<int>(new Func<int, int, int>(NextNatural), 9, 0, 9).ToArray();
            var d1 = n[8]*2 + n[7]*3 + n[6]*4 + n[5]*5 + n[4]*6 + n[3]*7 + n[2]*8 + n[1]*9 + n[0]*10;

            d1 = 11 - d1%11;

            if (d1 >= 10)
            {
                d1 = 0;
            }

            var d2 = d1*2 + n[8]*3 + n[7]*4 + n[6]*5 + n[5]*6 + n[4]*7 + n[3]*8 + n[2]*9 + n[1]*10 + n[0]*11;

            d2 = 11 - (d2%11);

            if (d2 >= 10)
            {
                d2 = 0;
            }

            return "" + n[0] + n[1] + n[2] + '.' + n[3] + n[4] + n[5] + '.' + n[6] + n[7] + n[8] + '-' + d1 + d2;
        }

        /// <summary>
        /// Generates a random Brazilian company Id.
        /// </summary>
        /// <returns>Returns random generated Brazilian company Id.</returns>
        public string NextCNPJ()
        {
            var n = NextList<int>(new Func<int, int, int>(NextNatural), 12, 0, 12);
            var d1 = n[11]*2 + n[10]*3 + n[9]*4 + n[8]*5 + n[7]*6 + n[6]*7 + n[5]*8 + n[4]*9 + n[3]*2 + n[2]*3 + n[1]*4 +
                     n[0]*5;

            d1 = 11 - (d1%11);

            if (d1 < 2)
            {
                d1 = 0;
            }

            var d2 = d1*2 + n[11]*3 + n[10]*4 + n[9]*5 + n[8]*6 + n[7]*7 + n[6]*8 + n[5]*9 + n[4]*2 + n[3]*3 + n[2]*4 +
                     n[1]*5 + n[0]*6;

            d2 = 11 - (d2%11);

            if (d2 < 2)
            {
                d2 = 0;
            }

            return "" + n[0] + n[1] + '.' + n[2] + n[3] + n[4] + '.' + n[5] + n[6] + n[7] + '/' + n[8] + n[9] + n[10] +
                   n[11] + '-' + d1 + d2;
        }

        /// <summary>
        /// Generates a random first name.
        /// </summary>
        /// <param name="isFemale">True to generate female names.</param>
        /// <returns>Returns random generated first name.</returns>
        public string NextFirstName(bool isFemale = false)
            => PickRandomItem(isFemale ? FirstNamesFemale : FirstNamesMale);

        /// <summary>
        /// Generates a random last name.
        /// </summary>
        /// <returns>Returns random generated last name.</returns>
        public string NextLastName() => PickRandomItem(LastNames);

        /// <summary>
        /// Generates a random gender.
        /// </summary>
        /// <returns>Returns either <c>Male</c> or <c>Female</c>.</returns>
        public string NextSex() => PickRandomItem(new List<string> {"Male", "Female"});

        /// <summary>
        /// Generates a random name.
        /// </summary>
        /// <param name="isMale">True to generate male names.</param>
        /// <param name="middle">True to use middle names.</param>
        /// <param name="middleInitial">True to use middle initial.</param>
        /// <param name="prefix">True to use name prefixes.</param>
        /// <param name="suffix">True to use name suffixes.</param>
        /// <param name="isFull">True to use full name preffixes.</param>
        /// <param name="gender">Male of Female.</param>
        /// <returns>Returns random generated name.</returns>
        public string NextName(bool isMale = false, bool middle = false, bool middleInitial = false, bool prefix = false,
            bool suffix = false, bool isFull = false, string gender = "all")
        {
            string first = NextFirstName(isMale),
                last = NextLastName(),
                name;

            if (middle)
            {
                name = $"{first} {NextFirstName(isMale)} {last}";
            } else if (middleInitial)
            {
                name = $"{first} {NextChar(alpha: true, casing: CasingRules.UpperCase)}. {last}";
            }
            else
            {
                name = $"{first} {last}";
            }

            if (prefix)
            {
                name = NextNamePrefix(isFull, gender) + " " + name;
            }

            if (suffix)
            {
                name = name + " " +NextNameSuffix(isFull);
            }

            return name;
        }

        /// <summary>
        /// Generates a random social security number.
        /// </summary>
        /// <param name="ssnFour">True to generate last 4 digits.</param>
        /// <param name="dashes">False to remove dashes.</param>
        /// <returns>Returns random generated social security number.</returns>
        public string NextSSN(bool ssnFour = false, bool dashes = false)
        {
            const string ssnPool = "1234567890";
            string ssn, dash = dashes ? "-" : "";

            if (!ssnFour)
            {
                ssn = NextString(3, ssnPool, false, false, CasingRules.MixedCase) + dash + NextString(2, ssnPool, false, false, CasingRules.MixedCase) +
                      dash + NextString(4, ssnPool, false, false, CasingRules.MixedCase);
            }
            else
            {
                ssn = NextString(4, ssnPool, false, false, CasingRules.MixedCase);
            }

            return ssn;
        }

        /// <summary>
        /// Generates a random name suffix.
        /// </summary>
        /// <param name="isFull">True to return full suffix.</param>
        /// <returns>Returns random generated name suffix.</returns>
        public string NextNameSuffix(bool isFull = false)
            => isFull ? PickRandomItem(NameSuffixes).Item1 : PickRandomItem(NameSuffixes).Item2;

        private static List<Tuple<string, string>> NamePrefixes(string gender)
        {
            var list = new List<Tuple<string, string>>
            {
                {"Doctor", "Dr."}
            };

            if (gender == "male" || gender == "all")
            {
                list.Add("Mister", "Mr.");
            }

            if (gender == "female" || gender == "all")
            {
                list.Add("Miss", "Miss");
                list.Add("Misses", "Mrs.");
            }

            return list;
        }

        /// <summary>
        /// Generates a random name prefix.
        /// </summary>
        /// <param name="full">True to return full prefix.</param>
        /// <param name="gender">All, Male or Female.</param>
        /// <returns>Returns random generated name prefix.</returns>
        public string NextNamePrefix(bool full = false, string gender = "all")
            => full
                ? PickRandomItem(NamePrefixes(gender.ToLowerInvariant())).Item1
                : PickRandomItem(NamePrefixes(gender.ToLowerInvariant())).Item2;

        /// <summary>
        /// Generates a random nationality.
        /// </summary>
        /// <returns>Returns random generated nationality.</returns>
        public string NextNationality() => PickRandomItem(Nationalities);

        #endregion

        #region Mobile

        /// <summary>
        /// Returns a random GCM registration ID.
        /// </summary>
        /// <returns>Returns a random GCM registration ID.</returns>
        public string NextAndroidId()
            => "APA91" +
               NextString(178, "0123456789abcefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-_", false, false, CasingRules.MixedCase);

        /// <summary>
        /// Returns a random Apple Push Token.
        /// </summary>
        /// <returns>Returns a random Apple Push Token.</returns>
        public string NextApplePushToken() => NextString(64, "abcdef1234567890", false, false, CasingRules.MixedCase);

        /// <summary>
        /// Returns a random Windows Phone 7 ANID.
        /// </summary>
        /// <returns>Returns a random Windows Phone 7 ANID.</returns>
        public string NextWP7ANID()
            => "A=" + new Regex(@"/-/g", RegexOptions.Multiline).Replace(NextGUID(), string.Empty).ToUpperInvariant() +
               "&E=" + NextHash(3) + "&W=" + NextInt(0, 9);

        /// <summary>
        /// Returns a random BlackBerry Device PIN.
        /// </summary>
        /// <returns>Returns a random BlackBerry Device PIN.</returns>
        public string NextBlackBerryPIN() => NextHash(8);

        #endregion

        #region Web

        /// <summary>
        /// Returns a random domain with a random Top Level Domain.
        /// </summary>
        /// <param name="domain">Custom Top Level Domain.</param>
        /// <returns>Returns random generated domain name.</returns>
        public string NextDomainName(string domain = null) => NextWord() + "." + (domain ?? PickRandomItem(Tlds));

        /// <summary>
        /// Returns a random email with a random domain.
        /// </summary>
        /// <param name="domain">Custom Top Level Domain</param>
        /// <param name="length">Length of an email address.</param>
        /// <returns>Returns a random email with a random domain.</returns>
        public string NextEmail(string domain = null, int length = 7)
            => NextWord(length: length) + "@" + (domain ?? NextDomainName());

        /// <summary>
        /// Returns a random Google Analytics tracking code.
        /// </summary>
        /// <returns>Returns a random Google Analytics tracking code.</returns>
        public string NextGoogleAnalyticsId()
            => $"UA-{Utils.NumberPadding(NextNatural(max: 999999), 6)}-{Utils.NumberPadding(NextNatural(max: 99))}";

        /// <summary>
        /// Returns a random hashtag. This is a string of the form ‘#thisisahashtag’.
        /// </summary>
        /// <returns>Returns a random hashtag.</returns>
        public string NextHashtag() => $"#{NextWord()}";

        /// <summary>
        /// Returns a random IP Address.
        /// </summary>
        /// <returns>Returns a random IP Address.</returns>
        public string NextIP()
            => $"{NextNatural(1, 254)}.{NextNatural(0, 255)}.{NextNatural(0, 255)}.{NextNatural(1, 254)}";

        /// <summary>
        /// Returns a random IPv6 Address.
        /// </summary>
        /// <returns>Returns a random IPv6 Address.</returns>
        public string NextIPv6()
            => string.Join(":", NextList<string>(new Func<int, CasingRules, string>(NextHash), 8, 4, CasingRules.LowerCase));

        /// <summary>
        /// Returns a random twitter handle.
        /// </summary>
        /// <returns>Returns a random twitter handle.</returns>
        public string NextTwitterName() => $"@{NextWord()}";

        /// <summary>
        /// Returns a random url.
        /// </summary>
        /// <param name="protocol">Custom protocol.</param>
        /// <param name="domain">Custom domain.</param>
        /// <param name="domainPrefix">Custom domain prefix.</param>
        /// <param name="path">Url path.</param>
        /// <param name="extensions">A list of Url extensions to pick one from.</param>
        /// <returns>Returns a random url.</returns>
        public string NextUrl(string protocol = "http", string domain = null, string domainPrefix = null, string path = null, List<string> extensions = null)
        {
            domain = domain ?? NextDomainName();
            var ext = extensions != null && extensions.Any() ? "." + PickRandomItem(extensions) : "";
            var dom = !string.IsNullOrEmpty(domainPrefix) ? domainPrefix + "." + domain : domain;

            return $"{protocol}://{dom}/{path}{ext}";
        }

        /// <summary>
        /// Returns a random color.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="grayscale">True then only grayscale colors be generated.</param>
        /// <param name="casing"></param>
        /// <returns>Returns a random color.</returns>
        public string NextColor(ColorFormats format = ColorFormats.Hex, bool grayscale = false, CasingRules casing = CasingRules.LowerCase)
        {
            Func<object, string, string> gray = (val, delimiter) => string.Join(delimiter ?? "", val, val, val);
            Func<int, int, bool, bool, string> hex = (start, end, withHash, isGrayscale) =>
            {
                var symbol = withHash ? "#" : "";
                var expr = isGrayscale ? gray(NextHash(start), "") : NextHash(end);

                return $"{symbol}{expr}";
            };
            Func<bool, bool, string> rgb = (hasAlpha, isGrayscale) =>
            {
                var rgbVal = hasAlpha ? "rgba" : "rgb";
                var alphaChan = hasAlpha ? $",{NextDouble(0,1)}" : "";
                var colorVal = isGrayscale
                    ? gray(NextNatural(max: 255), ",")
                    : NextNatural(max: 255) + "," + NextNatural(max: 255) + "," + NextNatural(max: 255);

                return $"{rgbVal}({colorVal}{alphaChan})";
            };

            string cv;

            switch (format)
            {
                case ColorFormats.ShortHex:
                    cv = hex(1, 2, true, grayscale);
                    break;

                case ColorFormats.RGB:
                    cv = rgb(false, grayscale);
                    break;

                case ColorFormats.RGBA:
                    cv = rgb(true, grayscale);
                    break;

                case ColorFormats.Ox:
                    cv = "0x" + hex(2, 6, false, grayscale);
                    break;

                case ColorFormats.Named:
                    return PickRandomItem(ColorNames);

                default:
                    cv = hex(2, 6, true, grayscale);
                    break;
            }

            if (casing == CasingRules.UpperCase)
            {
                cv = cv.ToUpperInvariant();
            }

            return cv;
        }

        #endregion

        #region Location

        /// <summary>
        /// Generates a random (U.S.) zip code.
        /// </summary>
        /// <param name="plusfour">True to return a Zip+4.</param>
        /// <returns>Returns random generated U.S. zip code.</returns>
        public string NextZipCode(bool plusfour = true)
        {
            var zip = NextList<object>(new Func<int, int, int>(NextNatural), 5, 0, 9);

            if (!plusfour)
            {
                return string.Join("", zip);
            }

            zip.Add("-");
            zip.AddRange(NextList<object>(new Func<int, int, int>(NextNatural), 4, 0, 9));

            return string.Join("", zip);
        }

        private KeyValuePair<string, string> StreetSuffix() => PickRandomItem(StreetSuffixes);

        /// <summary>
        /// Generates a random street.
        /// </summary>
        /// <param name="syllables">Number of syllables.</param>
        /// <param name="shortSuffix">True to use short suffix.</param>
        /// <returns>Returns random generated street name.</returns>
        public string NextStreet(int syllables = 2, bool shortSuffix = true)
            => NextWord(syllablesCount: syllables).Capitalize() + (shortSuffix
                ? StreetSuffix().Key
                : StreetSuffix().Value);

        /// <summary>
        /// Returns a random U.S. state.
        /// </summary>
        /// <param name="fullName">True to return full name.</param>
        /// <returns>Returns a random U.S. state.</returns>
        public string NextState(bool fullName = false)
            => fullName ? PickRandomItem(UsStates).Key : PickRandomItem(UsStates).Value;

        /// <summary>
        /// Returns a Canadian Postal code. Returned postal code is valid with respect to the Postal District (first character) and format only.
        /// </summary>
        /// <returns>Returns a Canadian Postal code.</returns>
        public string NextPostalCode()
            =>$"{NextChar("XVTSRPNKLMHJGECBA")}{NextNatural(max: 9)}{NextChar(alpha: true, casing: CasingRules.UpperCase)}-{NextNatural(max: 9)}{NextChar(alpha: true, casing: CasingRules.UpperCase)}{NextNatural(max: 9)}";

        /// <summary>
        /// Generates a random longitude.
        /// </summary>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <param name="decimals">Number of decimals.</param>
        /// <returns>Returns random generated longitude.</returns>
        public double NextLongitude(double min = -180.0, double max = 180.0, uint decimals = 5)
            => NextDouble(min, max, decimals);

        /// <summary>
        /// Generates a random latitude.
        /// </summary>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <param name="decimals">Number of decimals.</param>
        /// <returns>Returns random generated latitude.</returns>
        public double NextLatitude(double min = -90, double max = 90, uint decimals = 5)
            => NextDouble(min, max, decimals);

        /// <summary>
        /// Generates a random geohash.
        /// </summary>
        /// <param name="length">Length of geohash.</param>
        /// <returns>Returns random generated geohash.</returns>
        public string NextGeohash(int length = 7)
            => NextString(length, "0123456789bcdefghjkmnpqrstuvwxyz", false, false, CasingRules.MixedCase);

        /// <summary>
        /// Returns a random country.
        /// </summary>
        /// <param name="fullName">True to return full name.</param>
        /// <returns>Returns a random country.</returns>
        public string NextCountry(bool fullName = true)
            => fullName ? PickRandomItem(Countries).Key : PickRandomItem(Countries).Value;

        /// <summary>
        /// Generates a random city name.
        /// </summary>
        /// <returns>Returns random generated city name.</returns>
        public string NextCity() => NextWord(syllablesCount: 3).Capitalize();

        /// <summary>
        /// Generates a random street address.
        /// </summary>
        /// <param name="syllables">Number of syllables.</param>
        /// <param name="shortSuffix">True to use short suffix.</param>
        /// <returns>Returns random generated address.</returns>
        public string NextAddress(int syllables = 2, bool shortSuffix = true)
            => $"{NextNatural(5, 2000)} {NextStreet(syllables, shortSuffix)}";

        #endregion

        #region Date\Time

        /// <summary>
        /// Generates a random year.
        /// </summary>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <returns>Returns random generated year.</returns>
        public int NextYear(int min = 2016, int max = 2116) => NextNatural(min, max);

        /// <summary>
        /// Generates a random month.
        /// </summary>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>min</c> is less than 1.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>max</c> is greater than 12.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>min</c> is greater than <c>max</c>.</exception>
        /// <returns>Returns random generated month.</returns>
        public string NextMonth(int min = 1, int max = 12)
        {
            if (min < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(min), "min cannot be less than 1.");
            }

            if (max > 12)
            {
                throw new ArgumentOutOfRangeException(nameof(max), "max cannot be greater than 12.");
            }

            if (min > max)
            {
                throw new ArgumentOutOfRangeException(nameof(min), "min cannot be greater than max.");
            }

            return PickRandomItem(Months.Skip(min - 1).Take(max - min - 1).ToList()).Item1;
        }

        /// <summary>
        /// Generates a random second.
        /// </summary>
        /// <returns>Returns random generated second.</returns>
        public int NextSecond() => NextNatural(max: 59);

        /// <summary>
        /// Generates a random minute.
        /// </summary>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>min</c> is less than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>max</c> is greater than 59.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>min</c> is greater than <c>max</c>.</exception>
        /// <returns>Returns random generated minute.</returns>
        public int NextMinute(int min = 0, int max = 59)
        {
            if (min < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(min), "min cannot be less than 0.");
            }

            if (max > 59)
            {
                throw new ArgumentOutOfRangeException(nameof(max), "max cannot be greater than 59.");
            }

            if (min > max)
            {
                throw new ArgumentOutOfRangeException(nameof(min), "min cannot be greater than max.");
            }

            return NextNatural(min, max);
        }

        /// <summary>
        /// Generates a random hour.
        /// </summary>
        /// <param name="twentyfourHours">True to use 24-hours format.</param>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>min</c> is less than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>max</c> is greater than 23.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>max</c> is greater than 12 in 12-hours format.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>min</c> is greater than <c>max</c>.</exception>
        /// <returns>Returns random generated hour.</returns>
        public int NextHour(bool twentyfourHours = true, int min = 0, int max = 23)
        {
            if (min < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(min), "min cannot be less than 0.");
            }

            if (twentyfourHours && max > 23)
            {
                throw new ArgumentOutOfRangeException(nameof(max), "max cannot be greater than 23 for twentyfourHours option.");
            }

            if (!twentyfourHours && max > 12)
            {
                throw new ArgumentOutOfRangeException(nameof(max), "max cannot be greater than 12.");
            }

            if (min > max)
            {
                throw new ArgumentOutOfRangeException(nameof(min), "min cannot be greater than max.");
            }

            return NextNatural(twentyfourHours ? 0 : 1, twentyfourHours ? 23 : 12);
        }

        /// <summary>
        /// Generates a random millisecond.
        /// </summary>
        /// <returns>Returns random generated millisecond.</returns>
        public int NextMillisecond() => NextNatural(max: 999);

        /// <summary>
        /// Generates a random date.
        /// </summary>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <returns>Returns random generated date.</returns>
        public DateTime NextDate(DateTime? min = null, DateTime? max = null)
        {
            if (min.HasValue && max.HasValue)
            {
                return Utils.UnixTimestampToDateTime(NextNatural((int) Utils.DateTimeToUnixTimestamp(min.Value),
                    (int) Utils.DateTimeToUnixTimestamp(max.Value)));
            }

            var m = NextNatural(1, 12);

            return new DateTime(NextYear(), m, NextNatural(1, Months[m].Item4), NextHour(), NextMinute(),
                NextSecond(), NextMillisecond());
        }

        #endregion

        #region Finance

        private Tuple<string, string, string, int> CcType(string name = null)
        {
            Tuple<string, string, string, int> cc;

            if (!string.IsNullOrEmpty(name))
            {
                cc = CcTypes.FirstOrDefault(tcc => tcc.Item1 == name || tcc.Item2 == name);

                if (cc == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(name),
                        "Credit card type '" + name + "'' is not supported");
                }
            }
            else
            {
                cc = PickRandomItem(CcTypes);
            }

            return cc;
        }

        /// <summary>
        /// Generate a random currency.
        /// </summary>
        /// <param name="returnCode">True to return currency code.</param>
        /// <returns>Returns random generated currency.</returns>
        public string NextCurrency(bool returnCode = false)
            => returnCode ? PickRandomItem(CurrenyTypes).Key : PickRandomItem(CurrenyTypes).Value;

        /// <summary>
        /// Generates a random credit card number. This card number will pass the Luhn algorithm so it looks like a legit card.
        /// </summary>
        /// <param name="cardType">The available types are:
        /// American Express - amex
        /// Bankcard - bankcard
        /// China UnionPay - chinaunion
        /// Diners Club Carte Blanche - dccarte
        /// Diners Club enRoute - dcenroute
        /// Diners Club International - dcintl
        /// Diners Club United States and Canada - dcusc
        /// Discover Card - discover
        /// InstaPayment - instapay
        /// JCB - jcb
        /// Laser - laser
        /// Maestro - maestro
        /// Mastercard - mc
        /// Solo - solo
        /// Switch - switch
        /// Visa - visa
        /// Visa Electron - electron
        /// </param>
        /// <returns>Returns random generated credit card number.</returns>
        public long NextCreditCardNumber(string cardType = null)
        {
            var type = CcType(cardType);
            var toGenerate = type.Item4 - type.Item3.Length - 1;
            var number = type.Item3;
            number += string.Join("", NextList<int>(new Func<int, int, int>(NextInt), toGenerate, 0, 9));
            number += Utils.LuhnCalcualte(long.Parse(number));

            return long.Parse(number);
        }

        #endregion

        #region Miscellaneous

        /// <summary>
        /// Returns a random GUID.
        /// </summary>
        /// <param name="version">Custom version.</param>
        /// <returns>Returns a random GUID.</returns>
        public string NextGUID(int version = 5)
        {
            const string guidPool = "abcdef1234567890";
            const string variantPool = "ab89";
            Func<string, int, string> strFn = (pool, len) => NextString(len, pool, false, false, CasingRules.MixedCase);

            return strFn(guidPool, 8) + "-" +
                   strFn(guidPool, 4) + "-" +
                   version +
                   strFn(variantPool, 3) + "-" +
                   strFn(guidPool, 1) + "-" +
                   strFn(guidPool, 3) + "-" +
                   strFn(guidPool, 12);
        }

        /// <summary>
        /// Returns a random hex hash.
        /// </summary>
        /// <param name="length">Custom length.</param>
        /// <param name="casing">Casing to get a hash with only uppercase letters rather than the default lowercase.</param>
        /// <returns>Returns a random hex hash.</returns>
        public string NextHash(int length = 40, CasingRules casing = CasingRules.LowerCase)
        {
            var pool = casing == CasingRules.UpperCase ? HexPool.ToUpperInvariant() : HexPool;

            return NextString(length, pool, false, false, CasingRules.MixedCase);
        }

        #endregion

        #endregion
    }
}