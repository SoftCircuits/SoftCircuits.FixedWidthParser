// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace SoftCircuits.Parsers.Converters
{
    internal class CharConverter : DataConverter<Char>
    {
        public override string ConvertToString(Char value) => value.ToString();

        public override bool TryConvertFromString(string? s, out Char value) => Char.TryParse(s, out value);
    }
}
