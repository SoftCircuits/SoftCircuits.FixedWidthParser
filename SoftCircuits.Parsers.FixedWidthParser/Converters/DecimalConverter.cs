// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace SoftCircuits.Parsers.Converters
{
    internal class DecimalConverter : DataConverter<Decimal>
    {
        public override string ConvertToString(Decimal value) => value.ToString();

        public override bool TryConvertFromString(string? s, out Decimal value) => Decimal.TryParse(s, out value);
    }
}
