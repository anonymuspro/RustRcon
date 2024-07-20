#region

using System;
using RustRcon.Utility;

#endregion

namespace RustRcon.Pooling
{
    public class RustRconPool
    {
        private static readonly Hash<Type, IPool> Pools = new Hash<Type, IPool>();

        /// <summary>
        ///     Returns a pooled object of type T
        ///     Must inherit from <see cref="BasePoolable" /> and have an empty default constructor
        /// </summary>
        /// <typeparam name="T">Type to be returned</typeparam>
        /// <returns>Pooled object of type T</returns>
        public static T Get<T>() where T : BasePoolable, new()
        {
            return (T)ObjectPool<T>.Instance.Get();
        }

        /// <summary>
        ///     Returns a <see cref="BasePoolable" /> back into the pool
        /// </summary>
        /// <param name="value">Object to free</param>
        /// <typeparam name="T">Type of object being freed</typeparam>
        internal static void Free<T>(T value) where T : BasePoolable, new()
        {
            ObjectPool<T>.Instance.Free(value);
        }

        public static void AddPool<TType>(BasePool<TType> pool) where TType : class
        {
            Pools[typeof(TType)] = pool;
        }
    }
}