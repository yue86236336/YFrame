using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(TestNomono.Instance.Add(1, 2));
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log(TestMono.Instance.Change("aaasasdsf"));
        }
    }
}

