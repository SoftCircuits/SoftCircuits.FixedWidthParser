// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace SoftCircuits.Parsers.Converters
{
    internal class SByteConverter : DataConverter<SByte>
    {
        public override string ConvertToString(SByte value) => value.ToString();

        public override bool TryConvertFromString(string? s, out SByte value) => SByte.TryParse(s, out value);
    }
}
