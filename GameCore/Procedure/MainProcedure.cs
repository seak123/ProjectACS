using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainProcedure : IProcedure
{
    GameObject MainUI;
    public void OnEnter()
    {
        MainUI = ResourceManager.Instance.LoadUIPrefab(UIConst.MAIN_UI, UILayer.Normal_1);
    }

    public void OnUpdate()
    {

    }

    public void OnLeave()
    {
        GameObject.Destroy(MainUI);
    }
}
