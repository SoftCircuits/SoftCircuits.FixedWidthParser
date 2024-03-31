// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Globalization;

namespace SoftCircuits.Parsers.Converters
{
    internal class DateTimeOffsetConverter : DataConverter<DateTimeOffset>
    {
        private const string Format = "yyyy-MM-dd HH:mm:ss zzz";

        public override string ConvertToString(DateTimeOffset value) => value.ToString(Format, CultureInfo.InvariantCulture);

        public override bool TryConvertFromString(string? s, out DateTimeOffset value) => DateTimeOffset.TryParseExact(s, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out value);
    }
}
