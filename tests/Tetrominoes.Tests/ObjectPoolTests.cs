using System.Collections.Generic;
using Xunit;

namespace Tetrominoes.Tests
{
    public class ObjectPoolTests
    {
        [Fact]
        public void given_no_entries_item_should_come_from_factory()
        {
            var expected = new object();
            var queue = new Queue<object>();
            queue.Enqueue(expected);

            var pool = new ObjectPool<object>(queue.Dequeue);

            var actual = pool.Rent();

            Assert.Empty(queue);
            Assert.Same(expected, actual);
        }

        [Fact]
        public void given_entries_item_should_not_come_from_factory()
        {
            var expected = new object();
            var queue = new Queue<object>();
            queue.Enqueue(new object());

            var pool = new ObjectPool<object>(queue.Dequeue);
            pool.Return(expected);

            var actual = pool.Rent();

            Assert.NotEmpty(queue);
            Assert.Same(expected, actual);
        }
    }
}
