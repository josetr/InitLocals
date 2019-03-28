namespace InitLocalsSample
{
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
            Span<byte> array = stackalloc byte[Size];
            Debug.Assert(array.ToArray().All(n => n == 0));
        }

        [InitLocals(false)]
        static void False()
        {
            UseStack();
            Span<byte> array = stackalloc byte[Size];
            Debug.Assert(array.ToArray().Any(n => n != 0));
        }

        public static void UseStack()
        {
            Span<byte> array = stackalloc byte[Size];
            array.Fill(125);
        }
    }
}
