namespace Scripts.Core.Networking
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    
    public enum RequestType
    {
        Weather,
        BreedsList,
        BreedDetail
    }

    public struct QueuedRequest
    {
        public RequestType Type;
        public Func<CancellationToken, UniTask> Action;
    }
    
    internal static class ConcurrentQueueExtensions
    {
        public static void Clear<T>(this ConcurrentQueue<T> queue)
        {
            while (queue.TryDequeue(out _)) { }
        }
        
        public static void TryRemoveWhere<T>(this ConcurrentQueue<T> queue, Func<T, bool> predicate)
        {
            var arr = queue.ToArray();
            queue.Clear();
            foreach (var item in arr)
                if (!predicate(item))
                    queue.Enqueue(item);
        }
    }
    public sealed class RequestQueue
    {
        private readonly ConcurrentQueue<QueuedRequest> _queue = new();
        private CancellationTokenSource _processingCts = new();
        private bool _isProcessing;

        public void Enqueue(RequestType type, Func<CancellationToken, UniTask> request)
        {
            _queue.Enqueue(new QueuedRequest { Type = type, Action = request });
            if (!_isProcessing)
                ProcessQueue().Forget();
        }

        public void CancelCurrentAndClear(Func<QueuedRequest, bool> predicate = null)
        {
            _processingCts.Cancel();
            if (predicate == null)
                _queue.Clear();
            else
                _queue.TryRemoveWhere(predicate);
        }

        private async UniTaskVoid ProcessQueue()
        {
            _isProcessing = true;
            while (_queue.TryDequeue(out var req))
            {
                _processingCts = new CancellationTokenSource();
                try { await req.Action(_processingCts.Token); }
                catch (OperationCanceledException) { }
            }
            _isProcessing = false;
        }
    }
}