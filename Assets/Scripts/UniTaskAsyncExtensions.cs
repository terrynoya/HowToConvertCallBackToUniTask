using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

namespace Yaojz.UniTaskAsyncExtensions
{
    public static class UniTaskAsyncExtensions
    {
        public static UniTask OnStoppedAsync(this PlayableDirector director,CancellationToken cancellationToken = default,bool callOnce = true)
        {
            return new PlayableDirectorAsyncHandler(director,cancellationToken, callOnce).OnInvokeAsync();
        }
        
        public static UniTask<int> OnTriggerEnterAsync(this MyTrigger myTrigger,CancellationToken cancellationToken = default,bool callOnce = true)
        {
            return new MyTriggerEnterAsyncHandler(myTrigger,cancellationToken, callOnce).OnInvokeAsync();
        }
    }
}