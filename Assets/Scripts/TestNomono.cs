using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YF;

public class TestNomono : Singleton<TestNomono>
{
    public int Add(int a, int b)
    {
        return a + b;
    }
}
