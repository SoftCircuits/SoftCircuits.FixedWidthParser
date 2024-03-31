// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace SoftCircuits.Parsers.Converters
{
    internal class DateTimeConverter : DataConverter<DateTime>
    {
        public override string ConvertToString(DateTime value) => value.ToString();

        public override bool TryConvertFromString(string? s, out DateTime value) => DateTime.TryParse(s, out value);
    }
}
