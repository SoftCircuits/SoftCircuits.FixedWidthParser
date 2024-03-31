// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
namespace SoftCircuits.Parsers.Converters
{
    internal class StringConverter : DataConverter<string>
    {
        public override string ConvertToString(string? value) => value ?? string.Empty;

        public override bool TryConvertFromString(string? s, out string value)
        {
            value = s ?? string.Empty;
            return true;
        }
    }
}
