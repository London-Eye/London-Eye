using System.Collections.Generic;
using UnityEngine;

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
            IEnumerable<T> shuffledPool = ShufflePool();
            foreach (T item in shuffledPool)
            {
                currentPool.Push(item);
            }
        }

        private IEnumerable<T> ShufflePool()
        {
            T[] newArray = new T[Pool.Count];
            Pool.CopyTo(newArray);
            for (int i = 0; i < newArray.Length - 1; i++)
            {
                int r = Random.Range(i, newArray.Length);
                T tmp = newArray[i];
                newArray[i] = newArray[r];
                newArray[r] = tmp;
            }
            return newArray;
        }
    }
}
