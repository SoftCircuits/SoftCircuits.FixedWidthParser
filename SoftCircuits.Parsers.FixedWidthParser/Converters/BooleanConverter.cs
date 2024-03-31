// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace SoftCircuits.Parsers.Converters
{
    internal class BooleanConverter : DataConverter<Boolean>
    {
        public override string ConvertToString(Boolean value) => value.ToString();

        public override bool TryConvertFromString(string? s, out Boolean value) => Boolean.TryParse(s, out value);
    }
}
