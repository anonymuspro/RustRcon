#region

using System;

#endregion

namespace RustRcon.Utility
{
    public class ReadOnlyReactiveProperty<T>
    {
        private readonly ReactiveProperty<T> _source;

        public T Value => _source.Value;

        public event Action<T> ValueChanged
        {
            add => _source.ValueChanged += value;
            remove => _source.ValueChanged -= value;
        }

        public ReadOnlyReactiveProperty(ReactiveProperty<T> source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
        }
    }
}