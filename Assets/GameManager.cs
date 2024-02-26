using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<MouseAction> actions = new List<MouseAction>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < actions.Count; i++)
        {
            actions[i].OnMouseDownAction += Test;
        }
    }

    public void Test()
    {
        Debug.Log("click");
    }
}
