// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace SoftCircuits.Parsers.Converters
{
    internal class UInt32Converter : DataConverter<UInt32>
    {
        public override string ConvertToString(UInt32 value) => value.ToString();

        public override bool TryConvertFromString(string? s, out UInt32 value) => UInt32.TryParse(s, out value);
    }
}
