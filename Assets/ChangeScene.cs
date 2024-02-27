using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using YF;

public class ChangeScene : MonoBehaviour
{
    public string sceneName;

    public void Change()
    {
        SceneMgr.Instance.LoadSceneAsyn(sceneName, null);
    }
}
