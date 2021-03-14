// Copyright (c) 2020-2021 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Class to write fixed-width fields to a file.
    /// </summary>
    public class FixedWidthWriter : IDisposable
    {
        private readonly StreamWriter Writer;
        protected readonly FixedWidthOptions Options;

        /// <summary>
        /// Gets the list of fixed-width field descriptors.
        /// </summary>
        public List<FixedWidthField> Fields { get; private set; }

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
        /// Writes a collection of fields to one line in the output file.
        /// </summary>
        /// <param name="args">The field values to write.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FixedWidthOverflowException"></exception>
        public void Write(params string[] args) => Write(args as IEnumerable<string>);

        /// <summary>
        /// Writes a collection of fields to one line in the output file.
        /// </summary>
        /// <param name="values">The field values to write.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FixedWidthOverflowException"></exception>
        public void Write(IEnumerable<string>? values)
        {
            if (values == null)
                return;

            // Write fields to file
            IEnumerator<string> enumerator = values.GetEnumerator();
            foreach (FixedWidthField field in Fields)
            {
                if (field.Skip > 0)
                    Writer.Write(new string(Options.DefaultPadCharacter, field.Skip));
                string value = enumerator.MoveNext() ? enumerator.Current : string.Empty;
                Writer.Write(FormatField(value, field));
            }
            Writer.WriteLine();
        }

        /// <summary>
        /// Formats a single field for writing.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <param name="field">The descriptor for the current field.</param>
        /// <returns>The formatted string.</returns>
        private string FormatField(string value, FixedWidthField field)
        {
            if (value == null)
                value = string.Empty;

            if (value.Length < field.Length)
            {
                FieldAlignment alignment = field.Alignment ?? Options.DefaultAlignment;
                char padCharacter = field.PadCharacter ?? Options.DefaultPadCharacter;
                if (alignment == FieldAlignment.Left)
                    value = value.PadRight(field.Length, padCharacter);
                else // FieldAlignment.Right
                    value = value.PadLeft(field.Length, padCharacter);
            }
            else if (value.Length > field.Length)
            {
                if (Options.ThrowOverflowException)
                    throw new FixedWidthOverflowException(value, field.Length);
                value = value.Substring(0, field.Length);
            }
            return value;
        }

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
