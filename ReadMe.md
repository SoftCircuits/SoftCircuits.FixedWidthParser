<a href="https://www.buymeacoffee.com/jonathanwood" target="_blank"><img src="https://www.buymeacoffee.com/assets/img/custom_images/black_img.png" alt="Buy Me A Coffee" style="height: 37px !important;width: 170px !important;" ></a>

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

// Write data to disk
// FixedWidthWriter.Write() is overloaded to also accept string[] and IEnumerable<string>
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
        // Do someting with values here
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

## FixedWidthWriter<T> and FixedWidthReader<T> Classes

These classes are used to write and read fixed-width files using a class to define the fields.

All properties and fields in the class with a `FixedWidthField` attribute will be written and/or read to the fixed width file. Note that the members don't have to be strings. All the basic data types are supported, including `Guid`. (Note that `DateTime` members are not currently supported due to the many ways they can be formatted, but you can work around this by writing custom data converters.)

```cs
// Declare our class with FixedWidthField attributes
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
        writer.WriteItem(product);
}

// Read the data back from the file
List<Product> results = new List<Product>();
using (FixedWidthReader<Product> reader = new FixedWidthReader<Product>(filename))
{
    while (reader.ReadItem(out Product item))
        results.Add(item);
}
```

