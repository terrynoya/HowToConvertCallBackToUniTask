using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Yaojz.UniTaskAsyncExtensions;

public class MyTriggerTestMain : MonoBehaviour
{
    public MyTrigger trigger;
    // Start is called before the first frame update

    private CancellationTokenSource _cancellation;
    
    void Start()
    {
        _cancellation = new CancellationTokenSource();
        trigger.OnDestroyHandler += OnTriggerDestory;
        //TriggerTest(_cancellation.Token).Forget();
    }

    private void OnTriggerDestory()
    {
        Debug.Log("cancel");
        _cancellation.Cancel();   
    }

    public async UniTask<int> TriggerTest(CancellationToken cancellationToken)
    {
        // try
        // {
        //     var rlt = await trigger.OnTriggerEnterAsync(cancellationToken);
        // }
        // catch (OperationCanceledException e)
        // {
        //     Debug.Log(e);
        // }
        //return 1;
        var rlt = await trigger.OnTriggerEnterAsync(cancellationToken);
        Debug.Log(rlt);
        return rlt;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
