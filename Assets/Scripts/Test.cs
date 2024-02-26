using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using YF;

public class Test : MonoBehaviour
{
    LinkedList<GameObject> tests = new LinkedList<GameObject>();
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SaveLocalConfig();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            LoadLocalConfig();
        }
    }

    public void LoadLocalConfig()
    {
        for (int i = 0; i < 5; i++)
        {
            UserData Data = LocalConfig.LoadUserData("testasdaawd" + i.ToString());
            Debug.Log(Data.name + "--" + Data.level);
        }
    }

    public void SaveLocalConfig()
    {
        for (int i = 0; i < 5; i++)
        {
            UserData userData = new UserData();
            userData.name = "testasdaawd" + i;
            userData.level = i;
            userData.pos = new Vector3(i, 2 * i, 3 * i);
            LocalConfig.SaveUserData(userData);
        }
        Debug.Log("保存成功");
    }
}

