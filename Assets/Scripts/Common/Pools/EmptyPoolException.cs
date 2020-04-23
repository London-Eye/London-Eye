using System;

namespace Assets.Scripts.Common.Pools
{
    public class EmptyPoolException : InvalidOperationException
    {
        private const string EmptyPoolMessage = "The pool is empty.";

        public EmptyPoolException() : base(EmptyPoolMessage) { }
    }
}
