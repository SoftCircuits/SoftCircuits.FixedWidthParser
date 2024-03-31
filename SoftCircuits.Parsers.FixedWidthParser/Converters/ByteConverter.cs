// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace SoftCircuits.Parsers.Converters
{
    internal class ByteConverter : DataConverter<Byte>
    {
        public override string ConvertToString(Byte value) => value.ToString();

        public override bool TryConvertFromString(string? s, out Byte value) => Byte.TryParse(s, out value);
    }
}
