using System;
using System.Runtime.CompilerServices;
using RustRcon.Types.Span;

namespace RustRcon.Types
{
    public static class Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpanSplitter<T> Split<T>(this ReadOnlySpan<T> source, ReadOnlySpan<T> separator)
            where T : IEquatable<T>
        {
            return new SpanSplitter<T>(source, separator);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpanSplitter<T> Split<T>(this Span<T> source, ReadOnlySpan<T> separator)
            where T : IEquatable<T>
        {
            return new SpanSplitter<T>(source, separator);
        }
    }
}