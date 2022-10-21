using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

namespace Yaojz.UniTaskAsyncExtensions
{
    public class PlayableDirectorAsyncHandler:IUniTaskSource
    {
        static Action<object> cancellationCallback = CancellationCallback;
        
        private CancellationToken cancellationToken;
        private CancellationTokenRegistration registration;
        
        private UniTaskCompletionSourceCore<bool> core;
        bool isDisposed;
        bool callOnce;

        private PlayableDirector _director;

        public PlayableDirectorAsyncHandler(PlayableDirector director,CancellationToken cancellationToken, bool callOnce)
        {
            this.cancellationToken = cancellationToken;
            if (cancellationToken.IsCancellationRequested)
            {
                isDisposed = true;
                return;
            }
            this.callOnce = callOnce;
            _director = director;
            _director.stopped += OnDirectorStopped;
            
            if (cancellationToken.CanBeCanceled)
            {
                registration = cancellationToken.RegisterWithoutCaptureExecutionContext(cancellationCallback, this);
            }

            TaskTracker.TrackActiveTask(this, 3);
        }
        
        static void CancellationCallback(object state)
        {
            var self = (PlayableDirectorAsyncHandler)state;
            self.Dispose();
        }

        private void OnDirectorStopped(PlayableDirector obj)
        {
            core.TrySetResult(true);
        }
        
        public void Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;
                TaskTracker.RemoveTracking(this);
                registration.Dispose();
                _director.stopped -= OnDirectorStopped;
                core.TrySetCanceled(cancellationToken);
            }
        }
        
        public UniTask OnInvokeAsync()
        {
            core.Reset();
            if (isDisposed)
            {
                core.TrySetCanceled(this.cancellationToken);
            }
            return new UniTask(this, core.Version);
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
            try
            {
                core.GetResult(token);
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