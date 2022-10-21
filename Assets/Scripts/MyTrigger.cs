using System;
using UnityEngine;

namespace Yaojz.UniTaskAsyncExtensions
{
    public class MyTrigger:MonoBehaviour
    {
        public int CustomId;
        public Action<Collider> OnTriggerEnterCallBack;
        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("on trigger enter");
            OnTriggerEnterCallBack?.Invoke(other);
        }
    }
}