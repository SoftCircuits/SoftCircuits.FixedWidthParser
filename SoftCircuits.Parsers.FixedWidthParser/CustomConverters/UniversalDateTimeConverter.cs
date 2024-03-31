﻿// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using System;
using System.Globalization;

namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Custom <see cref="DateTime"/> converter designed to work reliably across cultures.
    /// </summary>
    public class UniversalDateTimeConverter : DataConverter<DateTime>
    {
        private const string Format = "yyyy-MM-dd HH:mm:ss";

        /// <inheritdoc/>
        public override string ConvertToString(DateTime value) => value.ToString(Format, CultureInfo.InvariantCulture);

        /// <inheritdoc/>
        public override bool TryConvertFromString(string? s, out DateTime value) => DateTime.TryParseExact(s, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out value);
    }
}
