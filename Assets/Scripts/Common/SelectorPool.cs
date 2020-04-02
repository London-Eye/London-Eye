using System.Collections.Generic;

namespace Assets.Scripts.Common
{
    public class SelectorPool<T>
    {
        /// <summary>
        /// The source pool to select values from.
        /// <para>If you change its elements (or set its value), the actual pool won't be updated until it is emptied (or you call <see cref="Empty"/>).</para>
        /// </summary>
        public List<T> Pool { get; set; }

        public int Count => currentPool.Count;

        private readonly Stack<T> currentPool = new Stack<T>();

        public SelectorPool()
        {
            Pool = new List<T>();
        }

        public SelectorPool(List<T> startingPool)
        {
            Pool = startingPool;
        }

        public T Select()
        {
            if (Count == 0)
            {
                Fill();
            }

            return currentPool.Pop();
        }

        public void Empty()
        {
            currentPool.Clear();
        }

        private void Fill()
        {
            foreach (T item in Pool.GetShuffle())
            {
                currentPool.Push(item);
            }
        }
    }
}
