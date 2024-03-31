// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using System;
using System.Globalization;

namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Custom <see cref="DateTime"/> converter that stores only the date portion of the value.
    /// </summary>
    public class DateOnlyDateTimeConverter : DataConverter<DateTime>
    {
        private const string Format = "yyyy-MM-dd";

        /// <inheritdoc/>
        public override string ConvertToString(DateTime value) => value.ToString(Format, CultureInfo.InvariantCulture);

        /// <inheritdoc/>
        public override bool TryConvertFromString(string? s, out DateTime value) => DateTime.TryParseExact(s, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out value);
    }
}
