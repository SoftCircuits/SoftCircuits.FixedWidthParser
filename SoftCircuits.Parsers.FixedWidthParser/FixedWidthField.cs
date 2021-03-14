// Copyright (c) 2020-2021 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System.Diagnostics;

namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Defines a fixed-width field.
    /// </summary>
    public class FixedWidthField
    {
        /// <summary>
        /// Gets or sets the number of characters occupied by this field.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Gets or sets the column alignment for this field. Leave as <c>null</c> to use the
        /// default alignment.
        /// </summary>
        public FieldAlignment? Alignment { get; set; }

        /// <summary>
        /// Gets or sets the character used to pad this field when writing values shorter than
        /// the field width. Leave as <c>null</c> to use the default pad character.
        /// </summary>
        public char? PadCharacter { get; set; }

        /// <summary>
        /// <para>
        /// Gets or sets whether leading and trailing pad characters are trimmed when reading
        /// field values. Leave as <c>null</c> to use the default trim setting.
        /// </para>
        /// <para>
        /// WARNING: If this property is <c>true</c> and the field value contains leading or
        /// trailing characters that match the current pad character, those characters will
        /// also be trimmed.
        /// </para>
        /// </summary>
        public bool? TrimField { get; set; }

        /// <summary>
        /// Gets or sets the number of characters to skip before this field. Normally, this
        /// property is set to zero. You can use this property to skip fixed-width fields
        /// that you don't want to read. When writing fixed-width files, the character
        /// specified by <see cref="FixedWidthOptions.DefaultPadCharacter"/> will be written
        /// to fill the skipped characters. The default value is <c>0</c>.
        /// </summary>
        public int Skip { get; set; }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthField"/>.
        /// </summary>
        /// <param name="length">The number of characters that this field occupies.</param>
        /// <param name="alignment">The column alignment for this field. Leave as <c>null</c>
        /// to use the default alignment.</param>
        /// <param name="padCharacter">Character used to pad this field when writing values
        /// shorter than the field width. Leave as <c>null</c> to use the default pad
        /// character.</param>
        /// <param name="trimField">Specifies whether leading and trailing pad characters
        /// are trimmed when reading field values. Leave as <c>null</c> to use the default
        /// trim setting.</param>
        /// <param name="skip">Gets or sets the number of characters to skip before this
        /// field. Normally, this property is set to zero. You can use this property to skip
        /// fixed-width fields that you don't want to read. Leave as <c>0</c> to not skip
        /// any characters.</param>
        public FixedWidthField(int length, FieldAlignment? alignment = null, char? padCharacter = null, bool? trimField = null, int skip = 0)
        {
            Length = length;
            Alignment = alignment;
            PadCharacter = padCharacter;
            TrimField = trimField;
            Skip = skip;
        }

        internal FixedWidthField(FixedWidthFieldAttribute attribute)
        {
            Debug.Assert(attribute?.Field != null);
            Length = attribute.Field.Length;
            Alignment = attribute.Field.Alignment;
            PadCharacter = attribute.Field.PadCharacter;
            TrimField = attribute.Field.TrimField;
            Skip = attribute.Field.Skip;
        }
    }
}
