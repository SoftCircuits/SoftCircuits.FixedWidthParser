// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Collections.Generic;

namespace SoftCircuits.Parsers.Converters
{
    internal class DataConverters
    {
        private static readonly Dictionary<Type, Func<IDataConverter>> ConverterLookup = new()
        {
            [typeof(Boolean)] = () => new BooleanConverter(),
            [typeof(Byte)] = () => new ByteConverter(),
            [typeof(Char)] = () => new CharConverter(),
            [typeof(DateTime)] = () => new DateTimeConverter(),
            [typeof(DateTimeOffset)] = () => new DateTimeOffsetConverter(),
            [typeof(Decimal)] = () => new DecimalConverter(),
            [typeof(Double)] = () => new DoubleConverter(),
            [typeof(Guid)] = () => new GuidConverter(),
            [typeof(Int16)] = () => new Int16Converter(),
            [typeof(Int32)] = () => new Int32Converter(),
            [typeof(Int64)] = () => new Int64Converter(),
            [typeof(SByte)] = () => new SByteConverter(),
            [typeof(Single)] = () => new SingleConverter(),
            [typeof(String)] = () => new StringConverter(),
            [typeof(UInt16)] = () => new UInt16Converter(),
            [typeof(UInt32)] = () => new UInt32Converter(),
            [typeof(UInt64)] = () => new UInt64Converter(),
#if NET6_0_OR_GREATER
            [typeof(DateOnly)] = () => new DateOnlyConverter(),
            [typeof(TimeOnly)] = () => new TimeOnlyConverter(),
#endif
        };

        public static IDataConverter GetConverter(Type type)
        {
            if (ConverterLookup.TryGetValue(type, out Func<IDataConverter>? f))
                return f();
            return new UnsupportedTypeConverter(type);
        }
    }
}
