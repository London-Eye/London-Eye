using Assets.Scripts.Common;
using System.Collections.Generic;

namespace Assets.Scripts.Dialogue.Texts.Snippets
{
    public class SelectorPool<T>
    {
        public List<T> Pool { get; }

        private readonly Stack<T> currentPool;

        public SelectorPool()
        {
            Pool = new List<T>();
            currentPool = new Stack<T>();
        }

        public SelectorPool(IEnumerable<T> pool)
        {
            Pool = new List<T>(pool);
            currentPool = new Stack<T>();
        }

        public T Select()
        {
            if (currentPool.Count == 0)
            {
                Fill();
            }

            return currentPool.Pop();
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
