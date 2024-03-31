// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace SoftCircuits.Parsers.Converters
{
    internal class UInt64Converter : DataConverter<UInt64>
    {
        public override string ConvertToString(UInt64 value) => value.ToString();

        public override bool TryConvertFromString(string? s, out UInt64 value) => UInt64.TryParse(s, out value);
    }
}
