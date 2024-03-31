// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Globalization;

namespace SoftCircuits.Parsers.Converters
{

#if NET6_0_OR_GREATER

    internal class DateOnlyConverter : DataConverter<DateOnly>
    {
        private const string Format = "yyyy-MM-dd";

        public override string ConvertToString(DateOnly value) => value.ToString(Format, CultureInfo.InvariantCulture);

        public override bool TryConvertFromString(string? s, out DateOnly value) => DateOnly.TryParseExact(s, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out value);
    }

#endif

}
