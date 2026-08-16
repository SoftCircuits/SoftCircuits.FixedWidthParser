// Copyright (c) 2020-2026 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using SoftCircuits.Parsers;
using System.Text;

namespace FixedWidthParser.Tests;

public class DataExceptionTests
{
    public class Measurement
    {
        [FixedWidthField(4)]
        public string Code { get; set; } = string.Empty;
        [FixedWidthField(6)]
        public int Reading { get; set; }
    }

    /// <summary>
    /// When a field's content cannot be converted to the target member type, the reader
    /// should throw FixedWidthDataException if Options.ThrowDataException is true (the
    /// default), or silently leave the member at its default value if false.
    /// </summary>
    [Fact]
    public void InvalidFieldData_ThrowsOrIsIgnored_PerThrowDataExceptionOption()
    {
        // Hand-craft a line where "Reading" contains non-numeric data - "abcd" doesn't
        // parse as an int. Code is 4 chars, Reading is 6 chars.
        const string line = "A001abcd  ";

        MemoryFile memFile = new();
        using (StreamWriter seedWriter = new(memFile.GetStream(loadLastStreamContent: false), Encoding.UTF8, leaveOpen: true))
            seedWriter.WriteLine(line);

        Assert.Throws<FixedWidthDataException>(() =>
        {
            using FixedWidthReader<Measurement> reader = new(memFile.GetStream(), new FixedWidthOptions { ThrowDataException = true });
            reader.Read();
        });

        Measurement result;
        using (FixedWidthReader<Measurement> reader = new(memFile.GetStream(), new FixedWidthOptions { ThrowDataException = false }))
        {
            Assert.True(reader.Read());
            Assert.NotNull(reader.Item);
            result = reader.Item;
        }

        Assert.Equal("A001", result.Code);
        Assert.Equal(0, result.Reading); // left at the default int value rather than throwing
    }
}
