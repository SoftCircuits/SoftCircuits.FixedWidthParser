// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace SoftCircuits.Parsers.Converters
{
    internal class SingleConverter : DataConverter<Single>
    {
        public override string ConvertToString(Single value) => value.ToString();

        public override bool TryConvertFromString(string? s, out Single value) => Single.TryParse(s, out value);
    }
}
