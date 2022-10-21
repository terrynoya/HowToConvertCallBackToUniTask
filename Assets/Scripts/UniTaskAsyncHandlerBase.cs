using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Yaojz.UniTaskAsyncExtensions
{
    public class UniTaskAsyncHandlerBase<TObject,TResult>:IUniTaskSource<TResult>
    {
        static Action<object> cancellationCallback = CancellationCallback;
        
        private CancellationToken cancellationToken;
        private CancellationTokenRegistration registration;
        
        private UniTaskCompletionSourceCore<TResult> core;
        bool isDisposed;
        bool callOnce;

        protected TObject _srcObject;
        
        public UniTaskAsyncHandlerBase(TObject srcObject,CancellationToken cancellationToken, bool callOnce)
        {
            this.cancellationToken = cancellationToken;
            if (cancellationToken.IsCancellationRequested)
            {
                isDisposed = true;
                return;
            }
            this.callOnce = callOnce;
            _srcObject = srcObject;
            OnCreate();
            
            if (cancellationToken.CanBeCanceled)
            {
                registration = cancellationToken.RegisterWithoutCaptureExecutionContext(cancellationCallback, this);
            }

            TaskTracker.TrackActiveTask(this, 3);
        }

        protected virtual void OnCreate()
        {
            
        }
        
        static void CancellationCallback(object state)
        {
            var self = (UniTaskAsyncHandlerBase<TObject,TResult>)state;
            self.Dispose();
        }
        
        public void Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;
                TaskTracker.RemoveTracking(this);
                registration.Dispose();
                OnDispose();
                core.TrySetCanceled(cancellationToken);
            }
        }

        protected virtual void OnDispose()
        {
            
        }

        protected virtual void TrySetResult(TResult result)
        {
            core.TrySetResult(result);
        }

        public UniTask<TResult> OnInvokeAsync()
        {
            core.Reset();
            if (isDisposed)
            {
                core.TrySetCanceled(this.cancellationToken);
            }
            return new UniTask<TResult>(this, core.Version);
        }

        public UniTaskStatus GetStatus(short token)
        {
            return core.GetStatus(token);
        }

        public void OnCompleted(Action<object> continuation, object state, short token)
        {
            core.OnCompleted(continuation, state, token);
        }

        public void GetResult(short token)
        {
            ((IUniTaskSource<TResult>)this).GetResult(token);
        }

        TResult IUniTaskSource<TResult>.GetResult(short token)
        {
            try
            {
                return core.GetResult(token);
            }
            finally
            {
                if (callOnce)
                {
                    Dispose();
                }
            }
        }

        public UniTaskStatus UnsafeGetStatus()
        {
            return core.UnsafeGetStatus();
        }
    }
}