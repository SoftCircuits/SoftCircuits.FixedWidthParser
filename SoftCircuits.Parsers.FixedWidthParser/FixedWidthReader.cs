// Copyright (c) 2020-2021 Jonathan Wood (www.softcircuits.com)
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
        /// Returns the complete, unparsed line of text last read by the <see cref="Read(ref string[]?)"/>
        /// method. Valid only when <see cref="Read(ref string[]?)"/> returns true.
        /// </summary>
        public string? CurrentLine { get; private set; }

        /// <summary>
        /// Returns the column values read in the last call to <see cref="Read"/> or <see cref="ReadAsync"/>.
        /// May be null.
        /// </summary>
        public string[]? Columns { get; private set; }

        /// <summary>
        /// Returns true if the current file position is at the end of the file.
        /// </summary>
        public bool EndOfFile => Reader.EndOfStream;

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
        [Obsolete("This method is deprecated and will be removed in a future version of this library. Please use a version of Read() that take no parameters.")]
#if !NETSTANDARD2_0
        [MemberNotNullWhen(true, "CurrentLine")]
        public bool Read([NotNullWhen(true)] ref string[]? values)
#else
        public bool Read(ref string[] values)
#endif
        {
            values = Read();
            return (values != null);
        }

        /// <summary>
        /// Reads the fields from one line of data.
        /// </summary>
        /// <param name="values">Array to contain the fields. This method will allocate
        /// the array if this parameter is null or does not have the right number of
        /// elements.</param>
        /// <returns>The values read or null if the end of the file was reached.</returns>
        /// <exception cref="FixedWidthOutOfRangeException"></exception>
#if !NETSTANDARD2_0
        [MemberNotNullWhen(true, "CurrentLine")]
#endif
        public string[]? Read()
        {
            // Get next line
            CurrentLine = Reader.ReadLine();
            if (CurrentLine == null)
                return null;

            ReadLine();
            return Columns;
        }

        /// <summary>
        /// Asynchronously reads the fields from one line of data.
        /// </summary>
        /// <param name="values">Array to contain the fields. This method will allocate
        /// the array if this parameter is null or does not have the right number of
        /// elements.</param>
        /// <returns>The values read or null if the end of the file was reached.</returns>
        /// <exception cref="FixedWidthOutOfRangeException"></exception>
        public async Task<string[]?> ReadAsync()
        {
            // Get next line
            CurrentLine = await Reader.ReadLineAsync();
            if (CurrentLine == null)
                return null;

            ReadLine();
            return Columns;
        }

        /// <summary>
        /// Processes a single fixed-width line.
        /// </summary>
        private void ReadLine()
        {
            Debug.Assert(CurrentLine != null);

            // If needed, allocate array to return values
            if (Columns == null || Columns.Length != Fields.Count)
                Columns = new string[Fields.Count];

            // Read fields
            int position = 0;
            for (int i = 0; i < Fields.Count; i++)
            {
                position += Fields[i].Skip;
                Columns[i] = ReadField(CurrentLine, Fields[i], ref position);
            }
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
#if !NETSTANDARD2_0
                result = (position <= line.Length) ? line[position..] : string.Empty;
#else
                result = (position <= line.Length) ? line.Substring(position) : string.Empty;
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
