// Copyright (c) 2020-2026 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SoftCircuits.Parsers;

/// <summary>
/// Output line buffer used by <see cref="FixedWidthWriter"/> to format lines for writing.
/// </summary>
internal class WriteBuffer(int initialCapacity = 0)
{
    /// <summary>
    /// Line buffer.
    /// </summary>
    public char[] Buffer { get; private set; } = new char[initialCapacity];

    /// <summary>
    /// Buffer length.
    /// </summary>
    public int Length { get; private set; } = 0;

    /// <summary>
    /// Calculates the required buffer length based on the list of fields and initializes the buffer.
    /// The buffer is only reallocated if the current buffer is smaller than the required length.
    /// </summary>
    /// <param name="fields">The current list of fields.</param>
    public void InitializeBuffer(List<FixedWidthField> fields)
    {
        int length = FixedWidthField.CalculateLineLength(fields);
        if (Buffer.Length < length)
            Buffer = new char[length];
        Length = 0;
    }

    /// <summary>
    /// Adds a character to the line.
    /// </summary>
    public void Append(char c)
    {
        // Prevent overflowing buffer
        if (Length < Buffer.Length)
            Buffer[Length++] = c;
    }

    /// <summary>
    /// Adds a character to the line the number of specified times.
    /// </summary>
    public void Append(char c, int count)
    {
        Debug.Assert(count >= 0);

        // Prevent overflowing buffer
        if (Length + count > Buffer.Length)
            count = Buffer.Length - Length;

#if NETSTANDARD2_0
        for (int i = 0; i < count; i++)
            Buffer[Length++] = c;
#else
        Buffer.AsSpan(Length, count).Fill(c);
        Length += count;
#endif
    }

    /// <summary>
    /// Appends a string to the line.
    /// </summary>
    public void Append(string s)
    {
        Debug.Assert(s != null);

        int count = s.Length;

        // Prevent overflowing buffer
        if (Length + count > Buffer.Length)
            count = Buffer.Length - Length;

        s.CopyTo(0, Buffer, Length, count);
        Length += count;
    }

    /// <summary>
    /// Appends a section of a string to the line.
    /// </summary>
    public void Append(string s, int startIndex, int count)
    {
        Debug.Assert(s != null);
        Debug.Assert(startIndex >= 0);
        Debug.Assert(startIndex <= Buffer.Length);
        Debug.Assert(count >= 0);

        // Prevent overflowing buffer
        if (Length + count > Buffer.Length)
            count = Buffer.Length - Length;

        // Prevent overflowing string
        if (startIndex + count > s.Length)
            count = s.Length - startIndex;

        s.CopyTo(startIndex, Buffer, Length, count);
        Length += count;
    }
}
