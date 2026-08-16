// Copyright (c) 2020-2026 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using SoftCircuits.Parsers;

namespace FixedWidthParser.Tests
{
    public class AccessorAndMappingTests
    {

        #region MapField() mutating an attribute-mapped field's length

        public class LineItem
        {
            [FixedWidthField(5)]
            public string Code { get; set; } = string.Empty;
            [FixedWidthField(5)]
            public string Description { get; set; } = string.Empty;
        }

        private readonly List<LineItem> LineItems =
        [
            new LineItem { Code = "A1", Description = "Foo" },
            new LineItem { Code = "B2", Description = "A Much Longer Value" },
        ];

        /// <summary>
        /// MapField() is called once, before any rows are written, to widen a field beyond
        /// its attribute-specified length. Every row - not just the first - must reflect the
        /// new length, since the per-row line length is recalculated fresh on every Write()
        /// call rather than cached at construction time.
        /// </summary>
        [Fact]
        public void MapField_OverridingAttributeLength_AppliesToEveryRow()
        {
            MemoryFile memFile = new();

            using (FixedWidthWriter<LineItem> writer = new(memFile.GetStream()))
            {
                writer.MapField(x => x.Description, 20);
                writer.Write(LineItems);
            }

            List<LineItem> results;
            using (FixedWidthReader<LineItem> reader = new(memFile.GetStream()))
            {
                reader.MapField(x => x.Description, 20);
                results = [.. reader.ReadAll()];
            }

            Assert.Equal(LineItems.Count, results.Count);
            for (int i = 0; i < results.Count; i++)
            {
                Assert.Equal(LineItems[i].Code, results[i].Code);
                Assert.Equal(LineItems[i].Description, results[i].Description);
            }
        }

        #endregion

        #region Fully programmatic mapping

        public class RawRecord
        {
            public string Sku { get; set; } = string.Empty;
            public int Quantity { get; set; }
        }

        /// <summary>
        /// A type with no FixedWidthFieldAttribute members at all - every field is mapped
        /// programmatically via MapField(), exercising the fluent SetAlignment/SetPadCharacter/
        /// SetSkip options and confirming Skip is correctly included in the line length used
        /// to size the write buffer.
        /// </summary>
        [Fact]
        public void ProgrammaticMapping_RoundTrips_WithFluentOptionsAndSkip()
        {
            List<RawRecord> items =
            [
                new RawRecord { Sku = "SKU1", Quantity = 7 },
                new RawRecord { Sku = "SKU42", Quantity = 123 },
            ];

            MemoryFile memFile = new();

            using (FixedWidthWriter<RawRecord> writer = new(memFile.GetStream()))
            {
                writer.MapField(x => x.Sku, 10)
                    .SetAlignment(FieldAlignment.Left)
                    .SetPadCharacter('.');
                writer.MapField(x => x.Quantity, 6)
                    .SetSkip(2)
                    .SetAlignment(FieldAlignment.Right)
                    .SetPadCharacter('0');
                writer.Write(items);
            }

            List<RawRecord> results;
            using (FixedWidthReader<RawRecord> reader = new(memFile.GetStream()))
            {
                reader.MapField(x => x.Sku, 10).SetPadCharacter('.');
                reader.MapField(x => x.Quantity, 6).SetSkip(2);
                results = [.. reader.ReadAll()];
            }

            Assert.Equal(items.Count, results.Count);
            for (int i = 0; i < results.Count; i++)
            {
                Assert.Equal(items[i].Sku, results[i].Sku);
                Assert.Equal(items[i].Quantity, results[i].Quantity);
            }
        }

        #endregion

        #region Readonly field semantics

        public class ImmutableRow
        {
            [FixedWidthField(10)]
            public readonly string Code;
            [FixedWidthField(10)]
            public string Description { get; set; } = string.Empty;

            public ImmutableRow() => Code = "DEFAULT";

            public ImmutableRow(string code, string description)
            {
                Code = code;
                Description = description;
            }
        }

        /// <summary>
        /// Documents the intended behavior for readonly fields: they are still readable, so a
        /// readonly field's current value IS written to the file. But since a readonly field
        /// cannot be assigned outside a constructor, it is correctly reported as not writable
        /// and is therefore left at its default (constructor-assigned) value when reading a
        /// file back in - it is never populated from file content. This is a deliberate
        /// behavior change: previously FieldMember.CanWrite was hardcoded true for all fields,
        /// so raw reflection would silently write to readonly fields too.
        /// </summary>
        [Fact]
        public void ReadonlyField_IsWrittenToFile_ButNotPopulatedWhenReading()
        {
            ImmutableRow original = new("ABC123", "Hello");
            MemoryFile memFile = new();

            using (FixedWidthWriter<ImmutableRow> writer = new(memFile.GetStream()))
                writer.Write(original);

            // Confirm the readonly field's value really was serialized to the file.
            Assert.Contains("ABC123", memFile.GetContent());

            ImmutableRow result;
            using (FixedWidthReader<ImmutableRow> reader = new(memFile.GetStream()))
            {
                Assert.True(reader.Read());
                Assert.NotNull(reader.Item);
                result = reader.Item;
            }

            // Description is an ordinary read-write property - it round-trips normally.
            Assert.Equal("Hello", result.Description);

            // Code is readonly - it must retain its parameterless-constructor value, NOT
            // the value read from the file.
            Assert.Equal("DEFAULT", result.Code);
        }

        #endregion

        #region Non-public member mapping

        public class SecureRecord
        {
            [FixedWidthField(8)]
            private string Token { get; set; } = string.Empty;
            [FixedWidthField(4)]
            private int Version;

            public string GetToken() => Token;
            public int GetVersion() => Version;
            public void SetTestValues(string token, int version)
            {
                Token = token;
                Version = version;
            }
        }

        /// <summary>
        /// A private property and a private field, both attribute-mapped. This specifically
        /// verifies that the compiled Expression-tree getters/setters (which replaced direct
        /// PropertyInfo/FieldInfo reflection calls) still work correctly for non-public
        /// members - the scenario most likely to silently regress in that kind of change.
        /// </summary>
        [Fact]
        public void PrivateMembers_AreReadAndWrittenCorrectly()
        {
            SecureRecord original = new();
            original.SetTestValues("TOKEN123", 42);

            MemoryFile memFile = new();

            using (FixedWidthWriter<SecureRecord> writer = new(memFile.GetStream()))
                writer.Write(original);

            SecureRecord result;
            using (FixedWidthReader<SecureRecord> reader = new(memFile.GetStream()))
            {
                Assert.True(reader.Read());
                Assert.NotNull(reader.Item);
                result = reader.Item;
            }

            Assert.Equal("TOKEN123", result.GetToken());
            Assert.Equal(42, result.GetVersion());
        }

        #endregion

    }
}
