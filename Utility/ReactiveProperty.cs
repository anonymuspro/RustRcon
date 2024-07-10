#region

using System;

#endregion

namespace RustRcon.Utility
{
    public class ReactiveProperty<T>
    {
        private T _value;
        public event Action<T> ValueChanged;

        public T Value
        {
            get => _value;
            set
            {
                if (!Equals(_value, value))
                {
                    _value = value;
                    ValueChanged?.Invoke(_value);
                }
            }
        }

        public ReactiveProperty(T initialValue = default)
        {
            _value = initialValue;
        }
    }
}