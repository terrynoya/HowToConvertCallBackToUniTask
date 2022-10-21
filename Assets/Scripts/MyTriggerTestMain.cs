using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Yaojz.UniTaskAsyncExtensions;

public class MyTriggerTestMain : MonoBehaviour
{
    public MyTrigger trigger;
    // Start is called before the first frame update
    
    void Start()
    {
        TriggerTest().Forget();
    }
    
    public async UniTask<int> TriggerTest()
    {
        var rlt = await trigger.OnTriggerEnterAsync();
        Debug.Log(rlt);
        return rlt;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
