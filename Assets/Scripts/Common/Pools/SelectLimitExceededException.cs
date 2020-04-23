using System;

namespace Assets.Scripts.Common.Pools
{
    public class SelectLimitExceededException : InvalidOperationException
    {
        private const string LimitExceededMessage = "The limit of select calls has been reached.";

        public SelectLimitExceededException() : base(LimitExceededMessage) { }
    }
}
