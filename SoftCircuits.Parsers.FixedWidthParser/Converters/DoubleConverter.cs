// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace SoftCircuits.Parsers.Converters
{
    internal class DoubleConverter : DataConverter<Double>
    {
        public override string ConvertToString(Double value) => value.ToString();

        public override bool TryConvertFromString(string? s, out Double value) => Double.TryParse(s, out value);
    }
}
