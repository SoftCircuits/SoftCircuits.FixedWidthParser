// Copyright (c) 2020 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Collections.Generic;

namespace SoftCircuits.Parsers.Converters
{
    internal class DataConverters
    {
        private static readonly Dictionary<Type, Func<IDataConverter>> ConverterLookup = new Dictionary<Type, Func<IDataConverter>>
        {
            [typeof(Boolean)] = () => new BooleanConverter(),
            [typeof(Byte)] = () => new ByteConverter(),
            [typeof(Char)] = () => new CharConverter(),
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
        };

        public static IDataConverter GetConverter(Type type)
        {
            return ConverterLookup.TryGetValue(type, out Func<IDataConverter> f) ?
                f() :
                new UnsupportedTypeConverter(type);
        }
    }
}
