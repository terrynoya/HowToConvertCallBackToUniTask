using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

namespace Yaojz.UniTaskAsyncExtensions
{
    public static class UniTaskAsyncExtensions
    {
        public static UniTask OnStoppedAsync(this PlayableDirector director,CancellationToken cancellationToken,bool callOnce = true)
        {
            return new PlayableDirectorAsyncHandler(director,cancellationToken, callOnce).OnInvokeAsync();
        }
    }
}