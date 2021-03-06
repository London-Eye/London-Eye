﻿using System;
using System.Collections.Generic;

namespace Assets.Scripts.Common.Pools
{
    public class SelectorPool<T>
    {
        /// <summary>
        /// The source pool to select values from.
        /// <para>If you change its elements (or set its value), the actual pool won't be updated until:</para>
        /// <para>- It is emptied (or you call <see cref="Empty"/>) (if <see cref="AutoRefill"/> is <see cref="true"/>)</para>
        /// <para>- The <see cref="Fill"/> method is called.</para>
        /// </summary>
        public HashSet<T> Pool { get; set; }

        /// <summary>
        /// When the actual pool gets emptied (either manually via <see cref="Empty"/>, or by getting all items from <see cref="Select"/>),
        /// the <see cref="Fill"/> method is automatically called.
        /// </summary>
        public bool AutoRefill { get; set; }


        /// <summary>
        /// (Optional) Add a limit for calls to <see cref="Select"/>. When reached, any further calls will result in an exception.
        /// </summary>
        public int? SelectLimit { get; set; }

        public int Count => currentPool.Count;

        private readonly Stack<T> currentPool = new Stack<T>();

        private int selectCalls = 0;

        public SelectorPool()
        {
            Pool = new HashSet<T>();
        }

        public SelectorPool(HashSet<T> pool)
        {
            Pool = pool;
        }

        public SelectorPool(IEnumerable<T> pool)
        {
            Pool = new HashSet<T>(pool);
        }

        public T Select()
        {
            selectCalls++;

            if (SelectLimit.HasValue && selectCalls > SelectLimit.Value)
            {
                throw new SelectLimitExceededException();
            }

            if (Count == 0)
            {
                if (AutoRefill) Fill();
                else throw new EmptyPoolException();
            }

            return currentPool.Pop();
        }

        public void ResetSelectCalls() => selectCalls = 0;

        public void Empty()
        {
            currentPool.Clear();

            if (AutoRefill)
            {
                Fill();
            }
        }

        public bool TryPushAndShuffle(T item)
        {
            bool pushed = TryPush(item);
            if (pushed) Shuffle();
            return pushed;
        }

        public bool TryPush(T item)
        {
            if (Pool.Contains(item))
            {
                currentPool.Push(item);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void PushAndShuffle(T item)
        {
            Push(item);
            Shuffle();
        }

        public void Push(T item)
        {
            if (Pool.Contains(item))
                currentPool.Push(item);
            else
                throw new ArgumentException($"The item is not in the {nameof(Pool)}");
        }

        public void Fill() => FillFrom(Pool);

        public void Shuffle() => FillFrom(currentPool);

        private void FillFrom(IEnumerable<T> pool)
        {
            var shuffledPool = pool.GetShuffle();

            if (Count > 0) currentPool.Clear();

            foreach (T item in shuffledPool)
            {
                currentPool.Push(item);
            }
        }
    }
}
