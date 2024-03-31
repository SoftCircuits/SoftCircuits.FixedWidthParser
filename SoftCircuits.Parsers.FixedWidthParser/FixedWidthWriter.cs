// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Class to write fixed-width fields to a file.
    /// </summary>
    public class FixedWidthWriter : IDisposable
    {
        /// <summary>
        /// The options used by this writer.
        /// </summary>
        protected readonly FixedWidthOptions Options;

        /// <summary>
        /// The underlying <see cref="StreamReader"/> used by this reader.
        /// </summary>
        protected readonly StreamWriter Writer;

        /// <summary>
        /// Gets the list of fixed-width field descriptors.
        /// </summary>
        protected List<FixedWidthField> Fields;

        /// <summary>
        /// Constructs a new <see cref="FixedWidthWriter"/> instance.
        /// </summary>
        /// <param name="fields">List of fixed-width field descriptors.</param>
        /// <param name="filename">File to write.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FixedWidthWriter(IEnumerable<FixedWidthField> fields, string filename, FixedWidthOptions? options = null)
        {
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));
            if (filename == null)
                throw new ArgumentNullException(nameof(filename));

            Fields = new List<FixedWidthField>(fields);
            Writer = new StreamWriter(filename);
            Options = options ?? new FixedWidthOptions();
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthWriter"/> instance.
        /// </summary>
        /// <param name="fields">List of fixed-width field descriptors.</param>
        /// <param name="filename">File to write.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FixedWidthWriter(IEnumerable<FixedWidthField> fields, string filename, Encoding encoding, FixedWidthOptions? options = null)
        {
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));
            if (filename == null)
                throw new ArgumentNullException(nameof(filename));
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            Fields = new List<FixedWidthField>(fields);
            Writer = new StreamWriter(filename, false, encoding);
            Options = options ?? new FixedWidthOptions();
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthWriter"/> instance.
        /// </summary>
        /// <param name="fields">List of fixed-width field descriptors.</param>
        /// <param name="stream">Stream to write.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FixedWidthWriter(IEnumerable<FixedWidthField> fields, Stream stream, FixedWidthOptions? options = null)
        {
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            Fields = new List<FixedWidthField>(fields);
            Writer = new StreamWriter(stream);
            Options = options ?? new FixedWidthOptions();
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthWriter"/> instance.
        /// </summary>
        /// <param name="fields">List of fixed-width field descriptors.</param>
        /// <param name="stream">Stream to write.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FixedWidthWriter(IEnumerable<FixedWidthField> fields, Stream stream, Encoding encoding, FixedWidthOptions? options = null)
        {
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            Fields = new List<FixedWidthField>(fields);
            Writer = new StreamWriter(stream, encoding);
            Options = options ?? new FixedWidthOptions();
        }

        /// <summary>
        /// Writes one or more values as one line in the output file.
        /// </summary>
        /// <param name="args">The field values to write.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FixedWidthOverflowException"></exception>
        public void Write(params string[] args)
        {
            if (args != null)
            {
                FormatLine(args);
                Writer.WriteLine(LineBuffer, 0, LineLength);
            }
        }

        /// <summary>
        /// Asynchronously writes one or more values as one line in the output file.
        /// </summary>
        /// <param name="args">The field values to write.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FixedWidthOverflowException"></exception>
        public async Task WriteAsync(params string[] args)
        {
            if (args != null)
            {
                FormatLine(args);
                await Writer.WriteLineAsync(LineBuffer, 0, LineLength);
            }
        }

        /// <summary>
        /// Writes a collection of values to one line in the output file.
        /// </summary>
        /// <param name="values">The field values to write.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FixedWidthOverflowException"></exception>
        public void Write(IEnumerable<string>? values)
        {
            if (values != null)
            {
                FormatLine(values);
                Writer.WriteLine(LineBuffer, 0, LineLength);
            }
        }

        /// <summary>
        /// Asynchronously writes a collection of values to one line in the output file.
        /// </summary>
        /// <param name="values">The field values to write.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FixedWidthOverflowException"></exception>
        public async Task WriteAsync(IEnumerable<string>? values)
        {
            if (values != null)
            {
                FormatLine(values);
                await Writer.WriteLineAsync(LineBuffer, 0, LineLength);
            }
        }

        /// <summary>
        /// Formats a line for writing.
        /// </summary>
#if !NETSTANDARD2_0
        [MemberNotNull(nameof(LineBuffer))]
#endif
        private void FormatLine(IEnumerable<string> values)
        {
            InitLineBuffer(FixedWidthField.CalculateLineLength(Fields));
            IEnumerator<string> enumerator = values.GetEnumerator();
            foreach (FixedWidthField field in Fields)
                FormatField(enumerator.MoveNext() ? enumerator.Current : string.Empty, field);
        }

        /// <summary>
        /// Formats a single field for writing.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <param name="field">The descriptor for the current field.</param>
        /// <returns>The formatted string.</returns>
        private void FormatField(string value, FixedWidthField field)
        {
            Debug.Assert(LineBuffer != null);

            value ??= string.Empty;

            if (field.Skip > 0)
                Append(Options.DefaultPadCharacter, field.Skip);

            int padLength = field.Length - value.Length;
            if (padLength > 0)
            {
                FieldAlignment alignment = field.Alignment ?? Options.DefaultAlignment;
                char padCharacter = field.PadCharacter ?? Options.DefaultPadCharacter;
                if (alignment == FieldAlignment.Left)
                {
                    Append(value);
                    Append(padCharacter, padLength);
                }
                else // FieldAlignment.Right
                {
                    Append(padCharacter, padLength);
                    Append(value);
                }
            }
            else if (padLength < 0)
            {
                if (Options.ThrowOverflowException)
                    throw new FixedWidthOverflowException(value, field.Length);
                Append(value, 0, field.Length);
            }
            else Append(value);
        }

        #region Line Buffer

        private char[]? LineBuffer = null;
        private int LineLength = 0;

#if !NETSTANDARD2_0
        [MemberNotNull(nameof(LineBuffer))]
#endif
        private void InitLineBuffer(int lineLength)
        {
            if (LineBuffer == null || LineBuffer.Length < lineLength)
                LineBuffer = new char[lineLength];
            LineLength = 0;
        }

        /// <summary>
        /// Adds a character to the line.
        /// </summary>
        public void Append(char c)
        {
            Debug.Assert(LineBuffer != null);

            // Prevent overflowing buffer
            if (LineLength < LineBuffer.Length)
                LineBuffer[LineLength++] = c;
        }

        /// <summary>
        /// Adds a character to the line the number of specified times.
        /// </summary>
        private void Append(char c, int count)
        {
            Debug.Assert(LineBuffer != null);
            Debug.Assert(count >= 0);

            // Prevent overflowing buffer
            if (LineLength + count > LineBuffer.Length)
                count = LineBuffer.Length - LineLength;

            for (int i = 0; i < count; i++)
                LineBuffer[LineLength++] = c;
        }

        /// <summary>
        /// Appends a string to the line.
        /// </summary>
        public void Append(string s)
        {
            Debug.Assert(LineBuffer != null);
            Debug.Assert(s != null);

            int count = s.Length;

            // Prevent overflowing buffer
            if (LineLength + count > LineBuffer.Length)
                count = LineBuffer.Length - LineLength;

            for (int i = 0; i < count; i++)
                LineBuffer[LineLength++] = s[i];
        }

        /// <summary>
        /// Appends a section of a string to the line.
        /// </summary>
        public void Append(string s, int startIndex, int count)
        {
            Debug.Assert(LineBuffer != null);
            Debug.Assert(s != null);
            Debug.Assert(startIndex >= 0);
            Debug.Assert(startIndex <= LineBuffer.Length);
            Debug.Assert(count >= 0);

            // Prevent overflowing buffer
            if (LineLength + count > LineBuffer.Length)
                count = LineBuffer.Length - LineLength;

            // Prevent overflowing string
            if (startIndex + count > s.Length)
                count = s.Length - startIndex;

            for (int i = 0; i < count; i++)
                LineBuffer[LineLength++] = s[startIndex + i];
        }

        #endregion

        /// <summary>
        /// Clears all buffers and causes any unbuffered data to be written to the underlying stream.
        /// </summary>
        public void Flush() => Writer.Flush();

        /// <summary>
        /// Closes the current <see cref="FixedWidthWriter"/> object and the underlying stream.
        /// </summary>
        public void Close() => Writer.Close();

        /// <summary>
        /// Releases resources used by this <see cref="FixedWidthWriter"/> object.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Writer.Dispose();
        }
    }
}
