using System.Threading;
using UnityEngine;

namespace Yaojz.UniTaskAsyncExtensions
{
    public class MyTriggerEnterAsyncHandler:UniTaskAsyncHandlerBase<MyTrigger,int>
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            _srcObject.OnTriggerEnterCallBack += OnTrigger;
        }

        private void OnTrigger(Collider obj)
        {
            TrySetResult(_srcObject.CustomId);
        }

        public MyTriggerEnterAsyncHandler(MyTrigger srcObject, CancellationToken cancellationToken, bool callOnce) : base(srcObject, cancellationToken, callOnce)
        {
            
        }
    }
}