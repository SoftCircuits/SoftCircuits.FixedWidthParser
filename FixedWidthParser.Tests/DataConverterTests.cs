// Copyright (c) 2020-2026 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using NUnit.Framework;
using NUnit.Framework.Legacy;
using SoftCircuits.Parsers;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FixedWidthParserTests
{
    public class DataConverterTests
    {
        private class AllTypesClass
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

        private class AllTypesComparer : IComparer, IComparer<AllTypesClass>
        {
            public int Compare(object? a, object? b)
            {
                if (a is not AllTypesClass ta || b is not AllTypesClass tb)
                    throw new InvalidOperationException();
                return Compare(ta, tb);
            }

            public int Compare(AllTypesClass? a, AllTypesClass? b)
            {
                if (a != null && b != null)
                {
                    int result;

                    result = a.BooleanValue.CompareTo(b.BooleanValue);
                    if (result != 0) return result;

                    result = a.ByteValue.CompareTo(b.ByteValue);
                    if (result != 0) return result;

                    result = a.CharValue.CompareTo(b.CharValue);
                    if (result != 0) return result;

                    result = a.DateOnlyValue.CompareTo(b.DateOnlyValue);
                    if (result != 0) return result;

                    a.DateTimeValue = TruncateMilliseconds(a.DateTimeValue);
                    b.DateTimeValue = TruncateMilliseconds(b.DateTimeValue);
                    result = a.DateTimeValue.CompareTo(b.DateTimeValue);
                    if (result != 0) return result;

                    a.DateTimeOffsetValue = TruncateMilliseconds(a.DateTimeOffsetValue);
                    b.DateTimeOffsetValue = TruncateMilliseconds(b.DateTimeOffsetValue);
                    result = a.DateTimeOffsetValue.CompareTo(b.DateTimeOffsetValue);
                    if (result != 0) return result;

                    result = a.DecimalValue.CompareTo(b.DecimalValue);
                    if (result != 0) return result;

                    result = a.DoubleValue.CompareTo(b.DoubleValue);
                    if (result != 0) return result;

                    result = a.GuidValue.CompareTo(b.GuidValue);
                    if (result != 0) return result;

                    result = a.Int16Value.CompareTo(b.Int16Value);
                    if (result != 0) return result;

                    result = a.Int32Value.CompareTo(b.Int32Value);
                    if (result != 0) return result;

                    result = a.Int64Value.CompareTo(b.Int64Value);
                    if (result != 0) return result;

                    result = a.SByteValue.CompareTo(b.SByteValue);
                    if (result != 0) return result;

                    result = a.SingleValue.CompareTo(b.SingleValue);
                    if (result != 0) return result;

                    result = a.StringValue.CompareTo(b.StringValue);
                    if (result != 0) return result;

                    a.TimeOnlyValue = TruncateMilliseconds(a.TimeOnlyValue);
                    b.TimeOnlyValue = TruncateMilliseconds(b.TimeOnlyValue);
                    result = a.TimeOnlyValue.CompareTo(b.TimeOnlyValue);
                    if (result != 0) return result;

                    result = a.UInt16Value.CompareTo(b.UInt16Value);
                    if (result != 0) return result;

                    result = a.UInt32Value.CompareTo(b.UInt32Value);
                    if (result != 0) return result;

                    result = a.UInt64Value.CompareTo(b.UInt64Value);
                    return result;
                }

                if (a == null && b == null)
                    return 0;
                if (a == null)
                    return -1;
                return 1;
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

        [Test]
        public void TestIntrinsicDataConverters()
        {
            ObjectMappingTests x = new();
            CollectionAssert.AreEqual(AllTypesItems, ObjectMappingTests.WriteReadValues(AllTypesItems), new AllTypesComparer());
        }
    }
}
