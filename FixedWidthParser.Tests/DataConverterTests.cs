// Copyright (c) 2020-2026 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using SoftCircuits.Parsers;

namespace FixedWidthParser.Tests
{
    public class DataConverterTests
    {
        public class AllTypesClass
        {
            [FixedWidthField(10)]
            public Boolean BooleanValue { get; set; }
            [FixedWidthField(10)]
            public Byte ByteValue { get; set; }
            [FixedWidthField(10)]
            public Char CharValue { get; set; }
            [FixedWidthField(26)]
            public DateOnly DateOnlyValue { get; set; }
            [FixedWidthField(26)]
            public DateTime DateTimeValue { get; set; }
            [FixedWidthField(26)]
            public DateTimeOffset DateTimeOffsetValue { get; set; }
            [FixedWidthField(10)]
            public Decimal DecimalValue { get; set; }
            [FixedWidthField(10)]
            public Double DoubleValue { get; set; }
            [FixedWidthField(36)]
            public Guid GuidValue { get; set; }
            [FixedWidthField(10)]
            public Int16 Int16Value { get; set; }
            [FixedWidthField(10)]
            public Int32 Int32Value { get; set; }
            [FixedWidthField(10)]
            public Int64 Int64Value { get; set; }
            [FixedWidthField(10)]
            public SByte SByteValue { get; set; }
            [FixedWidthField(10)]
            public Single SingleValue { get; set; }
            [FixedWidthField(10)]
            public String StringValue { get; set; }
            [FixedWidthField(26)]
            public TimeOnly TimeOnlyValue { get; set; }
            [FixedWidthField(10)]
            public UInt16 UInt16Value { get; set; }
            [FixedWidthField(10)]
            public UInt32 UInt32Value { get; set; }
            [FixedWidthField(10)]
            public UInt64 UInt64Value { get; set; }

            public AllTypesClass()
            {
                StringValue = string.Empty;
            }
        }

        public class AllTypesComparer : IEqualityComparer<AllTypesClass>
        {
            public bool Equals(AllTypesClass? x, AllTypesClass? y)
            {
                if (x is null && y is null)
                    return true;
                if (x is null || y is null)
                    return false;
                if (x.BooleanValue != y.BooleanValue)
                    return false;
                if (x.ByteValue != y.ByteValue)
                    return false;
                if (x.CharValue != y.CharValue)
                    return false;
                if (x.DateOnlyValue != y.DateOnlyValue)
                    return false;
                x.DateTimeValue = TruncateMilliseconds(x.DateTimeValue);
                y.DateTimeValue = TruncateMilliseconds(y.DateTimeValue);
                if (x.DateTimeValue != y.DateTimeValue)
                    return false;
                x.DateTimeOffsetValue = TruncateMilliseconds(x.DateTimeOffsetValue);
                y.DateTimeOffsetValue = TruncateMilliseconds(y.DateTimeOffsetValue);
                if (x.DateTimeOffsetValue != y.DateTimeOffsetValue)
                    return false;
                if (y.DecimalValue != y.DecimalValue)
                    return false;
                if (x.DoubleValue != y.DoubleValue)
                    return false;
                if (x.GuidValue != y.GuidValue)
                    return false;
                if (x.Int16Value != y.Int16Value)
                    return false;
                if (x.Int32Value != y.Int32Value)
                    return false;
                if (x.Int64Value != y.Int64Value)
                    return false;
                if (x.SByteValue != y.SByteValue)
                    return false;
                if (x.SingleValue != y.SingleValue)
                    return false;
                if (x.StringValue != y.StringValue)
                    return false;
                x.TimeOnlyValue = TruncateMilliseconds(x.TimeOnlyValue);
                y.TimeOnlyValue = TruncateMilliseconds(y.TimeOnlyValue);
                if (x.TimeOnlyValue != y.TimeOnlyValue)
                    return false;
                if (x.UInt16Value != y.UInt16Value)
                    return false;
                if (x.UInt32Value != y.UInt32Value)
                    return false;
                if (x.UInt64Value != y.UInt64Value)
                    return false;

                return true;
            }

            public int GetHashCode(AllTypesClass obj)
            {
                var hash = new HashCode();
                hash.Add(obj.BooleanValue);
                hash.Add(obj.ByteValue);
                hash.Add(obj.CharValue);
                hash.Add(obj.DateOnlyValue);
                hash.Add(obj.DateTimeValue);
                hash.Add(obj.DateTimeOffsetValue);
                hash.Add(obj.DecimalValue);
                hash.Add(obj.DoubleValue);
                hash.Add(obj.GuidValue);
                hash.Add(obj.Int16Value);
                hash.Add(obj.Int32Value);
                hash.Add(obj.Int64Value);
                hash.Add(obj.SByteValue);
                hash.Add(obj.SingleValue);
                hash.Add(obj.StringValue);
                hash.Add(obj.TimeOnlyValue);
                hash.Add(obj.UInt16Value);
                hash.Add(obj.UInt32Value);
                hash.Add(obj.UInt64Value);
                return hash.ToHashCode();
            }

            private static DateTime TruncateMilliseconds(DateTime dt) => dt.AddTicks(-(dt.Ticks % TimeSpan.TicksPerSecond));
            private static DateTimeOffset TruncateMilliseconds(DateTimeOffset dt) => dt.AddTicks(-(dt.Ticks % TimeSpan.TicksPerSecond));
            private static TimeOnly TruncateMilliseconds(TimeOnly t) => new(t.Ticks - t.Ticks % TimeSpan.TicksPerSecond);
        }

        private readonly List<AllTypesClass> AllTypesItems =
        [
            new AllTypesClass
            {
                BooleanValue = true,
                ByteValue = 47,
                CharValue = 'r',
                DateOnlyValue = DateOnly.MinValue,
                DateTimeValue = DateTime.MinValue,
                DateTimeOffsetValue = DateTimeOffset.MinValue,
                DecimalValue = 123.456m,
                DoubleValue = 47.9,
                GuidValue = Guid.NewGuid(),
                Int16Value = 4887,
                Int32Value = -98072,
                Int64Value = 489938827,
                SByteValue = -87,
                SingleValue = 432.99f,
                StringValue = "abcdef",
                TimeOnlyValue = TimeOnly.MinValue,
                UInt16Value = 8402,
                UInt32Value = 4662900,
                UInt64Value = 650094891,
            },
            new AllTypesClass
            {
                BooleanValue = false,
                ByteValue = 107,
                CharValue = 'v',
                DateOnlyValue = DateOnly.MaxValue,
                DateTimeValue = DateTime.MaxValue,
                DateTimeOffsetValue = DateTimeOffset.MaxValue,
                DecimalValue = 988.22m,
                DoubleValue = 90.44,
                GuidValue = Guid.NewGuid(),
                Int16Value = -987,
                Int32Value = 98072,
                Int64Value = -489938827,
                SByteValue = 87,
                SingleValue = 456.1f,
                StringValue = "xyz",
                TimeOnlyValue = TimeOnly.MaxValue,
                UInt16Value = 44987,
                UInt32Value = 472209,
                UInt64Value = 7760982,
            },
            new AllTypesClass
            {
                BooleanValue = true,
                ByteValue = 98,
                CharValue = '4',
                DateOnlyValue = DateOnly.FromDateTime(DateTime.Now),
                DateTimeValue = DateTime.Now,
                DateTimeOffsetValue = new(DateTime.Now),
                DecimalValue = 780.2m,
                DoubleValue = 86.9,
                GuidValue = Guid.NewGuid(),
                Int16Value = -4721,
                Int32Value = 18692,
                Int64Value = 84452091,
                SByteValue = 30,
                SingleValue = -98.4f,
                StringValue = "",
                TimeOnlyValue = TimeOnly.FromDateTime(DateTime.Now),
                UInt16Value = 44079,
                UInt32Value = 440796,
                UInt64Value = 4407960,
            },
            new AllTypesClass
            {
                BooleanValue = false,
                ByteValue = 142,
                CharValue = '&',
                DateOnlyValue = new(2021, 12, 25),
                DateTimeValue = new(2021, 12, 25, 8, 29, 12),
                DateTimeOffsetValue = new(2021, 12, 25, 8, 29, 12, TimeSpan.Zero),
                DecimalValue = 9088261.4m,
                DoubleValue = 478.32,
                GuidValue = Guid.NewGuid(),
                Int16Value = -1880,
                Int32Value = 45661,
                Int64Value = -43811297,
                SByteValue = -7,
                SingleValue = 28.28f,
                StringValue = "border",
                TimeOnlyValue= new TimeOnly(8, 29, 12),
                UInt16Value = 42660,
                UInt32Value = 4266079,
                UInt64Value = 426607980,
            },
        ];

        [Fact]
        public void TestIntrinsicDataConverters()
        {
            ObjectMappingTests x = new();
            Assert.Equal(AllTypesItems, ObjectMappingTests.WriteReadValues(AllTypesItems), new AllTypesComparer());
        }
    }
}
