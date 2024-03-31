// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace SoftCircuits.Parsers.Converters
{
    internal class UInt16Converter : DataConverter<UInt16>
    {
        public override string ConvertToString(UInt16 value) => value.ToString();

        public override bool TryConvertFromString(string? s, out UInt16 value) => UInt16.TryParse(s, out value);
    }
}
