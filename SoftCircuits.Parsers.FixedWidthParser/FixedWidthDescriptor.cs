// Copyright (c) 2020-2026 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
namespace SoftCircuits.Parsers;

/// <summary>
/// Contains basic information about a fixed-width field.
/// </summary>
/// <remarks>
/// Custom converter classes can optionally define a constructor that accepts a
/// <see cref="FixedWidthDescriptor"/> parameter. These classes will receive an
/// instance of this class. It provides information about the field being converted.
/// </remarks>
public class FixedWidthDescriptor
{
    /// <summary>
    /// Gets the name of the fixed-width field.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the number of characters occupied by this field.
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// Gets the column alignment for this field.
    /// </summary>
    public FieldAlignment? Alignment { get; }

    /// <summary>
    /// Gets the character used to pad this field when writing values shorter than
    /// the field width.
    /// </summary>
    public char? PadCharacter { get; }

    /// <summary>
    /// <para>
    /// Gets whether leading and trailing pad characters are trimmed when reading
    /// field values.
    /// </para>
    /// </summary>
    public bool? TrimField { get; }

    /// <summary>
    /// Constructs a new <see cref="FixedWidthDescriptor"/> instance.
    /// </summary>
    /// <param name="name">The name of the fixed-width field.</param>
    /// <param name="fixedWidthField">The underlying <see cref="FixedWidthField"/> object.</param>
    /// <param name="options">The reader/writer options. Used to get default descriptor values.</param>
    internal FixedWidthDescriptor(string name, FixedWidthField fixedWidthField, FixedWidthOptions? options)
    {
        Name = name;
        Length = fixedWidthField.Length;
        if (options != null)
        {
            Alignment = fixedWidthField.Alignment ?? options.DefaultAlignment;
            PadCharacter = fixedWidthField.PadCharacter ?? options.DefaultPadCharacter;
            TrimField = fixedWidthField.TrimField ?? options.TrimFields;
        }
        else
        {
            Alignment = fixedWidthField.Alignment ?? FixedWidthOptions.InitDefaultAlignment;
            PadCharacter = fixedWidthField.PadCharacter ?? FixedWidthOptions.InitDefaultPadCharacter;
            TrimField = fixedWidthField.TrimField ?? FixedWidthOptions.InitTrimFields;
        }
    }
}
