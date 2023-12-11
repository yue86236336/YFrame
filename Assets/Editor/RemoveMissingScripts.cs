using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RemoveMissingScripts : Editor
{
    [MenuItem("Tools/移除场景中所有丢失的脚本")]
    public static void RemoveSceneAllMissingScript()
    {
        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
        {
            int v = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
            if (v > 0)
                Debug.LogFormat("<color=#00ff00>Name:{0} , Missing script remove success</color>", go.name);

        }
    }

    [MenuItem("GameObject/移除选中物体身上所有丢失的脚本", false)]
    public static void RemoveSelectedGameObjectMissingScript()
    {
        Transform[] transforms = Selection.GetTransforms(SelectionMode.Deep);
        foreach (Transform tf in transforms)
        {
            int v = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(tf.gameObject);
            if (v > 0)
            {
                Debug.LogFormat("<color=#00ff00>Name:{0} , Missing script remove success</color>", tf.name);
            }
        }
    }
}
