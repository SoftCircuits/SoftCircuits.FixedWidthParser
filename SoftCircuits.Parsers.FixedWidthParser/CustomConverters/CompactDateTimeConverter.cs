// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using System;
using System.Globalization;

namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Custom <see cref="DateTime"/> converter that stores the date and time in a compact format. The formatted value is
    /// 12 characters long. Seconds and milliseconds are not stored.
    /// </summary>
    public class CompactDateTimeConverter : DataConverter<DateTime>
    {
        private const string Format = "yyyyMMddHHmm";

        /// <inheritdoc/>
        public override string ConvertToString(DateTime value) => value.ToString(Format, CultureInfo.InvariantCulture);

        /// <inheritdoc/>
        public override bool TryConvertFromString(string? s, out DateTime value) => DateTime.TryParseExact(s, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out value);
    }
}
