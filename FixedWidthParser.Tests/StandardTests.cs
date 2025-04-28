// Copyright (c) 2020-2025 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using NUnit.Framework;
using NUnit.Framework.Legacy;
using SoftCircuits.Parsers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FixedWidthParserTests
{
    public class StandardTests
    {
        [Test]
        public async Task BasicTestAsync()
        {
            FixedWidthField[] fields =
            [
                new(26),
                new(26),
                new(26),
                new(26),
            ];

            List<string[]> values =
            [
                ["abcdefghijklmnopqrstuvwxyz", "1234567890", "1010101010", ""],
                ["1234567890", "1010101010", "", "abcdefghijklmnopqrstuvwxyz"],
                ["1010101010", "", "abcdefghijklmnopqrstuvwxyz", "1234567890"],
                ["", "abcdefghijklmnopqrstuvwxyz", "1234567890", "1010101010"],
            ];

            string path = Path.GetTempFileName();

            // Build expected content
            StringBuilder builder = new();
            foreach (var value in values)
            {
                Debug.Assert(value.Length == fields.Length);
                for (int i = 0; i < fields.Length; i++)
                {
                    builder.Append(value[i]);
                    builder.Append(new string(' ', fields[i].Length - value[i].Length));
                }
                builder.AppendLine();
            }
            string expectedContent = builder.ToString();

            try
            {
                // Sync
                using (FixedWidthWriter writer = new(fields, path))
                {
                    for (int i = 0; i < values.Count; i++)
                        writer.Write(values[i]);
                }

                Assert.That(expectedContent == File.ReadAllText(path));

                using (FixedWidthReader reader = new(fields, path))
                {
                    int i = 0;
                    while (reader.Read())
                        CollectionAssert.AreEquivalent(values[i++], reader.Values);
                }

                // Async
                using (FixedWidthWriter writer = new(fields, path))
                {
                    for (int i = 0; i < values.Count; i++)
                        await writer.WriteAsync(values[i]);
                }

                Assert.That(expectedContent == File.ReadAllText(path));

                using (FixedWidthReader reader = new(fields, path))
                {
                    int i = 0;
                    while (await reader.ReadAsync())
                        CollectionAssert.AreEquivalent(values[i++], reader.Values!);
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

        [Test]
        public void FieldLayoutTest()
        {
            FixedWidthField[] fields =
            [
                new(26),
                new(26),
                new(26),
                new(26),
            ];

            List<string[]> values =
            [
                ["abcdefghijklmnopqrstuvwxyz", "1234567890", "1010101010", ""],
                ["1234567890", "1010101010", "", "abcdefghijklmnopqrstuvwxyz"],
                ["1010101010", "", "abcdefghijklmnopqrstuvwxyz", "1234567890"],
                ["", "abcdefghijklmnopqrstuvwxyz", "1234567890", "1010101010"],
            ];

            List<string[]> leftPadded =
            [
                ["abcdefghijklmnopqrstuvwxyz", "1234567890                ", "1010101010                ", "                          "],
                ["1234567890                ", "1010101010                ", "                          ", "abcdefghijklmnopqrstuvwxyz"],
                ["1010101010                ", "                          ", "abcdefghijklmnopqrstuvwxyz", "1234567890                "],
                ["                          ", "abcdefghijklmnopqrstuvwxyz", "1234567890                ", "1010101010                "],
            ];

            List<string[]> rightPadded =
            [
                ["abcdefghijklmnopqrstuvwxyz", "                1234567890", "                1010101010", "                          "],
                ["                1234567890", "                1010101010", "                          ", "abcdefghijklmnopqrstuvwxyz"],
                ["                1010101010", "                          ", "abcdefghijklmnopqrstuvwxyz", "                1234567890"],
                ["                          ", "abcdefghijklmnopqrstuvwxyz", "                1234567890", "                1010101010"],
            ];

            List<string[]> tildePadded =
            [
                ["abcdefghijklmnopqrstuvwxyz", "1234567890~~~~~~~~~~~~~~~~", "1010101010~~~~~~~~~~~~~~~~", "~~~~~~~~~~~~~~~~~~~~~~~~~~"],
                ["1234567890~~~~~~~~~~~~~~~~", "1010101010~~~~~~~~~~~~~~~~", "~~~~~~~~~~~~~~~~~~~~~~~~~~", "abcdefghijklmnopqrstuvwxyz"],
                ["1010101010~~~~~~~~~~~~~~~~", "~~~~~~~~~~~~~~~~~~~~~~~~~~", "abcdefghijklmnopqrstuvwxyz", "1234567890~~~~~~~~~~~~~~~~"],
                ["~~~~~~~~~~~~~~~~~~~~~~~~~~", "abcdefghijklmnopqrstuvwxyz", "1234567890~~~~~~~~~~~~~~~~", "1010101010~~~~~~~~~~~~~~~~"],
            ];

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
            FixedWidthField[] fields =
            [
                new(10),
                new(10),
                new(10),
                new(10),
            ];

            List<string[]> values =
            [
                ["abc"],
                ["abc", "def"],
                ["abc", "def", "ghi"],
                ["abc", "def", "ghi", "jkl"],
                ["abc", "def", "ghi", "jkl", "mno"],
                ["abc", "def", "ghi", "jkl", "mno", "pqr"],
            ];

            List<string[]> expected =
            [
                ["abc", string.Empty, string.Empty, string.Empty],
                ["abc", "def", string.Empty, string.Empty],
                ["abc", "def", "ghi", string.Empty],
                ["abc", "def", "ghi", "jkl"],
                ["abc", "def", "ghi", "jkl"],
                ["abc", "def", "ghi", "jkl"],
            ];

            WriteReadValues(fields, values, out List<string[]> results);
            CollectionAssert.AreEqual(expected, results);
        }

        [Test]
        public void SkipTests()
        {
            FixedWidthField[] fields =
            [
                new(5),
                new(5) { Skip = 4 },
                new(5),
            ];

            FixedWidthField[] fields2 =
            [
                new(5),
                new(4),
                new(5),
                new(5),
            ];

            List<string[]> values =
            [
                ["abc", "def", "ghi"],
                ["def", "ghi", "jkl"],
                ["ghi", "jkl", "mno"],
            ];

            List<string[]> expected =
            [
                ["abc  ", "def  ", "ghi  "],
                ["def  ", "ghi  ", "jkl  "],
                ["ghi  ", "jkl  ", "mno  "],
            ];

            List<string[]> expected2 =
            [
                ["abc  ", "    ", "def  ", "ghi  "],
                ["def  ", "    ", "ghi  ", "jkl  "],
                ["ghi  ", "    ", "jkl  ", "mno  "],
            ];

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
            FixedWidthField[] fields =
            [
                new(10, FieldAlignment.Left, ' ', false),
                new(10, FieldAlignment.Right, '~', false),
                new(10, alignment: FieldAlignment.Left, padCharacter: '@', trimField: false),
                new(10, alignment: FieldAlignment.Right, padCharacter: '!', trimField: false),
                new(10)
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
            ];

            List<string[]> values =
            [
                ["abc", "def", "ghi", "jkl", "mno", "pqr"],
                ["def", "ghi", "jkl", "mno", "pqr", "stu"],
                ["ghi", "jkl", "mno", "pqr", "stu", "vwx"],
            ];

            List<string[]> expected =
            [
                ["abc       ", "~~~~~~~def", "ghi@@@@@@@", "!!!!!!!jkl", "mno", "pqr"],
                ["def       ", "~~~~~~~ghi", "jkl@@@@@@@", "!!!!!!!mno", "pqr", "stu"],
                ["ghi       ", "~~~~~~~jkl", "mno@@@@@@@", "!!!!!!!pqr", "stu", "vwx"],
            ];

            WriteReadValues(fields, values, out List<string[]> results);
            CollectionAssert.AreEqual(expected, results);
        }

        [Test]
        public void OverflowTests()
        {
            FixedWidthField[] fields =
            [
                new FixedWidthField(2),
                new FixedWidthField(2),
            ];

            List<string[]> values =
            [
                ["abc", "def"],
                ["ghi", "jkl"],
            ];

            List<string[]> expected =
            [
                ["ab", "de"],
                ["gh", "jk"],
            ];

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
            FixedWidthField[] writeFields =
            [
                new(4),
                new(4),
            ];

            FixedWidthField[] readFields =
            [
                new(4),
                new(4),
                new(4),
            ];

            List<string[]> values =
            [
                ["abc", "def"],
                ["ghi", "jkl"],
            ];

            List<string[]> expected =
            [
                ["abc", "def", string.Empty],
                ["ghi", "jkl", string.Empty],
            ];

            Assert.Throws<FixedWidthOutOfRangeException>(() =>
            {
                WriteReadValues(writeFields, readFields, values, out List<string[]> results, new FixedWidthOptions { ThrowOutOfRangeException = true });
            });

            WriteReadValues(writeFields, readFields, values, out List<string[]> results, new FixedWidthOptions { ThrowOutOfRangeException = false });
            CollectionAssert.AreEqual(expected, results);
        }

        [Test]
        public void IgnoredLinesTests()
        {
            FixedWidthField[] fields =
            [
                new(4),
                new(4),
            ];

            List<string> lines =
            [
                "abcd1234",
                "abcd",
                "1234",
                "",
                "jklm5678",
            ];

            List<string[]> expected =
            [
                ["abcd", "1234"],
                ["jklm", "5678"],
            ];

            Assert.Throws<FixedWidthOutOfRangeException>(() =>
            {
                WriteReadValues(lines, fields, out List<string[]> results, new FixedWidthOptions { IsIgnoredLine = null });
            });

            WriteReadValues(lines, fields, out List<string[]> results, new FixedWidthOptions { IsIgnoredLine = s => s.Length < 8 });
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
            MemoryFile memFile = new();

            using (FixedWidthWriter writer = new(writeFields, memFile.GetStream(), options))
            {
                foreach (var item in items)
                    writer.Write(item);
            }

            results = [];
            using FixedWidthReader reader = new(readFields, memFile.GetStream(), options);
            {
                while (reader.Read())
                {
                    // Need to copy values so next read doesn't overwrite them
                    string[] copy = new string[reader.Values.Length];
                    reader.Values.CopyTo(copy, 0);
                    results.Add(copy);
                }
            }
        }

        /// <summary>
        /// Writes free-form text lines and reads back the results as fixed-width fields.
        /// </summary>
        private static void WriteReadValues(IEnumerable<string> writeLines, IEnumerable<FixedWidthField> readFields, out List<string[]> results, FixedWidthOptions? options = null)
        {
            MemoryFile memFile = new();

            using (StreamWriter writer = new(memFile.GetStream()))
            {
                foreach (var line in writeLines)
                    writer.WriteLine(line);
            }

            results = [];
            using FixedWidthReader reader = new(readFields, memFile.GetStream(), options);
            {
                while (reader.Read())
                {
                    // Need to copy values so next read doesn't overwrite them
                    string[] copy = new string[reader.Values.Length];
                    reader.Values.CopyTo(copy, 0);
                    results.Add(copy);
                }
            }
        }

        #endregion

    }
}
