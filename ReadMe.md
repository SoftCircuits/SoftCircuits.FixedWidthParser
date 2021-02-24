# Fixed-Width Parser

[![NuGet version (SoftCircuits.FixedWidthParser)](https://img.shields.io/nuget/v/SoftCircuits.FixedWidthParser.svg?style=flat-square)](https://www.nuget.org/packages/SoftCircuits.FixedWidthParser/)

```
Install-Package SoftCircuits.FixedWidthParser
```

## Overview

SoftCircuits.FixedWidthParser is a lightweight .NET class for reading and writing fixed-width data files. Includes basic reader and writer class, and also includes generic classes that automatically map class properties to fixed-width fields. Includes many options to control the library's behavior.

Fixed-width files are text files that contain data for one record on each line. Fields for each record are not delimited. Instead, each field has a fixed width, or length, and fields are found by their location within the line.

## FixedWidthWriter and FixedWidthReader Classes

These classes provide the simplest way to read and write fixed-width data files. The example below writes several rows of data to a fixed-width file and then reads it back.

```cs
// Define fixed-width fields
FixedWidthField[] PersonFields = new FixedWidthField[]
{
    new FixedWidthField(5),
    new FixedWidthField(10),
    new FixedWidthField(10),
};

// Write data to disk.
// FixedWidthWriter.Write() is overloaded to also accept string[] and IEnumerable<string>.
using (FixedWidthWriter writer = new FixedWidthWriter(PersonFields, filename))
{
    writer.Write("1", "Bill", "Smith");
    writer.Write("2", "Karen", "Williams");
    writer.Write("3", "Tom", "Phillips");
    writer.Write("4", "Jack", "Carpenter");
    writer.Write("5", "Julie", "Samson");
}

// Read the data from disk
using (FixedWidthReader reader = new FixedWidthReader(PersonFields, filename))
{
    // Array will be allocated if null or the wrong size
    string[] values = null;
    while (reader.Read(ref values))
    {
        // Do something with values here
    }
}
```

The code above writes and then reads the following file:

```
1    Bill      Smith     
2    Karen     Williams  
3    Tom       Phillips  
4    Jack      Carpenter 
5    Julie     Samson    
```

## FixedWidthWriter&lt;T&gt; and FixedWidthReader&lt;T&gt; Classes

These classes are used to write and read fixed-width files using a class to define the fields.

All properties and fields in the class with a `FixedWidthField` attribute will be written and/or read to the fixed width file. Note that the members don't have to be strings. All the basic data types are supported, including `Guid`. (Note that `DateTime` members are not currently supported due to the many ways they can be formatted, but you can work around this by writing custom data converters.)

```cs
// Declare our class with FixedWidthField attributes.
// Members without the FixedWidthField attribute will not be written or read.
private class Product
{
    [FixedWidthField(36)]
    public Guid Id { get; set; }
    [FixedWidthField(12)]
    public string Description { get; set; }
    [FixedWidthField(12)]
    public string Category { get; set; }
    [FixedWidthField(10)]
    public double Rating { get; set; }
}

// Create some data
private readonly List<Product> Products = new List<Product>
{
    new Product { Id = Guid.NewGuid(), Description = "Coffee Table", Category = "Furniture", Rating = 4.5 },
    new Product { Id = Guid.NewGuid(), Description = "Spoons", Category = "Utensils", Rating = 4.2 },
    new Product { Id = Guid.NewGuid(), Description = "Carpet", Category = "Flooring", Rating = 4.5 },
    new Product { Id = Guid.NewGuid(), Description = "Knives", Category = "Utensils", Rating = 4.7 },
    new Product { Id = Guid.NewGuid(), Description = "Recliner", Category = "Furniture", Rating = 4.5 },
    new Product { Id = Guid.NewGuid(), Description = "Floor Tiles", Category = "Flooring", Rating = 4.5 },
};

// Write the data to a file
using (FixedWidthWriter<Product> writer = new FixedWidthWriter<Product>(filename))
{
    foreach (var product in Products)
        writer.Write(product);
}

// Read the data back from the file
List<Product> results = new List<Product>();
using (FixedWidthReader<Product> reader = new FixedWidthReader<Product>(filename))
{
    while (reader.Read(out Product product))
        results.Add(product);
}
```

Here's the file created by the code above:

```
556348bf-058b-48fe-9a13-8ccc376f2e61Coffee TableFurniture   4.5       
3e00e345-1844-4842-b707-168767fb0d5fSpoons      Utensils    4.2       
aae72fec-80f0-48de-8cc5-cd08268624c9Carpet      Flooring    4.5       
ce2026bf-7401-47b2-a7ab-2202292a4425Knives      Utensils    4.7       
17f78e60-4909-4065-9574-d3f60ea55332Recliner    Furniture   4.5       
2d1d4c05-e05b-43dd-bfd5-c0998c4b8312Floor Tiles Flooring    4.5       
```

## Writing Custom Converters

If you have a class member of a type for which there is no built-in support, or if you want to customize the way a member is formatted, you can supply your own data conversion class.

Data conversion classes must implement the `IDataConverter` interface, but the easiest way to write a custom data converter in a type-safe manner is to derive your class from `DataConverter<T>`, where `T` is the type of the member you are converting. This class has two abstract members that you must implement in your derived class: `ConvertToString()` and `TryConvertFromString()`.

The following code reads and writes `Person` records, which contain a `DateTime` property. The `BirthdateConverter` class is used to provide data conversion support for the `DateTime` property. This is done by setting the `ConverterType` property of the `FixedWidthField` attribute.

```cs
// Define the Person class
private class Person
{
    [FixedWidthField(8)]
    public int Id { get; set; }
    [FixedWidthField(12)]
    public string FirstName { get; set; }
    [FixedWidthField(12)]
    public string LastName { get; set; }
    [FixedWidthField(12, ConverterType = typeof(BirthdateConverter))]
    public DateTime Birthdate { get; set; }
}

// Define our date converter class
private class BirthdateConverter : DataConverter<DateTime>
{
    private const string Format = "yyyyMMdd";

    public override string ConvertToString(DateTime value) => value.ToString(Format);

    public override bool TryConvertFromString(string s, out DateTime value)
    {
        return DateTime.TryParseExact(s, Format, null, System.Globalization.DateTimeStyles.None, out value);
    }
}

// Define some Person data
private readonly List<Person> People = new List<Person>
{
    new Person { Id = 1, FirstName = "Bill", LastName = "Smith", Birthdate = new DateTime(1982, 2, 7) },
    new Person { Id = 1, FirstName = "Gary", LastName = "Parker", Birthdate = new DateTime(1989, 8, 2) },
    new Person { Id = 1, FirstName = "Karen", LastName = "Wilson", Birthdate = new DateTime(1978, 6, 24) },
    new Person { Id = 1, FirstName = "Jeff", LastName = "Johnson", Birthdate = new DateTime(1972, 4, 18) },
    new Person { Id = 1, FirstName = "John", LastName = "Carter", Birthdate = new DateTime(1982, 12, 21) },
};

// Write the data to a file
using (FixedWidthWriter<Person> writer = new FixedWidthWriter<Person>(filename))
{
    foreach (var person in People)
        writer.Write(person);
}

// Read the data back from the file
List<Person> results = new List<Person>();
using (FixedWidthReader<Person> reader = new FixedWidthReader<Person>(filename))
{
    while (reader.Read(out Person person))
        results.Add(person);
}
```

Here's the file created by the code above:

```
1       Bill        Smith       19820207    
1       Gary        Parker      19890802    
1       Karen       Wilson      19780624    
1       Jeff        Johnson     19720418    
1       John        Carter      19821221    
```


## Additional Field Options

Whether you define your fields by declaring an array of `FixedWidthField`s or using the `FixedWidthField` attribute, there are a number of field options you can specify.

#### int Length

Gets or sets the number of characters occupied by this column.

#### FieldAlignment? Alignment

Gets or sets the column alignment for this field. Leave as `null` to use the default alignment.

#### char? PadCharacter

Gets or sets the character used to pad this field when writing values shorter than the field width. Leave as `null` to use the default pad character.

#### bool? TrimField

Gets or sets whether leading and trailing pad characters are trimmed when reading field values. Leave as `null` to use the default trim setting.

WARNING: If this property is <c>true</c> and the field value contains leading or trailing characters that match the current pad character, those characters will also be trimmed.

#### int Skip

Gets or sets the number of characters to skip before the field. Normally, this property is set to zero. You can use this property to skip fixed-width fields that you don't want to read. When writing fixed-width files, the character specified by `FixedWidthOptions.DefaultPadCharacter` will be written to fill the skipped characters. The default value is `0`.

#### Type ConverterType

Gets or sets the data type that converts this field to and from a string (as demonstrated previously). Must derive from `IDataConverter`. For best results and type safety, derive the class from `DataConverter<T>`.

This property is available with the `FixedWidthField` attribute and not the `FixedWidthField` class.

## Customizing the Library

All of the constructors for the `FixedWidthReader`, `FixedWidthReader<T>`, `FixedWidthWriter`, and `FixedWidthWriter<T>` class have an optional `FixedWidthOptions` parameter. To use the default options, leave this parameter as `null`. Provide your own instance of this class to customize the library settings.

```cs
// Set to right align, tilde padding and don't throw exception for invalid ata
FixedWidthOptions options = new FixedWidthOptions
{
    DefaultAlignment = FieldAlignment.Right,
    DefaultPadCharacter = '~',
    ThrowDataException = false
};

using (FixedWidthWriter<Product> writer = new FixedWidthWriter<Product>(filename, options))
{
    foreach (var product in Products)
        writer.Write(product);
}
```

The `FixedWidthOptions` class has the following properties.

#### FieldAlignment DefaultAlignment

Gets or sets the default way fields are padded. For example, if a field is right aligned, values shorter than the field width are padded on the left. Can be overridden for individual fields using the `FixedWidthField.Alignment` property. The default value is `FieldAlignment.Left`.

#### char DefaultPadCharacter

Gets or sets the default character used to pad fields when writing values shorter than the field width. Can be overridden for individual fields using the `FixedWidthField.PadCharacter` property. The default value is `' '`.

#### bool TrimFields

Gets or sets whether leading and trailing pad characters are trimmed when reading field values. Can be overridden for individual fields using the `FixedWidthField.TrimField` property. The default value is `true`.

WARNING: If this property is <c>true</c> and the field value contains leading or trailing characters that match the current pad character, those characters will also be trimmed.

#### bool ThrowDataException

Gets or sets whether a `FixedWidthDataException` is thrown when reading a field that cannot be converted to the target field type. The default value is `true`.

#### bool ThrowOutOfRangeException

Gets or sets whether a `FixedWidthOutOfRangeException` is thrown when reading a field from a line that is too short. If `false`, the library reads as much of the field as possible or returns an empty string. The default value is `true`.

#### bool ThrowOverflowException

Gets or sets whether a `FixedWidthOverflowException` is thrown when attempting to write a value that is too large for the field. If `false`, the value will be silently truncated. The default value is `true`.
