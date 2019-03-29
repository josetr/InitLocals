// Copyright (c) 2019 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

namespace InitLocals.Tests
{
    using System;
    using System.Linq;
    using Xunit;
    using static Util;

    public class Program
    {
        public static void Main()
        {
            True();
            False();
            FalseInheritedByClass.Check();
        }

        [InitLocals(true)]
        static void True()
        {
            FillWithGarbage();
            Span<byte> span = stackalloc byte[Size];
            Assert.True(span.ToArray().All(n => n == 0));
        }

        [InitLocals(false)]
        static void False()
        {
            FillWithGarbage();
            Span<byte> span = stackalloc byte[Size];
            Assert.Contains(Garbage, span.ToArray());
        }
    }

    [InitLocals(true)]
    public class TrueInheritedByClass
    {
        public static void Check()
        {
            FillWithGarbage();
            Span<byte> span = stackalloc byte[Size];
            Assert.True(span.ToArray().All(n => n == 0));
        }
    }

    [InitLocals(false)]
    public class FalseInheritedByClass
    {
        public static void Check()
        {
            FillWithGarbage();
            Span<byte> span = stackalloc byte[Size];
            Assert.Contains(Garbage, span.ToArray());
        }
    }

    public static class Util
    {
        public const int Size = 2048;
        public const byte Garbage = 125;

        public static void FillWithGarbage()
        {
            Span<byte> span = stackalloc byte[Size];
            span.Fill(Garbage);
        }
    }
}