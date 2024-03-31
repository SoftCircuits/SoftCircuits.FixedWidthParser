// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace SoftCircuits.Parsers.Converters
{
    internal class GuidConverter : DataConverter<Guid>
    {
        public override string ConvertToString(Guid value) => value.ToString();

        public override bool TryConvertFromString(string? s, out Guid value) => Guid.TryParse(s, out value);
    }
}
