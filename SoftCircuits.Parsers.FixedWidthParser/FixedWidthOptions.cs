// Copyright (c) 2020-2025 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using System;

namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Specifies options used by the <see cref="FixedWidthReader"/> and
    /// <see cref="FixedWidthWriter"/> classes.
    /// </summary>
    public class FixedWidthOptions
    {
        /// <summary>
        /// Gets or sets the default way fields are padded. For example, if a field is right aligned, values shorter
        /// than the field width are padded on the left. Can be overridden for individual fields using the
        /// <see cref="FixedWidthField.Alignment"/> property. The default value is
        /// <see cref="FieldAlignment.Left"/>.
        /// </summary>
        public FieldAlignment DefaultAlignment { get; set; }

        /// <summary>
        /// Gets or sets the default character used to pad fields when writing values shorter than the field width.
        /// Can be overridden for individual fields using the <see cref="FixedWidthField.PadCharacter"/> property.
        /// The default value is <c>' '</c>.
        /// </summary>
        public char DefaultPadCharacter { get; set; }

        /// <summary>
        /// <para>
        /// Gets or sets whether leading and trailing pad characters are trimmed when reading field values. Can be
        /// overridden for individual fields using the <see cref="FixedWidthField.TrimField"/> property. The default
        /// value is <c>true</c>.
        /// </para>
        /// <para>
        /// WARNING: If this property is <c>true</c> and the field value contains leading or trailing characters that
        /// match the current pad character, those characters will also be trimmed.
        /// </para>
        /// </summary>
        public bool TrimFields { get; set; }

        /// <summary>
        /// <para>
        /// Gets or sets a function that determines whether a line should be ignored when reading a file. For example,
        /// you could use <c>IsIgnoredLine = s =&gt; s.Length &lt; 20</c> to ignore all lines shorter than 20 characters
        /// (including blank lines). This is helpful when reading files that can contain a mix of fixed-width fields and
        /// other data.
        /// </para>
        /// <para>
        /// The function should return <c>true</c> to ignore the line, or <c>false</c> to read it
        /// normally. The default value is <c>null</c>, which causes all lines to be read normally.
        /// </para>
        /// </summary>
        public Func<string, bool>? IsIgnoredLine { get; set; }

        /// <summary>
        /// Gets or sets whether a <see cref="FixedWidthDataException"/> exception is thrown when
        /// reading a field that cannot be converted to the target field type. The default
        /// value is <c>true</c>.
        /// </summary>
        public bool ThrowDataException;

        /// <summary>
        /// Gets or sets whether a <see cref="FixedWidthOutOfRangeException"/> exception is thrown when
        /// reading a field from a line that is too short. If <c>false</c>, the library reads
        /// as much of the field as possible or returns an empty string. The default value is
        /// <c>true</c>.
        /// </summary>
        public bool ThrowOutOfRangeException;

        /// <summary>
        /// Gets or sets whether a <see cref="FixedWidthOverflowException"/> exception is thrown when
        /// attempting to write a value that is too large for the field. If <c>false</c>,
        /// the value will be silently truncated. The default value is <c>true</c>.
        /// </summary>
        public bool ThrowOverflowException;

        /// <summary>
        /// Constructs a new <see cref="FixedWidthOptions"/> instance.
        /// </summary>
        public FixedWidthOptions()
        {
            DefaultAlignment = FieldAlignment.Left;
            DefaultPadCharacter = ' ';
            TrimFields = true;
            IsIgnoredLine = null;
            ThrowDataException = true;
            ThrowOutOfRangeException = true;
            ThrowOverflowException = true;
        }
    }
}
