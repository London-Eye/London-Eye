using Assets.Scripts.Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Assets.Scripts.Dialogue.Texts.Snippets
{
    public class SelectorPool<T>
    {
        public IList<T> Pool
        {
            get => new ReadOnlyCollection<T>(pool);
            set
            {
                if (currentPool.Count == 0)
                {
                    pool = value;
                }
                else
                {
                    throw new System.InvalidOperationException("Cannot change the pool while the current one is not empty");
                }
            }
        }

        private readonly Stack<T> currentPool;
        private IList<T> pool;

        public SelectorPool()
        {
            pool = new List<T>();
            currentPool = new Stack<T>();
        }

        public SelectorPool(IEnumerable<T> startingPool)
        {
            pool = new List<T>(startingPool);
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

        public void Empty()
        {
            currentPool.Clear();
        }

        private void Fill()
        {
            foreach (T item in pool.GetShuffle())
            {
                currentPool.Push(item);
            }
        }
    }
}
