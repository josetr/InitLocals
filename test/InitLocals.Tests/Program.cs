// Copyright (c) 2019 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

namespace InitLocals.Tests
{
    using System;
    using System.Linq;
    using static Util;

    public class Program
    {
        static void Main()
        {
            True();
            False();
            FalseInheritedByClass.Check();
            Console.WriteLine("Success!");
        }

        [InitLocals(false)]
        static void False()
        {
            FillWithGarbage();
            Span<byte> array = stackalloc byte[Size];
            if (!array.ToArray().Any(n => n != 0))
                throw new Exception("InitLocals(false) failed. Memory does not contain garbage.");
        }

        [InitLocals(true)]
        static void True()
        {
            FillWithGarbage();
            Span<byte> array = stackalloc byte[Size];
            if (!array.ToArray().All(n => n == 0))
                throw new Exception("InitLocals(true) failed. Memory is not zero initialized.");
        }
    }

    [InitLocals(false)]
    public class FalseInheritedByClass
    {
        public static void Check()
        {
            FillWithGarbage();
            Span<byte> array = stackalloc byte[Size];
            if (!array.ToArray().Any(n => n != 0))
                throw new Exception("InitLocals(false) failed. Memory does not contain garbage.");
        }
    }

    public static class Util
    {
        public const int Size = 2048;

        public static void FillWithGarbage()
        {
            Span<byte> array = stackalloc byte[Size];
            array.Fill(3);
        }
    }
}

