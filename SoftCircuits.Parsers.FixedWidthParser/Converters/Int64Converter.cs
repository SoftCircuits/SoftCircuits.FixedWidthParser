// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace SoftCircuits.Parsers.Converters
{
    internal class Int64Converter : DataConverter<Int64>
    {
        public override string ConvertToString(Int64 value) => value.ToString();

        public override bool TryConvertFromString(string? s, out Int64 value) => Int64.TryParse(s, out value);
    }
}
