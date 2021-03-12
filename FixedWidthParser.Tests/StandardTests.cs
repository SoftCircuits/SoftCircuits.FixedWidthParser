// Copyright (c) 2020-2021 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using NUnit.Framework;
using SoftCircuits.Parsers;
using System;
using System.Collections.Generic;
using System.IO;

namespace FixedWidthParserTests
{
    public class StandardTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void BasicTest()
        {
            FixedWidthField[] fields = new FixedWidthField[]
            {
                new FixedWidthField(26),
                new FixedWidthField(26),
                new FixedWidthField(26),
                new FixedWidthField(26),
            };

            List<string[]> values = new List<string[]>
            {
                new string[] { "abcdefghijklmnopqrstuvwxyz", "1234567890", "1010101010", "" },
                new string[] { "1234567890", "1010101010", "", "abcdefghijklmnopqrstuvwxyz" },
                new string[] { "1010101010", "", "abcdefghijklmnopqrstuvwxyz", "1234567890" },
                new string[] { "", "abcdefghijklmnopqrstuvwxyz", "1234567890", "1010101010" },
            };

            List<string[]> leftPadded = new List<string[]>
            {
                new string[] { "abcdefghijklmnopqrstuvwxyz", "1234567890                ", "1010101010                ", "                          " },
                new string[] { "1234567890                ", "1010101010                ", "                          ", "abcdefghijklmnopqrstuvwxyz" },
                new string[] { "1010101010                ", "                          ", "abcdefghijklmnopqrstuvwxyz", "1234567890                " },
                new string[] { "                          ", "abcdefghijklmnopqrstuvwxyz", "1234567890                ", "1010101010                " },
            };

            List<string[]> rightPadded = new List<string[]>
            {
                new string[] { "abcdefghijklmnopqrstuvwxyz", "                1234567890", "                1010101010", "                          " },
                new string[] { "                1234567890", "                1010101010", "                          ", "abcdefghijklmnopqrstuvwxyz" },
                new string[] { "                1010101010", "                          ", "abcdefghijklmnopqrstuvwxyz", "                1234567890" },
                new string[] { "                          ", "abcdefghijklmnopqrstuvwxyz", "                1234567890", "                1010101010" },
            };

            List<string[]> tildePadded = new List<string[]>
            {
                new string[] { "abcdefghijklmnopqrstuvwxyz", "1234567890~~~~~~~~~~~~~~~~", "1010101010~~~~~~~~~~~~~~~~", "~~~~~~~~~~~~~~~~~~~~~~~~~~" },
                new string[] { "1234567890~~~~~~~~~~~~~~~~", "1010101010~~~~~~~~~~~~~~~~", "~~~~~~~~~~~~~~~~~~~~~~~~~~", "abcdefghijklmnopqrstuvwxyz" },
                new string[] { "1010101010~~~~~~~~~~~~~~~~", "~~~~~~~~~~~~~~~~~~~~~~~~~~", "abcdefghijklmnopqrstuvwxyz", "1234567890~~~~~~~~~~~~~~~~" },
                new string[] { "~~~~~~~~~~~~~~~~~~~~~~~~~~", "abcdefghijklmnopqrstuvwxyz", "1234567890~~~~~~~~~~~~~~~~", "1010101010~~~~~~~~~~~~~~~~" },
            };

            WriteReadValues(fields, values, out List<string[]> results, new FixedWidthOptions { TrimFields = true });
            CollectionAssert.AreEqual(values, results);

            WriteReadValues(fields, values, out results, new FixedWidthOptions { DefaultPadCharacter = '~', TrimFields = true });
            CollectionAssert.AreEqual(values, results);

            WriteReadValues(fields, values, out results, new FixedWidthOptions { DefaultAlignment = FieldAlignment.Left, TrimFields = false });
            CollectionAssert.AreEqual(leftPadded, results);

            WriteReadValues(fields, values, out results, new FixedWidthOptions { DefaultAlignment = FieldAlignment.Right, TrimFields = false });
            CollectionAssert.AreEqual(rightPadded, results);

            WriteReadValues(fields, values, out results, new FixedWidthOptions { DefaultPadCharacter = '~', TrimFields = false });
            CollectionAssert.AreEqual(tildePadded, results);
        }

        [Test]
        public void FieldMismatchTest()
        {
            FixedWidthField[] fields = new FixedWidthField[]
            {
                new FixedWidthField(10),
                new FixedWidthField(10),
                new FixedWidthField(10),
                new FixedWidthField(10),
            };

            List<string[]> values = new()
            {
                new string[] { "abc" },
                new string[] { "abc", "def" },
                new string[] { "abc", "def", "ghi" },
                new string[] { "abc", "def", "ghi", "jkl" },
                new string[] { "abc", "def", "ghi", "jkl", "mno" },
                new string[] { "abc", "def", "ghi", "jkl", "mno", "pqr" },
            };

            List<string[]> expected = new()
            {
                new string[] { "abc", string.Empty, string.Empty, string.Empty },
                new string[] { "abc", "def", string.Empty, string.Empty },
                new string[] { "abc", "def", "ghi", string.Empty },
                new string[] { "abc", "def", "ghi", "jkl" },
                new string[] { "abc", "def", "ghi", "jkl" },
                new string[] { "abc", "def", "ghi", "jkl" },
            };

            WriteReadValues(fields, values, out List<string[]> results);
            CollectionAssert.AreEqual(expected, results);
        }

        [Test]
        public void SkipTests()
        {
            FixedWidthField[] fields = new FixedWidthField[]
            {
                new FixedWidthField(5),
                new FixedWidthField(5) { Skip = 4 },
                new FixedWidthField(5),
            };

            FixedWidthField[] fields2 = new FixedWidthField[]
            {
                new FixedWidthField(5),
                new FixedWidthField(4),
                new FixedWidthField(5),
                new FixedWidthField(5),
            };

            List<string[]> values = new()
            {
                new string[] { "abc", "def", "ghi" },
                new string[] { "def", "ghi", "jkl" },
                new string[] { "ghi", "jkl", "mno" },
            };

            List<string[]> expected = new()
            {
                new string[] { "abc  ", "def  ", "ghi  " },
                new string[] { "def  ", "ghi  ", "jkl  " },
                new string[] { "ghi  ", "jkl  ", "mno  " },
            };

            List<string[]> expected2 = new()
            {
                new string[] { "abc  ", "    ", "def  ", "ghi  " },
                new string[] { "def  ", "    ", "ghi  ", "jkl  " },
                new string[] { "ghi  ", "    ", "jkl  ", "mno  " },
            };

            WriteReadValues(fields, values, out List<string[]> results);
            CollectionAssert.AreEqual(values, results);

            WriteReadValues(fields, values, out results, new FixedWidthOptions { TrimFields = false });
            CollectionAssert.AreEqual(expected, results);

            WriteReadValues(fields, fields2, values, out results, new FixedWidthOptions { TrimFields = false });
            CollectionAssert.AreEqual(expected2, results);
        }

        [Test]
        public void FieldOverrideTests()
        {
            FixedWidthField[] fields = new FixedWidthField[]
            {
                new FixedWidthField(10)
                {
                    Alignment = FieldAlignment.Left,
                    PadCharacter = ' ',
                    TrimField = false,
                },
                new FixedWidthField(10)
                {
                    Alignment = FieldAlignment.Right,
                    PadCharacter = '~',
                    TrimField = false,
                },
                new FixedWidthField(10)
                {
                    Alignment = FieldAlignment.Left,
                    PadCharacter = '@',
                    TrimField = false,
                },
                new FixedWidthField(10)
                {
                    Alignment = FieldAlignment.Right,
                    PadCharacter = '!',
                    TrimField = false,
                },
                new FixedWidthField(10)
                {
                    Alignment = FieldAlignment.Left,
                    PadCharacter = '=',
                    TrimField = true,
                },
                new FixedWidthField(10)
                {
                    Alignment = FieldAlignment.Right,
                    PadCharacter = '=',
                    TrimField = true,
                },
            };

            List<string[]> values = new List<string[]>
            {
                new string[] { "abc", "def", "ghi", "jkl", "mno", "pqr" },
                new string[] { "def", "ghi", "jkl", "mno", "pqr", "stu" },
                new string[] { "ghi", "jkl", "mno", "pqr", "stu", "vwx" },
            };

            List<string[]> expected = new List<string[]>
            {
                new string[] { "abc       ", "~~~~~~~def", "ghi@@@@@@@", "!!!!!!!jkl", "mno", "pqr" },
                new string[] { "def       ", "~~~~~~~ghi", "jkl@@@@@@@", "!!!!!!!mno", "pqr", "stu" },
                new string[] { "ghi       ", "~~~~~~~jkl", "mno@@@@@@@", "!!!!!!!pqr", "stu", "vwx" },
            };

            WriteReadValues(fields, values, out List<string[]> results);
            CollectionAssert.AreEqual(expected, results);
        }

        [Test]
        public void OverflowTests()
        {
            FixedWidthField[] fields = new FixedWidthField[]
            {
                new FixedWidthField(2),
                new FixedWidthField(2),
            };

            List<string[]> values = new()
            {
                new string[] { "abc", "def" },
                new string[] { "ghi", "jkl" },
            };

            List<string[]> expected = new()
            {
                new string[] { "ab", "de" },
                new string[] { "gh", "jk" },
            };

            Assert.Throws<FixedWidthOverflowException>(() =>
            {
                WriteReadValues(fields, values, out List<string[]> results, new FixedWidthOptions { ThrowOverflowException = true });
            });

            WriteReadValues(fields, values, out List<string[]> results, new FixedWidthOptions { ThrowOverflowException = false });
            CollectionAssert.AreEqual(expected, results);
        }

        [Test]
        public void OutOfRangeTests()
        {
            FixedWidthField[] writeFields = new FixedWidthField[]
            {
                new FixedWidthField(4),
                new FixedWidthField(4),
            };

            FixedWidthField[] readFields = new FixedWidthField[]
            {
                new FixedWidthField(4),
                new FixedWidthField(4),
                new FixedWidthField(4),
            };

            List<string[]> values = new()
            {
                new string[] { "abc", "def" },
                new string[] { "ghi", "jkl" },
            };

            List<string[]> expected = new()
            {
                new string[] { "abc", "def", string.Empty },
                new string[] { "ghi", "jkl", string.Empty },
            };

            Assert.Throws<FixedWidthOutOfRangeException>(() =>
            {
                WriteReadValues(writeFields, readFields, values, out List<string[]> results, new FixedWidthOptions { ThrowOutOfRangeException = true });
            });

            WriteReadValues(writeFields, readFields, values, out List<string[]> results, new FixedWidthOptions { ThrowOutOfRangeException = false });
            CollectionAssert.AreEqual(expected, results);
        }

        #region Support methods

        /// <summary>
        /// Writes the given values to a fixed-width file and reads back the results.
        /// </summary>
        private static void WriteReadValues(IEnumerable<FixedWidthField> fields, List<string[]> items, out List<string[]> results, FixedWidthOptions? options = null) =>
            WriteReadValues(fields, fields, items, out results, options);

        /// <summary>
        /// Writes the given values to a fixed-width file and reads back the results.
        /// </summary>
        private static void WriteReadValues(IEnumerable<FixedWidthField> writeFields, IEnumerable<FixedWidthField> readFields, List<string[]> items, out List<string[]> results, FixedWidthOptions? options = null)
        {
            string path = Path.GetTempFileName();

            try
            {
                using (FixedWidthWriter writer = new FixedWidthWriter(writeFields, path, options))
                {
                    foreach (var item in items)
                        writer.Write(item);
                }

                results = new List<string[]>();

                using FixedWidthReader reader = new FixedWidthReader(readFields, path, options);

                string[]? values = null;
                while (reader.Read(ref values))
                {
                    // Need to copy values so next read doesn't overwrite them
                    string[] copy = new string[values.Length];
                    values.CopyTo(copy, 0);
                    results.Add(copy);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                File.Delete(path);
            }
        }

        #endregion

    }
}
