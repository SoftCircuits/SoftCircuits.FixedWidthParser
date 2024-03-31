// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace SoftCircuits.Parsers.Converters
{
    internal class Int32Converter : DataConverter<Int32>
    {
        public override string ConvertToString(Int32 value) => value.ToString();

        public override bool TryConvertFromString(string? s, out Int32 value) => Int32.TryParse(s, out value);
    }
}
