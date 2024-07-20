using System;
using System.Runtime.CompilerServices;

namespace RustRcon.Types.Span
{
    public ref struct SpanSplitEnumerator<T>
        where T : IEquatable<T>
    {
        private int _nextStartIndex;
        private readonly ReadOnlySpan<T> _separator;
        private readonly ReadOnlySpan<T> _source;
        private SpanSplitValue _current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SpanSplitEnumerator(ReadOnlySpan<T> source, ReadOnlySpan<T> separator)
        {
            _nextStartIndex = 0;
            _source = source;
            _separator = separator;

            if (0 == separator.Length)
            {
                throw new ArgumentException("Requires non-empty value", nameof(separator));
            }

            _current = default;
        }

        public bool MoveNext()
        {
            if (_nextStartIndex > _source.Length)
            {
                return false;
            }

            var nextSource = _source.Slice(_nextStartIndex);

            var foundIndex = nextSource.IndexOf(_separator);

            var length = -1 < foundIndex
                ? foundIndex
                : nextSource.Length;

            _current = new SpanSplitValue(_nextStartIndex, length, _source);
            _nextStartIndex += _separator.Length + _current.Length;

            return true;
        }

        public SpanSplitValue Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _current;
        }

        public readonly ref struct SpanSplitValue
        {
            public SpanSplitValue(int startIndex, int length, ReadOnlySpan<T> source)
            {
                StartIndex = startIndex;
                Length = length;
                Source = source;
            }

            public int StartIndex { get;  }
            public int Length { get; }
            public ReadOnlySpan<T> Source { get;}

            public ReadOnlySpan<T> AsSpan() => Source.Slice(StartIndex, Length);

            public static implicit operator ReadOnlySpan<T>(SpanSplitValue value)
                => value.AsSpan();
        }
    }
}