# InitLocals

[![#](https://img.shields.io/nuget/v/InitLocals.Fody.svg?style=flat)](http://www.nuget.org/packages/InitLocals.Fody/)
[![Build Status](https://josetr.visualstudio.com/InitLocals/_apis/build/status/InitLocals?branchName=master)](https://josetr.visualstudio.com/InitLocals/_build/latest?definitionId=12&branchName=master)

Controls whether the local variables in methods are zero-initialized.

## Usage

* Install package `InitLocals.Fody`.
* Mark your method/class/module with the `InitLocals` attribute.

## Sample

```cs
using System;
using System.Diagnostics;
using System.Linq;

public class Program
{
    public const int Size = 2048;

    static void Main()
    {
        True();
        False();
    }

    [InitLocals(true)]
    static void True()
    {
        UseStack();
        Span<byte> span = stackalloc byte[Size];
        Debug.Assert(span.ToArray().All(n => n == 0));
    }

    [InitLocals(false)]
    static void False()
    {
        UseStack();
        Span<byte> span = stackalloc byte[Size];
        Debug.Assert(span.ToArray().Any(n => n != 0));
    }

    public static void UseStack()
    {
        Span<byte> span = stackalloc byte[Size];
        span.Fill(125);
    }
}
```
