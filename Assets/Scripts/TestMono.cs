using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YF;

public class TestMono : MonoSingleton<TestMono>
{
    public string Change(string s)
    {
        return s.ToUpper();
    }

    public override void Awake()
    {
        base.Awake();
        Debug.Log("重写");
    }
}
