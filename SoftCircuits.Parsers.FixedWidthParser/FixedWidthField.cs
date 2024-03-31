// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Collections.Generic;
using System.Linq;

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
        /// Constructs a new <see cref="FixedWidthField"/> instance.
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
            Length = attribute.Field.Length;
            Alignment = attribute.Field.Alignment;
            PadCharacter = attribute.Field.PadCharacter;
            TrimField = attribute.Field.TrimField;
            Skip = attribute.Field.Skip;
        }

        /// <summary>
        /// Calculates the line length for the given set of <see cref="FixedWidthField"/>s.
        /// </summary>
        /// <param name="fields">The collection of <see cref="FixedWidthField"/>s from which to calculate the line length.</param>
        /// <returns>The calculated line length.</returns>
        internal static int CalculateLineLength(IEnumerable<FixedWidthField> fields) => fields.Sum(f => f.Skip + f.Length);

        #region Virtual Methods

        internal virtual string GetValue(object item) => throw new NotSupportedException();

        internal virtual void SetValue(object item, string s, bool throwExceptionOnInvalidData) => throw new NotSupportedException();

        #endregion

    }
}
