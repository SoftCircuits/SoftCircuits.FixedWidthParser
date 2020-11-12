// Copyright (c) 2020 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Defines a fixed-width field.
    /// </summary>
    public class FixedWidthField
    {
        /// <summary>
        /// Gets or sets the number of characters occupied by this column.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Gets or sets the column alignment for this field. Leave as <c>null</c> to use the
        /// default alignment.
        /// </summary>
        public FieldAlignment? Alignment { get; set; }

        /// <summary>
        /// Gets or sets the character used to pad this field when writing values shorter than the field width.
        /// Leave as <c>null</c> to use the default pad character.
        /// </summary>
        public char? PadCharacter { get; set; }

        /// <summary>
        /// <para>
        /// Gets or sets whether leading and trailing pad characters are trimmed when reading field values. Leave as
        /// <c>null</c> to use the default trim setting.
        /// </para>
        /// <para>
        /// WARNING: If this property is <c>true</c> and the field value contains leading or trailing characters that
        /// match the current pad character, those characters will also be trimmed.
        /// </para>
        /// </summary>
        public bool? TrimField { get; set; }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthField"/>.
        /// </summary>
        /// <param name="length">The number of characters that this field occupies.</param>
        /// <param name="trimField">Specifies if leading and trailing whitespace is removed
        /// from fields when reading them from a file.</param>
        /// <param name="padCharacter">Character used to pad values to fill field. Specify
        /// to override default pad character.</param>
        public FixedWidthField(int length)
        {
            Length = length;
            Alignment = null;
            PadCharacter = null;
            TrimField = null;
        }

        internal FixedWidthField(FixedWidthField field)
        {
            Length = field.Length;
            Alignment = field.Alignment;
            PadCharacter = field.PadCharacter;
            TrimField = field.TrimField;
        }
    }
}
