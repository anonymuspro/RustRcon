using System;
using System.Runtime.CompilerServices;

namespace RustRcon.Types.Span
{
    public readonly ref struct SpanSplitter<T>
        where T : IEquatable<T>
    {
        private readonly ReadOnlySpan<T> _source;
        private readonly ReadOnlySpan<T> _separator;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SpanSplitter(ReadOnlySpan<T> source, ReadOnlySpan<T> separator)
        {
            if (0 == separator.Length)
            {
                throw new ArgumentException("Requires non-empty value", nameof(separator));
            }

            _source = source;
            _separator = separator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SpanSplitEnumerator<T> GetEnumerator()
        {
            return new SpanSplitEnumerator<T>(_source, _separator);
        }
    }
}