using System;
using System.Collections.Generic;

namespace Tetrominoes
{
    public class ObjectPool<T>
    {
        readonly Func<T> _factory;
        readonly Queue<T> _items = new Queue<T>();
        public ObjectPool(Func<T> factory) =>
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));

        public T Rent() =>
            _items.TryDequeue(out var item)
                ? item
                : _factory();
        public void Return(T item) =>
            _items.Enqueue(item);
    }
}
