// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Globalization;

namespace SoftCircuits.Parsers.Converters
{

#if NET6_0_OR_GREATER

    internal class TimeOnlyConverter : DataConverter<TimeOnly>
    {
        private const string Format = "HH:mm:ss";

        public override string ConvertToString(TimeOnly value) => value.ToString(Format, CultureInfo.InvariantCulture);

        public override bool TryConvertFromString(string? s, out TimeOnly value) => TimeOnly.TryParseExact(s, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out value);
    }

#endif

}
