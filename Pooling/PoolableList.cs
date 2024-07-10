#region

using System.Collections.Generic;

#endregion

namespace RustRcon.Pooling
{
    public class PoolableList<T> : BasePoolable
    {
        private readonly List<T> _internalList;

        public PoolableList()
        {
            _internalList = new List<T>();
        }

        public void Add(T item)
        {
            _internalList.Add(item);
        }

        public bool Remove(T item)
        {
            return _internalList.Remove(item);
        }

        public int Count => _internalList.Count;

        public T this[int index]
        {
            get => _internalList[index];
            set => _internalList[index] = value;
        }

        protected override void EnterPool()
        {
            base.EnterPool();

            _internalList.Clear();
        }
    }
}