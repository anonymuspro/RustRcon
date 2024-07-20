#region

using System.Collections;
using System.Collections.Generic;

#endregion

namespace RustRcon.Pooling
{
    public class PoolableList<T> : BasePoolable, IEnumerable<T>
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

        public T this[int index]
        {
            get => _internalList[index];
            set => _internalList[index] = value;
        }

        public int Count => _internalList.Count;

        public IEnumerator<T> GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected override void EnterPool()
        {
            base.EnterPool();

            _internalList.Clear();
        }
    }
}