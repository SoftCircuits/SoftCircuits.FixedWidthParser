// Copyright (c) 2020-2021 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Class to read fixed-width fields from a file.
    /// </summary>
    public class FixedWidthReader : IDisposable
    {
        private readonly StreamReader Reader;
        protected readonly FixedWidthOptions Options;

        /// <summary>
        /// Gets the list of fixed-width field descriptors.
        /// </summary>
        public List<FixedWidthField> Fields { get; private set; }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader"/> instance.
        /// </summary>
        /// <param name="fields">List of fixed-width field descriptors.</param>
        /// <param name="filename">File to read.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FixedWidthReader(IEnumerable<FixedWidthField> fields, string filename, FixedWidthOptions? options = null)
        {
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));
            if (filename == null)
                throw new ArgumentNullException(nameof(filename));

            Fields = new List<FixedWidthField>(fields);
            Reader = new StreamReader(filename);
            Options = options ?? new FixedWidthOptions();
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader"/> instance.
        /// </summary>
        /// <param name="fields">List of fixed-width field descriptors.</param>
        /// <param name="filename">File to read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FixedWidthReader(IEnumerable<FixedWidthField> fields, string filename, Encoding encoding, FixedWidthOptions? options = null)
        {
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));
            if (filename == null)
                throw new ArgumentNullException(nameof(filename));
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            Fields = new List<FixedWidthField>(fields);
            Reader = new StreamReader(filename, encoding);
            Options = options ?? new FixedWidthOptions();
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader"/> instance.
        /// </summary>
        /// <param name="fields">List of fixed-width field descriptors.</param>
        /// <param name="filename">File to read.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FixedWidthReader(IEnumerable<FixedWidthField> fields, string filename, bool detectEncodingFromByteOrderMarks, FixedWidthOptions? options = null)
        {
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));
            if (filename == null)
                throw new ArgumentNullException(nameof(filename));

            Fields = new List<FixedWidthField>(fields);
            Reader = new StreamReader(filename, detectEncodingFromByteOrderMarks);
            Options = options ?? new FixedWidthOptions();
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader"/> instance.
        /// </summary>
        /// <param name="fields">List of fixed-width field descriptors.</param>
        /// <param name="filename">File to read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FixedWidthReader(IEnumerable<FixedWidthField> fields, string filename, Encoding encoding, bool detectEncodingFromByteOrderMarks, FixedWidthOptions? options = null)
        {
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));
            if (filename == null)
                throw new ArgumentNullException(nameof(filename));
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            Fields = new List<FixedWidthField>(fields);
            Reader = new StreamReader(filename, encoding, detectEncodingFromByteOrderMarks);
            Options = options ?? new FixedWidthOptions();
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader"/> instance.
        /// </summary>
        /// <param name="fields">List of fixed-width field descriptors.</param>
        /// <param name="stream">Stream to read.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FixedWidthReader(IEnumerable<FixedWidthField> fields, Stream stream, FixedWidthOptions? options = null)
        {
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            Fields = new List<FixedWidthField>(fields);
            Reader = new StreamReader(stream);
            Options = options ?? new FixedWidthOptions();
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader"/> instance.
        /// </summary>
        /// <param name="fields">List of fixed-width field descriptors.</param>
        /// <param name="stream">Stream to read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FixedWidthReader(IEnumerable<FixedWidthField> fields, Stream stream, Encoding encoding, FixedWidthOptions? options = null)
        {
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            Fields = new List<FixedWidthField>(fields);
            Reader = new StreamReader(stream, encoding);
            Options = options ?? new FixedWidthOptions();
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader"/> instance.
        /// </summary>
        /// <param name="fields">List of fixed-width field descriptors.</param>
        /// <param name="stream">Stream to read.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FixedWidthReader(IEnumerable<FixedWidthField> fields, Stream stream, bool detectEncodingFromByteOrderMarks, FixedWidthOptions? options = null)
        {
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            Fields = new List<FixedWidthField>(fields);
            Reader = new StreamReader(stream, detectEncodingFromByteOrderMarks);
            Options = options ?? new FixedWidthOptions();
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader"/> instance.
        /// </summary>
        /// <param name="fields">List of fixed-width field descriptors.</param>
        /// <param name="stream">Stream to read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FixedWidthReader(IEnumerable<FixedWidthField> fields, Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, FixedWidthOptions? options = null)
        {
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            Fields = new List<FixedWidthField>(fields);
            Reader = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks);
            Options = options ?? new FixedWidthOptions();
        }

        /// <summary>
        /// Reads the fields from one line of data.
        /// </summary>
        /// <param name="values">Array to contain the fields. This method will allocate
        /// the array if this parameter is null or does not have the right number of
        /// elements.</param>
        /// <returns>True if successful, false if the end of the file was reached.</returns>
        /// <exception cref="FixedWidthOutOfRangeException"></exception>
#if NETSTANDARD2_0
        public bool Read(ref string[] values)
#else
        public bool Read([NotNullWhen(true)] ref string[]? values)
#endif
        {
            // Get next line
            string? line = Reader.ReadLine();
            if (line == null)
                return false;

            // If needed, allocate array to return values
            if (values == null || values.Length != Fields.Count)
                values = new string[Fields.Count];

            // Read fields
            int position = 0;
            for (int i = 0; i < Fields.Count; i++)
            {
                position += Fields[i].Skip;
                values[i] = ReadField(line, Fields[i], ref position);
            }

            return true;
        }

        /// <summary>
        /// Reads the specified field from the given line.
        /// </summary>
        /// <param name="line">The line that contains all the fields.</param>
        /// <param name="field">Field descriptor.</param>
        /// <param name="position">The current position within the liine.</param>
        /// <returns>The extracted field.</returns>
        private string ReadField(string line, FixedWidthField field, ref int position)
        {
            Debug.Assert(line != null);
            Debug.Assert(field != null);
            Debug.Assert(position >= 0);

            // Extract field
            string result;
            if (line.Length < position + field.Length)
            {
                if (Options.ThrowOutOfRangeException)
                    throw new FixedWidthOutOfRangeException();
#if NETSTANDARD2_0
                result = (position <= line.Length) ? line.Substring(position) : string.Empty;
#else
                result = (position <= line.Length) ? line[position..] : string.Empty;
#endif
            }
            else
            {
                result = line.Substring(position, field.Length);
            }

            // If requested, trim field
            if (field.TrimField ?? Options.TrimFields)
                result = result.Trim(field.PadCharacter ?? Options.DefaultPadCharacter);

            // Advance line position
            position += field.Length;

            return result;
        }

        /// <summary>
        /// Closes the <see cref="FixedWidthReader"/> object and the underlying stream, and releases
        /// any system resources associated with the reader.
        /// </summary>
        public void Close() => Reader.Close();

        /// <summary>
        /// Releases all resources used by the <see cref="FixedWidthReader"/> object.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Reader.Dispose();
        }
    }
}
