// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace SoftCircuits.Parsers.Converters
{
    internal class Int16Converter : DataConverter<Int16>
    {
        public override string ConvertToString(Int16 value) => value.ToString();

        public override bool TryConvertFromString(string? s, out Int16 value) => Int16.TryParse(s, out value);
    }
}
